using UnityEngine;

namespace Imperio
{
    /// <summary>
    /// Clase abstracta
    /// Acciones que realizan las cartas cuando son arrastradas
    /// </summary>
    public abstract class DraggingAction : MonoBehaviour
    {
        /// <summary>
        /// Se le llama cuando cogemos la carta
        /// </summary>
        public abstract void OnStartDrag();

        /// <summary>
        /// Se le llama cuando soltamos la carta
        /// </summary>
        public abstract void OnEndDrag();

        /// <summary>
        /// Se le llama continuamente cuando estamos arrastrando la carta
        /// </summary>
        public abstract void OnDragging();

        /// <summary>
        /// Devuelve si podemos coger esta carta actualmente
        /// </summary>
        /// <returns></returns>
        public virtual bool CanDrag()
        {
            return true;
        }

        /// <summary>
        /// Devuelve si el drag no fue correcto y no podemos hacer una accion
        /// </summary>
        /// <returns></returns>
        protected abstract bool DragSuccessful();
    }
}