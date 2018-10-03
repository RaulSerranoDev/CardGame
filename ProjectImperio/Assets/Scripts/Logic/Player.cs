using UnityEngine;
using System.Collections.Generic;

namespace Imperio
{
    /// <summary>
    /// Clase jugador. Tiene todas las listas de cartas
    /// </summary>
    public class Player : MonoBehaviour
    {
        //-----------------ATRIBUTOS INSPECTOR-----------------

        //Listas
        public Deck Deck;
        public Hand Hand;
        public Battlefield PlayerBattlefield;

        //Atributos
        public Gold Gold;
        public Fort Fort;

        //TODO: Hacerlo de mejor manera
        public Canvas Canvas;

        //------------Propiedades---------------------

        //Devuelve el contrincante
        public Player OtherPlayer
        {
            get { return GameManager.Instance.Players[(index + 1) % 2]; }
        }

        //------------Propiedades---------------------

        //------------ATRIBUTOS PRIVADOS---------------------

        private int index;
        private int activeForts;
        private int actualDamageByFatigue;

        //------------ATRIBUTOS PRIVADOS---------------------

        //public Graveyard PlayerGraveyard;
        //public Fort LeftFort { get; set; }
        //public Fort RightFort { get; set; }
        //public CharacterScript CharacterAsset { get; set; }
        //Lista de secretos, armas

        /// <summary>
        /// Inicializa las listas y atributos
        /// </summary>
        /// <param name="ind"></param>
        /// <param name="initialDeck"></param>
        public void BuildPlayer(int ind, List<CardInfo> initialDeck)
        {
            index = ind;

            //Construcción de las listas
            Deck.BuildDeck(initialDeck);
            Hand.BuildHand();
            PlayerBattlefield.BuildBattlefield();

            //Construcción de atributos
            Gold.BuildGold(this);
            Fort.BuildFort(this);

            activeForts = 1;
            actualDamageByFatigue = 1;
        }

        /// <summary>
        /// Es llamado cuando empieza el turno. Recolecta oro, roba una carta e informa a los minions de que ha empezado un nuevo turno
        /// </summary>
        public void OnTurnStart()
        {
            new ShowMessageCommand("Turno del player: " + index, 2.0f).EnqueueComand();

            //Aumenta la cantidad de oro correspondiente
            Gold.OnTurnStart();

            DrawACard();

            //Informamos al Battlefield de que ha empezado el turno
            PlayerBattlefield.OnTurnStart();
        }

        /// <summary>
        /// Es llamado cuando acaba el turno del jugador.
        /// Actualiza las cartas jugables e informa al battlefield
        /// </summary>
        public void OnTurnEnd()
        {
            Hand.HighlightPlayableCards();

            PlayerBattlefield.OnTurnEnd();
        }

        /// <summary>
        /// Roba una carta del mazo
        /// </summary>
        public void DrawACard()
        {
            //Hay cartas en el mazo
            if (Deck.CanDraw())
            {
                CardInfo cardInfo = Deck.DrawCard();

                //No tengo la mano llena : Robamos la carta
                if (Hand.CanTakeCard())
                    new DrawACardCommand(cardInfo, this).EnqueueComand();

                //Quemar la carta
                else
                    new BurnCardCommand(cardInfo, this).EnqueueComand();
            }

            //No hay cartas en el mazo: fatiga
            else        
                new FatigueCommand(this).EnqueueComand();
            
        }

        /// <summary>
        /// Crea un minion en la mesa a partir de una carta arrastrada
        /// </summary>
        /// <param name="playedCard"></param>
        /// <param name="tablePos"></param>
        public void PlayAMinionFromHand(MinionCard minionCard, int tablePos)
        {
            Gold.AddGold(-minionCard.CurrentGoldCost);

            //Borramos la carta de la mano
            Hand.RemoveCard(minionCard);
            Destroy(minionCard.gameObject);

            //TODO: Minion al instante en battlefield?

            new PlayAMinionCommand(this, minionCard, tablePos).EnqueueComand();
        }

        /// <summary>
        /// Juega un hechizo en un minion
        /// </summary>
        /// <param name="playedCard"></param>
        /// <param name="tablePos"></param>
        public void PlayATargetSpellFromHand(SpellCard spellCard, Minion minion)
        {
            Gold.AddGold(-spellCard.CurrentGoldCost);

            //Borramos la carta de la mano
            Hand.RemoveCard(spellCard);
            Destroy(spellCard.gameObject);

            minion.ApplyEffect(spellCard.Effect);
        }

        /// <summary>
        /// Juega un hechizo en un fuerte
        /// </summary>
        /// <param name="playedCard"></param>
        /// <param name="tablePos"></param>
        public void PlayATargetSpellFromHand(SpellCard spellCard, Fort fort)
        {
            Gold.AddGold(-spellCard.CurrentGoldCost);

            //Borramos la carta de la mano
            Hand.RemoveCard(spellCard);
            Destroy(spellCard.gameObject);

            fort.ApplyEffect(spellCard.Effect);
        }

        /// <summary>
        /// Juega un hechizo sin target
        /// </summary>
        /// <param name="playedCard"></param>
        /// <param name="tablePos"></param>
        public void PlayANoTargetSpellFromHand(SpellCard spellCard)
        {
            Gold.AddGold(-spellCard.CurrentGoldCost);

            //Borramos la carta de la mano
            Hand.RemoveCard(spellCard);
            Destroy(spellCard.gameObject);

            Effect effect = spellCard.Effect;
            switch (spellCard.SpellTarget)
            {
                case SpellTarget.ALLALLYCHARACTERS:
                    Fort.ApplyEffect(effect);
                    PlayerBattlefield.ApplyEffect(effect);
                    break;
                case SpellTarget.ALLALLYMINIONS:
                    PlayerBattlefield.ApplyEffect(effect);
                    break;
                case SpellTarget.ALLCHARACTERS:
                    Fort.ApplyEffect(effect);
                    PlayerBattlefield.ApplyEffect(effect);
                    OtherPlayer.Fort.ApplyEffect(effect);
                    OtherPlayer.PlayerBattlefield.ApplyEffect(effect);
                    break;
                case SpellTarget.ALLENEMYCHARACTERS:
                    OtherPlayer.Fort.ApplyEffect(effect);
                    OtherPlayer.PlayerBattlefield.ApplyEffect(effect);
                    break;
                case SpellTarget.ALLENEMYMINIONS:
                    OtherPlayer.PlayerBattlefield.ApplyEffect(effect);
                    break;
                case SpellTarget.ALLMINIONS:
                    PlayerBattlefield.ApplyEffect(effect);
                    OtherPlayer.PlayerBattlefield.ApplyEffect(effect);
                    break;
                case SpellTarget.NOTARGET:
                    //TODO: MONEDA DEL PLAYER
                    break;

            }
        }

        /// <summary>
        /// Se le llama cuando se pierde un fuerte. Detecta si es el final de la partida
        /// </summary>
        /// <param name="fort"></param>
        public void LostFort(Fort fort)
        {
            activeForts--;
            if (activeForts <= 1)
                new GameOverCommand(this).EnqueueComand();
        }

        /// <summary>
        /// Es llamado desde DrawFatigueCardCommand
        /// </summary>
        public void OnDrawFatigueCard()
        {
            Fort.Health -= actualDamageByFatigue;
            actualDamageByFatigue++;
        }

        //TODO: Meter carta que no es del mazo
        //TODO: Crea un minion en la mesa a partir de un ID

    }
}