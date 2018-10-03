using UnityEngine;
using DG.Tweening;

namespace Imperio
{
    /// <summary>
    /// Muestra la carta más grande cuando ponemos el cursor encima
    /// </summary>
    public class HoverPreview : MonoBehaviour
    {
        //-------------------Atributos públicos---------------------

        //Referencia al GameObject que queremos ocultar cuando mostremos el Preview (CardPanel)
        public GameObject TurnThisOffWhenPreviewing;  // if this is null, will not turn off anything 

        //Posición del Preview relativo a la carta
        public Vector3 TargetPosition;

        //Escala del Preview
        public float TargetScale;

        //Referencia al Preview GameObject (CardPreview)
        public GameObject previewGameObject;

        //Si es true, siempre que pasemos el cursor por encima, se mostrará el preview
        //Solo para debuggear, ya que requiere condiciones
        public bool ActivateInAwake = false;

        //-------------------Atributos privados Static---------------------

        //Guarda la instancia del preview que estamos actualmente viendo 
        private static HoverPreview currentlyViewing = null;

        //Propiedad
        //Variable que determina si podemos previsualizar alguna carta (Al robar no podemos)
        private static bool _PreviewsAllowed = true;
        public static bool PreviewsAllowed
        {
            get { return _PreviewsAllowed; }

            set
            {
                //Debug.Log("Hover Previews Allowed is now: " + value);
                _PreviewsAllowed = value;

                //Si establecemos que el Preview no está permitido, tenemos que dejar de permitir las Previews
                if (!_PreviewsAllowed)
                    StopAllPreviews();
            }
        }

        //-------------------Atributos privados---------------------

        //Variable que determina si el GameObject con este script está siendo Previsualizado
        private bool _thisPreviewEnabled = false;
        public bool ThisPreviewEnabled
        {
            get { return _thisPreviewEnabled; }

            set
            {
                _thisPreviewEnabled = value;

                //Si no esta permitido este preview, lo paramos
                if (!_thisPreviewEnabled)
                    StopThisPreview();
            }
        }

        //Variable pública que no es visible en el inspector
        //Es true cuando tiene el cursor encima
        public bool OverCollider { get; set; }

        void Awake()
        {
            //Si en el editor hemos establecido que podemos Previsualizar la carta desde el principio
            ThisPreviewEnabled = ActivateInAwake;
        }

        void OnMouseEnter()
        {
            //Hemos entrado en la carta
            OverCollider = true;

            //Si Preview está permitido y el de este GameObject también
            if (PreviewsAllowed && ThisPreviewEnabled)
                PreviewThisObject();//Visualizamos el Preview
        }

        void OnMouseExit()
        {
            //Hemos salido de la carta
            OverCollider = false;

            //Si no estamos ya viendo ninguna carta, paramos todas las previews
            if (!PreviewingSomeCard())
                StopAllPreviews();
        }

        void PreviewThisObject()
        {
            // 1) Desactivamos todos los previews en caso de que haya
            StopAllPreviews();

            // 2) Establecemos la instacia Static del HoverPreview a la actual
            currentlyViewing = this;

            // 3) Activamos Preview GameObject
            previewGameObject.SetActive(true);

            // 4) Desactivamos lo que queramos desactivar (Carta). Minion no se desactiva nada
            if (TurnThisOffWhenPreviewing != null)
                TurnThisOffWhenPreviewing.SetActive(false);

            // 5) Animamos con DOTween a la posición elegida

            //Establecemos posición y escala inicial (locales)
            previewGameObject.transform.localPosition = Vector3.zero;
            previewGameObject.transform.localScale = Vector3.one;

            //Animamos localmente
            previewGameObject.transform.DOLocalMove(TargetPosition, 1f).SetEase(Ease.OutQuint);
            previewGameObject.transform.DOScale(TargetScale, 1f).SetEase(Ease.OutQuint);
        }

        //Método que desactiva el preview de este GameObject y reinicia valores. Activa lo que hayamos desactivado (carta)
        void StopThisPreview()
        {
            //Desactiva la preview
            previewGameObject.SetActive(false);

            //Reinicia valores
            previewGameObject.transform.localScale = Vector3.one;
            previewGameObject.transform.localPosition = Vector3.zero;

            //Activa lo que hayamos desactivado (carta)
            if (TurnThisOffWhenPreviewing != null)
                TurnThisOffWhenPreviewing.SetActive(true);
        }

        //Desactiva todas las previews activas
        private static void StopAllPreviews()
        {
            //Si este objeto es el que está visualizando su preview, lo paramos
            if (currentlyViewing != null)
                currentlyViewing.StopThisPreview();
        }

        //Devuelve si estamos previsualizando alguna carta
        private static bool PreviewingSomeCard()
        {
            //Si no esta permitido ver ninguna carta
            if (!PreviewsAllowed)
                return false;

            //Buscamos en todas las cartas si alguna esta siendo previsualizada
            HoverPreview[] allHoverBlowups = GameObject.FindObjectsOfType<HoverPreview>();

            foreach (HoverPreview hb in allHoverBlowups)
            {
                //Si estamos encima de la carta y la carta está siendo previsualizada
                if (hb.OverCollider && hb.ThisPreviewEnabled)
                    return true;
            }

            return false;
        }
    }
}