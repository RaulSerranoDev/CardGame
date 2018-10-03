using UnityEngine;
using DG.Tweening;

namespace Imperio
{
    /// <summary>
    /// Comando que roba una carta del mazo y la destruye, por no haber espacio en la mano
    /// </summary>
    public class BurnCardCommand : Command
    {
        //Carta a robar
        private CardInfo cardInfo;

        //Jugador que roba la carta
        private Player player;

        //Instancia de la carta
        private GameObject GOcard = null;

        /// <summary>
        /// Comando que roba una carta del mazo y la destruye, por no haber espacio en la mano
        /// </summary>
        public BurnCardCommand(CardInfo card, Player player)
        {
            this.cardInfo = card;
            this.player = player;
        }

        /// <summary>
        /// Empieza la reproducción del comando: roba la carta
        /// Instancia una carta del tipo adecuado, y la destruye cuando acaba la secuencia
        /// </summary>
        public override void CommandStart()
        {
            //Distinguimos que prefab tenemos que instanciar. Lo instanciamos desde el mazo
            switch (cardInfo.Type)
            {
                case CardType.MINION:
                    GOcard = GameObject.Instantiate(GameManager.Instance.MinionCardPrefab, player.Deck.transform);
                    break;
                case CardType.SPELL:
                    if (cardInfo.IsTargeted)
                        GOcard = GameObject.Instantiate(GameManager.Instance.TargetSpellCardPrefab, player.Deck.transform);
                    else
                        GOcard = GameObject.Instantiate(GameManager.Instance.NoTargetSpellCardPrefab, player.Deck.transform);
                    break;

            }

            //Construimos el componente de la carta
            Card newCard = GOcard.GetComponent<Card>();
            newCard.BuildCard(cardInfo);
            newCard.Owner = player;

            GOcard.transform.SetParent(GameManager.Instance.GlobalCanvas.transform);

            //La escalamos
            GOcard.transform.DOScale(3.0f, 2.0f).onComplete += OnEndDraw;
        }

        /// <summary>
        /// Cuando acaba la secuencia de escalar de DOTween
        /// Termina el comando
        /// </summary>
        private void OnEndDraw()
        {
            GameObject.Destroy(GOcard);
            CommandComplete();
        }
    }
}