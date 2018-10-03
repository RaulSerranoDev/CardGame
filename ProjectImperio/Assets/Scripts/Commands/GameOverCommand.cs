using DG.Tweening;

namespace Imperio
{
    /// <summary>
    /// Comando que acaba la partida porque un jugador ha muerto
    /// </summary>
    public class GameOverCommand : Command {

        private Player player;

        public GameOverCommand(Player player)
        {
            this.player = player;
        }

        /// <summary>
        /// Empieza la reproducción del comando: termina el juego
        /// </summary>
        public override void CommandStart()
        {
            //TODO: BLOQUEAR ACCIONES DE LOS JUGADORES
            GameManager.Instance.StopTheTimer();
            GameManager.Instance.GameOver = true;

            player.Fort.gameObject.transform.DOShakeScale(3.5f).SetEase(Ease.OutQuint).onComplete += OnEndAnim;
        }

        void OnEndAnim()
        {
            GameManager.Instance.ReloadScene();
        }
    }
}
