using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Imperio
{
    /// <summary>
    /// Atributo de player
    /// </summary> 
    public class Fort : MonoBehaviour, IDropHandler
    {
        //---------------Inspector--------------

        public Text HealthText;

        //---------------Properties--------------

        /// <summary>
        /// Vida actual del fuerte
        /// Actualiza el texto cada vez que es modificado
        /// </summary>
        public int Health
        {
            get { return health; }
            set
            {
                if (value <= 0)    
                    health = 0;
                
                else if (value > maxHealth)
                    health = maxHealth;
                else
                    health = value;

                HealthText.text = health.ToString();

                //Informar al jugador si ha perdido un fuerte
                if (health <= 0)
                    Player.LostFort(this);
            }
        }

        public Player Player { get; set; }

        //---------------Private attributes--------------

        private int health;
        private int maxHealth;

        /// <summary>
        /// Constructora del fuerte
        /// Inicializa la vida
        /// </summary>
        /// <param name="player"></param>
        public void BuildFort(Player player)
        {
            this.Player = player;
            maxHealth = GameManager.LIFEOFTHEFORT;
            health = maxHealth;
        }

        /// <summary>
        /// El fuerte es receptor de hechizos de efecto. Le pueden modificar la salud
        /// </summary>
        /// <param name="effect"></param>
        public void ApplyEffect(Effect effect)
        {
            maxHealth += effect.MaxHealth;

            if (effect.Health != 0)
            {
                Health += effect.Health;
                DamageEffect.CreateDamageEffect(transform.position, effect.Health);

            }

        }

        /// <summary>
        /// Método heredado de DropHandler. Es llamado cuando se suelta un objeto en Fort
        /// Detecta si es un minion o un hechizo y si lo es, establece su target a este
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrop(PointerEventData eventData)
        {
            //Detectamos si llevamos una carta
            if (eventData.pointerDrag != null)
            {
                DragCreatureAttack creature = eventData.pointerDrag.gameObject.GetComponent<DragCreatureAttack>();
                if (creature != null)
                    creature.Target = this.gameObject;
                else
                {
                    DragTargetSpellCard spell = eventData.pointerDrag.gameObject.GetComponent<DragTargetSpellCard>();
                    if (spell != null)
                        spell.Target = this.gameObject;
                }
            }
        }
    }
}