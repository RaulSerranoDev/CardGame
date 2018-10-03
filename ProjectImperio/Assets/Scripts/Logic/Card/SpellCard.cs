namespace Imperio
{
    /// <summary>
    /// Clase base del Objeto SpellCard.
    /// Tiene los atributos propios de un hechizo.
    /// </summary>
    public class SpellCard : Card
    {
        public Effect Effect { get; set; }
        public SpellTarget SpellTarget { get; set; }
        public bool IsTargeted { get; set; }

        /// <summary>
        /// Lee el CardInfo y establece su relación con el Canvas
        /// </summary>
        protected override void ReadCardFromInfo()
        {
            base.ReadCardFromInfo();

            Effect = CardInfo.Effect;
            SpellTarget = CardInfo.SpellTarget;
            IsTargeted = CardInfo.IsTargeted;
        }

    }
}