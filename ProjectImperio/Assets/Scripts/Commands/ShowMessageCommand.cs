namespace Imperio
{
    /// <summary>
    /// Comando que muestra un mensaje con un panel en la duración establecida
    /// </summary>
    public class ShowMessageCommand : Command
    {
        private string message;
        private float duration;

        /// <summary>
        /// Comando que muestra un mensaje con un panel en la duración establecida
        /// </summary>
        public ShowMessageCommand(string message, float duration)
        {
            this.message = message;
            this.duration = duration;
        }

        /// <summary>
        /// Empieza la reproducción del comando: Instancia el mensaje
        /// </summary>
        public override void CommandStart()
        {
            MessageManager.Instance.ShowMessage(message, duration);
        }
    }
}