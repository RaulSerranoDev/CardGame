using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using MoreMountains.Tools;

namespace Imperio
{
    /// <summary>
    /// Construye un diccionario con la información del XML de todas las cartas
    /// Construye los mazos predefinidos de los jugadores
    /// </summary>
    public class TextManager : PersistentSingleton<TextManager>//Hace que sea Singleton
    {
        /// <summary>
        /// Xml que queremos leer
        /// </summary>
        [SerializeField]
        private TextAsset gameAsset;

        /// <summary>
        /// Diccionario con todas las cartas que leemos del XML
        /// </summary>
        public Dictionary<int, CardInfo> Cards { get; set; }

        /// <summary>
        /// Mazos de la jugadores
        /// Decks[0] -> Player 1
        /// Decks[1] -> Player 2
        /// </summary>
        public List<CardInfo>[] Decks { get; set; }

        protected override void Awake()
        {
            //Construye el Singleton
            base.Awake();

            //Cargamos el XML
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(gameAsset.text);

            ReadCards(xmlDoc);

            ReadDecks(xmlDoc);
        }

        /// <summary>
        /// Parsea el XML. Llena el diccionario de CardInfos.
        /// Lee tanto Minions como Spells
        /// </summary>
        void ReadCards(XmlDocument xmlDoc)
        {
            Cards = new Dictionary<int, CardInfo>();

            //Cogemos el array de nodos de minionCard
            XmlNodeList minionsList = xmlDoc.GetElementsByTagName("minionCard");

            //Leemos todos los minions de la lista
            foreach (XmlNode minion in minionsList)
            {
                //Creamos un objeto CardInfo, que insertaremos en el diccionario
                CardInfo cardInfo = new CardInfo();

                cardInfo.Type = CardType.MINION;
                cardInfo.ID = int.Parse(minion.Attributes["key"].Value);

                //Array de atributos de minionCard
                XmlNodeList attributesList = minion.ChildNodes;

                //Leemos todos los atributos y los insertamos en cardInfo
                foreach (XmlNode attribute in attributesList)
                {
                    if (attribute.Name == "object")
                    {
                        switch (attribute.Attributes["name"].Value)
                        {
                            //Atributos comunes
                            case "Name":
                                cardInfo.Name = attribute.InnerText;
                                break;
                            case "Description":
                                cardInfo.Description = attribute.InnerText;
                                break;
                            case "GoldCost":
                                cardInfo.GoldCost = int.Parse(attribute.InnerText);
                                break;
                            case "Image":
                                Sprite sprite = Resources.Load<Sprite>(attribute.InnerText);//TODO: Hacerlo de otra manera más adecuada               
                                cardInfo.Image = sprite;
                                break;

                            //Atributos específicos
                            case "Attack":
                                cardInfo.Attack = int.Parse(attribute.InnerText);
                                break;
                            case "Health":
                                cardInfo.Health = int.Parse(attribute.InnerText);
                                break;
                            case "Mechanics":
                                string[] mechanics = attribute.InnerText.Split(',');
                                for (int i = 0; i < mechanics.Length; i++)
                                {
                                    switch (mechanics[i])
                                    {
                                        case "TAUNT":
                                            cardInfo.Taunt = true;
                                            break;
                                        case "CHARGE":
                                            cardInfo.Charge = true;
                                            break;
                                        case "STEALTH":
                                            cardInfo.Stealth = true;
                                            break;
                                        case "WINDFURY":
                                            cardInfo.Windfury = true;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                        }
                    }

                }

                //Añadimos la carta al diccionario de Cartas
                Cards.Add(cardInfo.ID, cardInfo);
            }

            //Cogemos el array de nodos de spellCard
            XmlNodeList spellsList = xmlDoc.GetElementsByTagName("spellCard"); // array of the level nodes.

            //Leemos todas las spells de la lista
            foreach (XmlNode spell in spellsList)
            {
                //Creamos un objeto CardInfo, que insertaremos en el diccionario
                CardInfo cardInfo = new CardInfo();

                cardInfo.Type = CardType.SPELL;
                cardInfo.ID = int.Parse(spell.Attributes["key"].Value);

                //Array de atributos de spellCard
                XmlNodeList attributeList = spell.ChildNodes;

                //Leemos todos los atributos y los insertamos en cardInfo
                foreach (XmlNode attribute in attributeList)
                {
                    switch (attribute.Attributes["name"].Value)
                    {
                        //Atributos comunes
                        case "Name":
                            cardInfo.Name = attribute.InnerText;
                            break;
                        case "Description":
                            cardInfo.Description = attribute.InnerText;
                            break;
                        case "GoldCost":
                            cardInfo.GoldCost = int.Parse(attribute.InnerText);
                            break;
                        case "Image":
                            Sprite sprite = Resources.Load<Sprite>(attribute.InnerText); //TODO: Hacerlo de otra manera más adecuada                  
                            cardInfo.Image = sprite;
                            break;

                        //Atributos específicos
                        case "SpellTarget":
                            switch (attribute.InnerText)
                            {
                                case "NoTarget":
                                    cardInfo.SpellTarget = SpellTarget.NOTARGET;
                                    cardInfo.IsTargeted = false;
                                    break;

                                //Target único
                                case "Minion":
                                    cardInfo.SpellTarget = SpellTarget.MINION;
                                    cardInfo.IsTargeted = true;
                                    break;
                                case "AllyMinion":
                                    cardInfo.SpellTarget = SpellTarget.ALLYMINION;
                                    cardInfo.IsTargeted = true;
                                    break;
                                case "EnemyMinion":
                                    cardInfo.SpellTarget = SpellTarget.ENEMYMINION;
                                    cardInfo.IsTargeted = true;
                                    break;
                                case "Character":
                                    cardInfo.SpellTarget = SpellTarget.CHARACTER;
                                    cardInfo.IsTargeted = true;
                                    break;
                                case "AllyCharacter":
                                    cardInfo.SpellTarget = SpellTarget.ALLYCHARACTER;
                                    cardInfo.IsTargeted = true;
                                    break;
                                case "EnemyCharacter":
                                    cardInfo.SpellTarget = SpellTarget.ENEMYCHARACTER;
                                    cardInfo.IsTargeted = true;
                                    break;

                                //Target múltiple
                                case "AllMinions":
                                    cardInfo.SpellTarget = SpellTarget.ALLMINIONS;
                                    cardInfo.IsTargeted = false;
                                    break;
                                case "AllAllyMinions":
                                    cardInfo.SpellTarget = SpellTarget.ALLALLYMINIONS;
                                    cardInfo.IsTargeted = false;
                                    break;
                                case "AllEnemyMinions":
                                    cardInfo.SpellTarget = SpellTarget.ALLENEMYMINIONS;
                                    cardInfo.IsTargeted = false;
                                    break;
                                case "AllCharacters":
                                    cardInfo.SpellTarget = SpellTarget.ALLCHARACTERS;
                                    cardInfo.IsTargeted = false;
                                    break;
                                case "AllAllyCharacters":
                                    cardInfo.SpellTarget = SpellTarget.ALLALLYCHARACTERS;
                                    cardInfo.IsTargeted = false;
                                    break;
                                case "AllEnemyCharacters":
                                    cardInfo.SpellTarget = SpellTarget.ALLENEMYCHARACTERS;
                                    cardInfo.IsTargeted = false;
                                    break;
                            }
                            break;

                        case "HealthEffect":
                            cardInfo.Effect.Health = int.Parse(attribute.InnerText);
                            break;
                        case "MaxHealthEffect":
                            cardInfo.Effect.MaxHealth = int.Parse(attribute.InnerText);
                            break;
                        case "AttackEffect":
                            cardInfo.Effect.Attack = int.Parse(attribute.InnerText);
                            break;

                        case "MechanicsEffect":
                            string[] mechanics = attribute.InnerText.Split(',');
                            for (int i = 0; i < mechanics.Length; i++)
                            {
                                switch (mechanics[i])
                                {
                                    case "TAUNT":
                                        cardInfo.Effect.Taunt = true;
                                        break;
                                    case "CHARGE":
                                        cardInfo.Effect.Charge = true;
                                        break;
                                    case "STEALTH":
                                        cardInfo.Effect.Stealth = true;
                                        break;
                                    case "WINDFURY":
                                        cardInfo.Effect.Windfury = true;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;

                    }
                }

                //Añadimos la carta al diccionario de Cartas
                Cards.Add(cardInfo.ID, cardInfo);
            }
        }

        /// <summary>
        /// Parsea el XML. Llena los mazos de los jugadores
        /// </summary>
        void ReadDecks(XmlDocument xmlDoc)
        {
            //Cogemos el array de nodos de decks
            XmlNodeList decksList = xmlDoc.GetElementsByTagName("deck");

            Decks = new List<CardInfo>[2];
            int ID = 0;

            //Leemos todos los decks
            foreach (XmlNode deck in decksList)
            {
                //Creamos una lista de CardInfo, que contendrá el mazo
                Decks[ID] = new List<CardInfo>();

                //Array de atributos de Deck
                XmlNodeList cards = deck.ChildNodes;

                foreach (XmlNode card in cards)
                {
                    int numCopies = 0;
                    int IDCard = 0;

                    //Array de atributos de minionCard
                    XmlNodeList levelObject = card.ChildNodes;

                    //Leemos la carta y el número de copias
                    foreach (XmlNode levelsItens in levelObject)
                    {
                        switch (levelsItens.Attributes["name"].Value)
                        {
                            case "ID":
                                IDCard = int.Parse(levelsItens.InnerText);
                                break;
                            case "Copies":
                                numCopies = int.Parse(levelsItens.InnerText);
                                break;
                        }
                    }

                    //Añadimos al mazo la carta el número de veces especificada
                    for (int i = 0; i < numCopies; i++)
                        Decks[ID].Add(Cards[IDCard]);
                }
                ID++;
            }
        }
    }
}