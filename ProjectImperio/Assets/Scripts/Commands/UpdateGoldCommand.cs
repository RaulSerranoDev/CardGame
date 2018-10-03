namespace Imperio
{
    /// <summary>
    /// Comando que actualiza el contador de oro
    /// </summary>
    public class UpdateGoldCommand : Command
    {
        private Gold _gold;

        /// <summary>
        /// Comando que actualiza el contador de oro
        /// </summary>
        public UpdateGoldCommand(Gold gold)
        {
            _gold = gold;
        }

        /// <summary>
        /// Cambia el texto del oro
        /// </summary>
        public override void CommandStart()
        {
            _gold.GoldText.text = _gold.CurrentGold.ToString() + " Gold";
            CommandComplete();
        }
    }
}