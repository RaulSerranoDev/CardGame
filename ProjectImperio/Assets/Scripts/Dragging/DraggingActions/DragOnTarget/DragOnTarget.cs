
using UnityEngine;
using UnityEngine.UI;

namespace Imperio
{
    /// <summary>
    /// Componente que se añade a la cartas que tienen un target único: Minions y TargetSpell
    /// Dibuja la diana
    /// </summary>
    public abstract class DragOnTarget : DraggingAction
    {
        /// <summary>
        /// Color de la diana
        /// </summary>
        public Color CrossColor;

        /// <summary>
        /// Cuando acabemos de arrastrar, este es el Target que atacará
        /// </summary>
        public GameObject Target { get; set; }

        /// <summary>
        /// Referencia a la diana
        /// </summary>
        protected Image sr;

        /// <summary>
        /// Obtiene referencias y pone el color inicial
        /// </summary>
        protected virtual void Awake()
        {
            sr = GetComponent<Image>();
            sr.color = new Color(CrossColor.r, CrossColor.g, CrossColor.b, 0.0f);
        }

        /// <summary>
        /// Renderiza la diana
        /// Reinicia valores de Target
        /// </summary>
        public override void OnStartDrag()
        {
            sr.color = new Color(CrossColor.r, CrossColor.g, CrossColor.b, 1.0f);
            Target = null;

            transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        /// <summary>
        /// No hace nada en este Script
        /// </summary>
        public override void OnDragging()
        {

        }

        /// <summary>
        /// Oculta la diana y ataca el objetivo en caso de que tenga.
        /// Distinguiendo entre Minion y Fuerte
        /// </summary>
        public override void OnEndDrag()
        {
            //Devolvemos la diana a la posición original
            transform.localPosition = Vector3.zero;
            sr.color = new Color(CrossColor.r, CrossColor.g, CrossColor.b, 0.0f);
            transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

    }
}
