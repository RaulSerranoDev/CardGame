using UnityEngine;
using DG.Tweening;

namespace Imperio
{
    /// <summary>
    /// Comando que roba una carta de fatiga del mazo.
    /// Hace daño al jugador cuando acaba de robar
    /// </summary>
    public class FatigueCommand : Command
    {
        private Player player;              //Jugador que roba la carta
        private GameObject GOcard = null;   //Instancia de la carta

        /// <summary>
        /// Comando que roba una carta de fatiga del mazo.
        /// </summary>
        public FatigueCommand(Player player)
        {
            this.player = player;
        }

        /// <summary>
        /// Empieza la reproducción del comando: roba la carta de fatiga
        /// Instancia la "carta" de fatiga y hace una animación. Cuando esta acaba hace daño al jugador.
        /// </summary>
        public override void CommandStart()
        {
            GOcard = GameObject.Instantiate(GameManager.Instance.FatigueCard, player.Deck.transform);

            //Para que se renderice por encima de todo
            GOcard.transform.SetParent(GameManager.Instance.GlobalCanvas.transform);

            //La escalamos
            GOcard.transform.DOScale(3.0f, 2.0f).onComplete += OnEndScale; 
        }

        /// <summary>
        /// Cuando acaba la secuencia de escala de DOTween.
        /// Destruye la carta y hace daño al jugador.
        /// </summary>
        private void OnEndScale()
        {
            GameObject.Destroy(GOcard);
            player.OnDrawFatigueCard();

            CommandComplete();
        }
    }
}