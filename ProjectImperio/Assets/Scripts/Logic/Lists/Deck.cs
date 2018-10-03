using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Imperio
{
    /// <summary>
    /// Lista de cartas del mazo y métodos auxiliares
    /// </summary>
    public class Deck : MonoBehaviour
    {
        /// <summary>
        /// Semilla random para ordenar aleatoriamente
        /// </summary>
        private static System.Random rng = new System.Random();

        /// <summary>
        /// Lista de cartas del mazo
        /// </summary>
        private List<CardInfo> cards;

        /// <summary>
        /// Inicializa el mazo y baraja las cartas
        /// </summary>
        /// <param name="initialDeck"></param>
        public void BuildDeck(List<CardInfo> initialDeck)
        {
            cards = initialDeck.ToList();
            Shuffle();
        }

        /// <summary>
        /// Ordena las cartas aleatoriamente
        /// </summary>
        public void Shuffle()
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                CardInfo value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }

        }

        /// <summary>
        /// Coge la primera carta de la lista y la elimina del mazo
        /// </summary>
        /// <returns></returns>
        public CardInfo DrawCard()
        {
            CardInfo card = cards.FirstOrDefault();
            cards.RemoveAt(0);
            return card;
        }

        /// <summary>
        /// Devuelve si podemos robar una carta
        /// </summary>
        /// <returns></returns>
        public bool CanDraw()
        {
            return (cards.Count > 0);
        }

        ///// <summary>
        ///// Añade una carta en posición aleatoria
        ///// </summary>
        ///// <param name="card"></param>
        //public void AddCard(CardInfo card)
        //{
        //    if (Cards.Count < GameManager.MAXDECK)
        //        Cards.Insert(rng.Next(0, Cards.Count), card);
        //}

        ///// <summary>
        ///// Devuelve si podemos añadir una carta
        ///// </summary>
        ///// <returns></returns>
        //public bool CanAddCard()
        //{
        //    return (Cards.Count < GameManager.MAXDECK);
        //}
    }
}