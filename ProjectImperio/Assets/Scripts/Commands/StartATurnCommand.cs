namespace Imperio
{
    /// <summary>
    /// Comando que asigna el jugador actual
    /// </summary>
    public class StartATurnCommand : Command
    {
        private Player _player;

        /// <summary>
        /// Comando que asigna el jugador actual
        /// </summary>
        public StartATurnCommand(Player p)
        {
            this._player = p;
        }

        public override void CommandStart()
        {
            GameManager.Instance.WhoseTurn = _player;
            CommandComplete();
        }
    }
}