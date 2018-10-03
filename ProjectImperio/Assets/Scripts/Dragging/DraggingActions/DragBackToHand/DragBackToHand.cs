using UnityEngine;
using DG.Tweening;

namespace Imperio
{
    /// <summary>
    /// Componente abstracto, que crea un hueco vacío en la mano mientras se arrastra la carta.
    /// Devuelve la carta a la mano con una secuencia de Dotween en caso de error al soltar la carta.
    /// </summary>
    public abstract class DragBackToHand : DraggingAction
    {
        //---------------------INSPECTOR----------------------

        public float TimeToReturnToHand;

        //---------------------INSPECTOR----------------------

        //---------------------PROTECTED VARIABLES------------------------------

        /// <summary>
        /// GameObject para mostrar un hueco vacío entre las cartas de la mano
        /// </summary>
        protected GameObject handPlaceholder = null;

        //---------------------PROTECTED VARIABLES------------------------------

        /// <summary>
        /// Referencia a la carta
        /// </summary>
        private Card card;

        /// <summary>
        /// Guarda referencia a la carta
        /// </summary>
        protected virtual void Awake()
        {
            card = GetComponent<Card>();
        }

        /// <summary>
        /// Es llamado al coger la carta
        /// Crea el placeholder de la mano
        /// </summary>
        public override void OnStartDrag()
        {
            //Creamos el hueco vacío
            CreateHandPlaceholder();

            //Eliminamos su parentesco a la mano
            transform.SetParent(transform.parent.parent);

            //Bloqueamos raycast de la carta para poder dropearla en una zona
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        /// <summary>
        /// Crea un GameObject en la zona correspondiente para mostrar un hueco vacío
        /// </summary>
        private void CreateHandPlaceholder()
        {
            handPlaceholder = new GameObject();

            //Lo ponemos en la posición correspondiente de la zona
            handPlaceholder.transform.SetParent(transform.parent);
            handPlaceholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

            //Queremos que ocupe lo mismo que la carta en el layout
            handPlaceholder.AddComponent<RectTransform>();
        }

        /// <summary>
        /// Es llamado por la clase hijo cuando la carta es soltada en una zona no correspondiente
        /// Devuelve la carta a la mano
        /// Si se suelta en battlefield: Juega la carta
        /// Si se suelta fuera de battlefield: Devuelve la carta a la mano
        /// </summary>
        public override void OnEndDrag()
        {
            transform.DOMove(handPlaceholder.transform.position, TimeToReturnToHand).SetEase(Ease.OutQuint).onComplete += this.CardBackToHand;
        }

        /// <summary>
        /// Es llamado cuando acaba la secuencia de DOTween que devuelve la carta a la posición de la mano inicial
        /// Destruye el Placeholder y mete la carta en la mano
        /// </summary>
        private void CardBackToHand()
        {
            //La metemos en el hueco correspondiente
            transform.SetParent(card.Owner.Hand.transform);
            transform.SetSiblingIndex(handPlaceholder.transform.GetSiblingIndex());

            //Destruimos el hueco vacío
            Destroy(handPlaceholder);
            handPlaceholder = null;

            //Puede volver a ser cogida
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}