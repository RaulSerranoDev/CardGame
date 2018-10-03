using UnityEngine;

namespace Imperio
{
    /// <summary>
    /// Componente que se añade a las cartas que son minions
    /// Permite arrastrarlas y jugar los minions, comprobando si es posible
    /// </summary>
    public class DragCreatureCard : DragBackToHand
    {
        //---------------------PRIVATE VARIABLES------------------------------

        /// <summary>
        /// Referencia a la carta
        /// </summary>
        private MinionCard minionCard;

        /// <summary>
        /// Referencia al battlefield del jugador
        /// </summary>
        private Battlefield ownerBattlefield;

        /// <summary>
        /// GameObject para mostrar un hueco vacío entre los minions del Battlefield
        /// </summary>
        private GameObject battlefieldPlaceholder = null;

        //---------------------PRIVATE VARIABLES------------------------------

        /// <summary>
        /// Obtenemos referencias
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            minionCard = GetComponent<MinionCard>();
        }

        /// <summary>
        /// Guarda referencia al battlefield
        /// </summary>
        private void Start()
        {
            ownerBattlefield = minionCard.Owner.PlayerBattlefield;
        }

        /// <summary>
        /// Actualiza la posición del hueco vacío
        /// </summary>
        public override void OnDragging()
        {
            //Movemos el hueco vacío a donde le toque
            //Si el puntero está sobre el battlefield, se ha creado un placeholder
            if (battlefieldPlaceholder != null)
            {
                //Movemos el hueco vacío a donde le corresponda del layout
                int newSiblingIndex = ownerBattlefield.transform.childCount;

                for (int i = 0; i < ownerBattlefield.transform.childCount; i++)
                {
                    if (transform.position.x < ownerBattlefield.transform.GetChild(i).position.x)
                    {
                        newSiblingIndex = i;
                        if (battlefieldPlaceholder.transform.GetSiblingIndex() < newSiblingIndex)
                            newSiblingIndex--;
                        break;
                    }
                }
                battlefieldPlaceholder.transform.SetSiblingIndex(newSiblingIndex);
            }
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
                //La metemos en el hueco correspondiente del battlefield
                transform.SetParent(ownerBattlefield.transform);
                transform.SetSiblingIndex(battlefieldPlaceholder.transform.GetSiblingIndex());

                //Destruimos los placeholders
                Destroy(battlefieldPlaceholder);
                battlefieldPlaceholder = null;
                Destroy(handPlaceholder);
                handPlaceholder = null;

                //Juega el minion en la posición de la mesa pasada
                minionCard.Owner.PlayAMinionFromHand(minionCard, transform.GetSiblingIndex());
            }
            //Si la carta se ha soltado en otra zona cualquiera
            else
                base.OnEndDrag();

        }

        /// <summary>
        /// Devuelve true si hay espacio en el Battlefield y si hemos soltados en una zona de battlefield
        /// </summary>
        /// <returns></returns>
        protected override bool DragSuccessful()
        {
            bool tableNotFull = GameManager.Instance.WhoseTurn.PlayerBattlefield.CanPlayMinion();
            //Comprobamos si el placeholder es distinto de null, porque si lo es, la carta está encima del battlefield
            return (tableNotFull && battlefieldPlaceholder != null);
        }

        /// <summary>
        /// Devuelve si podemos coger esta carta actualmente
        /// </summary>
        /// <returns></returns>
        public override bool CanDrag()
        {
            return (minionCard.CanBePlayed());
        }

        /// <summary>
        /// Es llamado cuando el puntero del ratón entra en el battlefield correspondiente desde la clase Battlefield
        /// Crea un placeholder vacío
        /// </summary>
        public void CreateBattlefieldPlaceholder()
        {
            battlefieldPlaceholder = new GameObject();

            //Lo ponemos en la posición correspondiente de la zona
            battlefieldPlaceholder.transform.SetParent(ownerBattlefield.transform);

            //Queremos que ocupe lo mismo que la carta en el layout
            battlefieldPlaceholder.AddComponent<RectTransform>();
        }

        /// <summary>
        /// Es llamado cuando el puntero del ratón sale del battlefield
        /// Elimina el placeholder vacío
        /// </summary>
        public void DeleteBattlefieldPlaceholder()
        {
            Destroy(battlefieldPlaceholder);
            battlefieldPlaceholder = null;
        }
    }
}