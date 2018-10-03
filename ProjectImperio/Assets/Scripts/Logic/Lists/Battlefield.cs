using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

namespace Imperio
{
    /// <summary>
    /// Lista de Minions con diferentes métodos auxiliares
    /// </summary>
    public class Battlefield : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// Lista de minions en el campo de batalla
        /// </summary>
        private List<Minion> minions;

        /// <summary>
        /// Construye la lista de minions
        /// </summary>
        public void BuildBattlefield()
        {
            minions = new List<Minion>(GameManager.MAXBATTLEFIELD);
        }

        /// <summary>
        /// Informa a los minions de que ha empezado un nuevo turno
        /// </summary>
        public void OnTurnStart()
        {
            foreach (Minion minion in minions)
                minion.OnTurnStart();
        }

        /// <summary>
        /// Informa a los minions de que ha acabado el turno
        /// </summary>
        public void OnTurnEnd()
        {
            foreach (Minion minion in minions)
                minion.OnTurnEnd();
        }

        /// <summary>
        /// Añade una carta en la zona y posición establecida
        /// </summary>
        /// <param name="card"></param>
        /// <param name="index"></param>
        public void PlayMinion(Minion card, int index)
        {
            minions.Insert(index, card);
        }

        /// <summary>
        /// Devuelve si podemos jugar una carta en una zona
        /// </summary>
        /// <returns></returns>
        public bool CanPlayMinion()
        {
            return (minions.Count < GameManager.MAXBATTLEFIELD);
        }

        /// <summary>
        /// Remueve el minion del battlefield
        /// </summary>
        /// <returns></returns>
        public void RemoveMinion(Minion minion)
        {
            minions.Remove(minion);
        }

        /// <summary>
        /// Devuelve si hay algún minion con TAUNT en el Battlefield
        /// </summary>
        /// <returns></returns>
        public bool SomeoneWithTaunt()
        {
            for (int i = 0; i < minions.Count; i++)
            {
                if (minions[i].Taunt)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Aplica un efecto a todos los minions del Battlefield
        /// </summary>
        /// <param name="effect"></param>
        public void ApplyEffect(Effect effect)
        {
            foreach (Minion minion in minions)
                minion.ApplyEffect(effect);
        }

        /// <summary>
        /// Es llamado cuando el puntero del ratón entra en el GameObject
        /// Si llevamos una carta, establecemos el placeholder en el nuevo dropzone
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            //Detectamos si llevamos una carta
            if (eventData.pointerDrag != null)
            {
                //Detectamos si llevamos una carta minion
                DragCreatureCard creature = eventData.pointerDrag.gameObject.GetComponent<DragCreatureCard>();
                if (creature != null && Draggable.DraggingThis != null)
                {
                    //Creamos hueco vacio.
                    creature.CreateBattlefieldPlaceholder();
                }
            }
        }

        /// <summary>
        /// Es llamado cuando el puntero del ratón sale del GameObject
        /// Si salimos del dropzone arrastrando una carta, le decimos al placeholder que vuelva al dropzone original
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            //Detectamos si llevamos una carta
            if (eventData.pointerDrag != null)
            {
                DragCreatureCard creature = eventData.pointerDrag.gameObject.GetComponent<DragCreatureCard>();
                if (creature != null)
                    creature.DeleteBattlefieldPlaceholder();
            }
        }

        ///// <summary>
        ///// Devuelve y quita una carta de la lista del battlefield
        ///// </summary>
        ///// <param name="index"></param>
        ///// <returns></returns>
        //public Minion GetCard(int index)
        //{
        //    Minion card = Minions.ElementAt(index);
        //    Minions.RemoveAt(index);
        //    return card;
        //}

        ///// <summary>
        ///// Devuelve si hay algún minion en una zona
        ///// </summary>
        ///// <returns></returns>
        //public bool CanGetCard()
        //{
        //    return (Minions.Count > 0);
        //}



    }
}