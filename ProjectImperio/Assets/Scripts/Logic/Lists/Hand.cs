using UnityEngine;
using System.Collections.Generic;

namespace Imperio
{
    /// <summary>
    /// Lista de cartas de la mano y métodos auxiliares
    /// </summary>
    public class Hand : MonoBehaviour
    {
        /// <summary>
        /// Lista de cartas en la mano
        /// </summary>
        private List<Card> cards;

        public void BuildHand()
        {
            cards = new List<Card>(GameManager.MAXHAND);
        }

        /// <summary>
        /// Añade una carta en la ultima posición
        /// </summary>
        /// <param name="card"></param>
        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        /// <summary>
        /// Devuelve y quita una carta de la lista de la mano
        /// </summary>
        /// <param name="card"></param>
        public void RemoveCard(Card card)
        {
            cards.Remove(card);
        }

        /// <summary>
        /// Devuelve si podemos añadir una carta a la mano
        /// </summary>
        /// <returns></returns>
        public bool CanTakeCard()
        {
            return (cards.Count < GameManager.MAXHAND);
        }

        public void HighlightPlayableCards()
        {
            foreach (Card card in cards)
                card.SetGlowImage(card.CanBePlayed());
        }
    }
}