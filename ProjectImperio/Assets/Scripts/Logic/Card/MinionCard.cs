using UnityEngine;
using UnityEngine.UI;

namespace Imperio
{
    /// <summary>
    /// Clase base del objeto MinionCard
    /// Tiene los atributos propios de Minions
    /// </summary>
    public class MinionCard : Card
    {
        //------------Atributos inspector-------------

        [Header("Minion Card References")]
        [SerializeField] private Text attackText;
        [SerializeField] private Text healthText;

        //------------Atributos inspector-------------

        //------------Propiedades-------------

        public int Attack { get; set; }
        public int MaxHealth { get; set; }

        public bool Taunt { get; set; }
        public bool Charge { get; set; }
        public bool Stealth { get; set; }
        public bool Windfury { get; set; }

        //------------Propiedades-------------

        /// <summary>
        /// Lee el CardInfo y establece su relación con el Canvas
        /// </summary>
        protected override void ReadCardFromInfo()
        {
            base.ReadCardFromInfo();

            attackText.text = CardInfo.Attack.ToString();
            healthText.text = CardInfo.Health.ToString();

            Attack = CardInfo.Attack;
            MaxHealth = CardInfo.Health;

            Taunt = CardInfo.Taunt;
            Charge = CardInfo.Charge;
            Stealth = CardInfo.Stealth;
            Windfury = CardInfo.Windfury;
        }

        /// <summary>
        /// Comprueba si hay espacio en el tablero además de comprobar si hay oro y es su turno.
        /// </summary>
        /// <returns></returns>
        public override bool CanBePlayed()
        {          
            bool field = (Owner.PlayerBattlefield.CanPlayMinion());
            return (field && base.CanBePlayed());
        }
    }
}