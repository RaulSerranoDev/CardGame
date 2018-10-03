using UnityEngine;

namespace Imperio
{
    /// <summary>
    /// Tipo de la carta
    /// </summary>
    public enum CardType { MINION, SPELL }

    /// <summary>
    /// Tipo de target de la carta en caso de que sea un hechizo
    /// </summary>
    public enum SpellTarget
    {
        NOTARGET,

        //Target único
        MINION,
        ALLYMINION,
        ENEMYMINION,
        CHARACTER,
        ALLYCHARACTER,
        ENEMYCHARACTER,

        //Target múltiple
        ALLMINIONS,
        ALLALLYMINIONS,
        ALLENEMYMINIONS,
        ALLCHARACTERS,
        ALLALLYCHARACTERS,
        ALLENEMYCHARACTERS
    }

    /// <summary>
    /// Actua como una plantilla de todos los datos que queremos guardar en la carta
    /// </summary>
    public class CardInfo
    {
        //Atributos comunes de la carta
        public string Name;
        public string Description;
        public int GoldCost;
        public Sprite Image;
        public int ID;
        public CardType Type;

        //Atributos de Minion
        public int Attack;
        public int Health;
        public bool Taunt;
        public bool Charge;
        public bool Stealth;
        public bool Windfury;

        //Atributos de Spell
        public SpellTarget SpellTarget;
        public Effect Effect;
        public bool IsTargeted;

        public CardInfo()
        {
            //Valores base
            Attack = Health = 0;
            Taunt = Charge = Stealth = Windfury = false;

            SpellTarget = SpellTarget.NOTARGET;
            Effect = new Effect();
        }
    }
}