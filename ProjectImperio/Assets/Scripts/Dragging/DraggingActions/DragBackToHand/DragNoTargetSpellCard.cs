using UnityEngine;

namespace Imperio
{
    public class DragNoTargetSpellCard : DragBackToHand
    {
        //---------------------PRIVATE VARIABLES------------------------------

        //TODO: A LO MEJOR NO ES UN SPELLCARD, SI NO UN NOTARGETSPELLCARD
        /// <summary>
        /// Referencia a la carta
        /// </summary>
        private SpellCard spellCard;

        //---------------------PRIVATE VARIABLES------------------------------

        /// <summary>
        /// Obtenemos referencias
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            spellCard = GetComponent<SpellCard>();
        }

        /// <summary>
        /// No hace nada en esta clase
        /// </summary>
        public override void OnDragging()
        {
        }

        /// <summary>
        /// Es llamado cuando la carta es soltada
        /// Si se suelta en battlefield: Juega la carta
        /// Si se suelta fuera de battlefield: Devuelve la carta a la mano
        /// </summary>
        public override void OnEndDrag()
        {
            //Si la carta se ha colocado en el battlefield
            if (DragSuccessful())
            {
                Destroy(handPlaceholder);
                handPlaceholder = null;

                //TODO: HECHIZOS
                //Juega el minion en la posición de la mesa pasada
                spellCard.Owner.PlayANoTargetSpellFromHand(spellCard);
            }
            //Si la carta se ha soltado en otra zona cualquiera
            else
                base.OnEndDrag();

        }

        /// <summary>
        ///TODO: COMPROBACIONES DE TARGET
        /// </summary>
        /// <returns></returns>
        protected override bool DragSuccessful()
        {
            //TODO: COMPROBACIONES DE QUE EL CURSOR ESTÁ DONDE QUIERO
            return true;
        }

        /// <summary>
        /// Devuelve si podemos coger esta carta actualmente
        /// </summary>
        /// <returns></returns>
        public override bool CanDrag()
        {
            return (spellCard.CanBePlayed());
        }
    }
}
