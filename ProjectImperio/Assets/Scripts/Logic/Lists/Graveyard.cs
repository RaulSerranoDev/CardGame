/*
using System.Collections.Generic;

namespace Imperio
{
    /// <summary>
    /// 
    /// </summary>
    public class Graveyard
    {
        public List<Card> Cards { get; set; }

        public Graveyard()
        {
            Cards = new List<Card>();
        }

        //Ordena las cartas aleatoriamente
        public void Shuffle()
        {
            Cards.OrderBy(x => Guid.NewGuid());
        }

        //Añade una carta en la última posición
        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public bool CanAddCard()
        {
            return (Cards.Count < Player.MAXDECK);
        }

        //Coge la carta de la cima y la elimina del mazo
        public Card GetCard()
        {
            if (Cards.Count > 0)
            {
                int index = Cards.Count - 1;

                Card card = Cards.ElementAtOrDefault(index);
                Cards.RemoveAt(index);

                return card;
            }
            return null;
        }

        public bool IsEmpty()
        {
            return (Cards.Count == 0);
        }
    }
}
*/