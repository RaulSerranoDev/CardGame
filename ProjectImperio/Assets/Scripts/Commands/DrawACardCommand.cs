using UnityEngine;
using DG.Tweening;

namespace Imperio
{
    /// <summary>
    /// Comando que roba una carta del mazo
    /// </summary>
    public class DrawACardCommand : Command
    {
        //Carta a robar
        private CardInfo cardInfo;

        //Jugador que roba la carta
        private Player player;

        //Instancia de la carta
        private GameObject GOcard = null;

        //Para instanciar un hueco vacío hasta que llega la carta al lugar
        private GameObject placeholder = null;

        /// <summary>
        /// Comando que roba una carta del mazo
        /// </summary>
        public DrawACardCommand(CardInfo card, Player player)
        {
            this.cardInfo = card;
            this.player = player;
        }

        /// <summary>
        /// Empieza la reproducción del comando: roba la carta
        /// Instancia una carta del tipo adecuado, la mete en la mano y hace la secuencia de dotween
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

            //Lo añadimos a la lista de cartas en la mano
            player.Hand.AddCard(newCard);

            //Creamos el hueco vacío
            placeholder = new GameObject();

            //Lo ponemos en la posición correspondiente de la zona
            placeholder.transform.SetParent(player.Hand.transform);

            //Queremos que ocupe lo mismo que la carta en el layout
            placeholder.AddComponent<RectTransform>();

            //Que vaya a la posición correspondiente.
            Vector3 offset = new Vector3(75.0f * (player.Hand.transform.childCount - 1), 0.0f, 0.0f);
            //Lo movemos a la mano
            GOcard.transform.DOMove(player.Hand.transform.position + offset, 0.1f).SetEase(Ease.OutQuint).onComplete += OnEndDraw;
        }

        /// <summary>
        /// Cuando acaba la secuencia de robo de DOTween
        /// Destruye el placeholder, termina el comando y actualiza las cartas jugables
        /// </summary>
        private void OnEndDraw()
        {
            GameObject.Destroy(placeholder);
            GOcard.transform.SetParent(player.Hand.transform);

            CommandComplete();

            if (GameManager.Instance.WhoseTurn == player)
                player.Hand.HighlightPlayableCards();
        }
    }
}