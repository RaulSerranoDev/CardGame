using UnityEngine;
using UnityEngine.EventSystems;

namespace Imperio
{
    /// <summary>
    /// Componente que se añade a las cartas para que puedan ser cogidas y arrastrasdas
    /// Hace diferentes acciones en función del DraggingAction añadido
    /// </summary>
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        //---------------Atributos inspector------------------

        /// <summary>
        /// Si es true, el puntero estará en la misma posición de inicio siempre
        /// Si es false, la carta saltará y se pondra en el centro del puntero
        /// </summary>
        public bool UsePointerDisplacement = true;

        //---------------Atributos inspector------------------

        //---------------PROPIEDADES------------------

        /// <summary>
        /// Propiedades STATIC que devuelve la instacia del Draggable que estamos arrastrando actualmente
        /// TODO: Puede que no se use para nada
        /// </summary>
        private static Draggable draggingThis;
        public static Draggable DraggingThis
        {
            get { return draggingThis; }
        }

        //---------------PROPIEDADES------------------

        //---------------Atributos privados------------------

        /// <summary>
        /// Referencia a un Script de DraggingActions. Este script debe ser añadido a este mismo GameObject
        /// </summary>
        private DraggingAction draggingAction;

        /// <summary>
        /// Distancia desde el centro de este GameObject hasta el puntero del raton al hacer click
        /// </summary>
        private Vector3 pointerDisplacement = Vector3.zero;

        //---------------Atributos privados------------------

        private void Awake()
        {
            draggingAction = GetComponent<DraggingAction>();
        }

        private void Start()
        {
            pointerDisplacement = Vector3.zero;
        }

        /// <summary>
        /// Empieza a arrastrar la carta en caso de que el DraggingAction lo permita. 
        /// Ajusta el centro de la carta al puntero del ratón
        /// Informa al DraggingAction correspondiente
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (draggingAction != null && draggingAction.CanDrag())
            {
                draggingThis = this;

                //TODO: Previews no permitidas

                //Calculamos la diferencia de posición entre click y centro de la carta
                if (UsePointerDisplacement)
                    pointerDisplacement = -transform.position + new Vector3(eventData.position.x, eventData.position.y);
                else
                    pointerDisplacement = Vector3.zero;

                draggingAction.OnStartDrag();
            }

        }

        /// <summary>
        /// Método que es llamado continuamente cuando arrastramos una carta con el ratón
        /// Desplaza la carta a la posición del ratón e informa a DraggingAction correspondiente
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (draggingThis != null)
            {
                //La posición de la carta es la posición del cursor
                this.transform.position = new Vector3(eventData.position.x - pointerDisplacement.x, eventData.position.y - pointerDisplacement.y, transform.position.z);

                draggingAction.OnDragging();
            }
        }

        /// <summary>
        /// Método que es llamado una vez cuando soltamos el clickIzquierdo del raton (Después de OnDrop())
        /// Informa al DraggingAction correspondiente
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (draggingThis != null)
            {
                //TODO: permitir Preview
                draggingThis = null;

                draggingAction.OnEndDrag();
            }
        }
    }
}