using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
namespace Imperio
{
    /// <summary>
    /// 
    /// </summary>
    public class Minion : MonoBehaviour, IDropHandler
    {
        //------------Atributos inspector-------------

        [Header("Text Component References")]
        [SerializeField] private Text attackText;
        public Text HealthText;

        [Header("Image References")]
        [SerializeField] private Image creatureGraphicImage; //Artwork de la carta 
        [SerializeField] private Image creatureGlowImage; //Aura de carta activa

        //Imágenes de las mecánicas
        [SerializeField] private GameObject tauntImage;
        [SerializeField] private GameObject stealthImage;
        [SerializeField] private GameObject chargeImage;
        [SerializeField] private GameObject windfuryImage;

        //------------Atributos inspector-------------

        //------------PROPIEDADES-------------

        /// <summary>
        /// Jugador al que pertenece el minion
        /// </summary>
        public Player Owner { get; set; }

        /// <summary>
        /// Vida actual del minion. 
        /// Si es <= 0 muere
        /// Controla que no se pase de vida máxima
        /// </summary>
        public int CurrentHealth
        {
            get { return currentHealth; }

            set
            {
                if (value > maxHealth)
                    currentHealth = maxHealth;
                else if (value <= 0)
                {
                    currentHealth = 0;
                    attacksLeftThisTurn = 0;
                }
                else
                    currentHealth = value;

                HealthText.text = currentHealth.ToString();

                if (currentHealth <= 0)
                    MinionDie();
            }
        }
        private int currentHealth;

        /// <summary>
        /// Ataque actual
        /// </summary>
        public int Attack
        {
            get
            {
                return attack;
            }
            set
            {
                attack = value;
                attackText.text = attack.ToString();
            }
        }
        private int attack;

        /// <summary>
        /// Ataques que le quedan hacer al minion este turno
        /// </summary>
        private int m_attacksLeftThisTurn;
        private int attacksLeftThisTurn
        {
            get
            {
                return m_attacksLeftThisTurn;
            }
            set
            {
                m_attacksLeftThisTurn = value;
                creatureGlowImage.enabled = value > 0;

            }
        }

        //Mecánicas
        public bool Taunt { get; set; }
        public bool Charge { get; set; }
        public bool Stealth { get; set; }
        public bool Windfury { get; set; }

        //------------PROPIEDADES-------------

        //------------Atributos privados-------------

        /// <summary>
        /// Referencia al contenido del minion
        /// </summary>
        private CardInfo minionInfo;

        /// <summary>
        /// Vida máxima actual
        /// </summary>
        private int maxHealth;

        private int attacksForOneTurn;
        private int attacksThisTurn;

        //TODO: FROZEN, DICCIONARIO DE CARTAS JUGADAS

        //------------Atributos privados-------------

        /// <summary>
        ///  Construye el minion, establece todos los atributos de minionInfo
        /// </summary>
        /// <param name="minionInfo"></param>
        public void BuildMinion(MinionCard minionCard) 
        {
            minionInfo = minionCard.CardInfo;
            maxHealth = minionCard.MaxHealth;

            Attack = minionCard.Attack;
            CurrentHealth = minionCard.MaxHealth;

            attacksForOneTurn = 1;
            attacksLeftThisTurn = 0;
            attacksThisTurn = 0;

            creatureGraphicImage.sprite = minionInfo.Image;

            Taunt = false;
            Charge = false;
            Stealth = false;
            Windfury = false;

            if (minionCard.Charge)
                ApplyCharge();

            if (minionCard.Windfury)
                ApplyWindfury();

            if (minionCard.Taunt)
                ApplyTaunt();

            if (minionCard.Stealth)
                ApplyStealth();

        }

        /// <summary>
        /// Es llamado cuando se inicia el turno de un jugador
        /// </summary>
        public void OnTurnStart()
        {
            if (Windfury)
                attacksLeftThisTurn = 2;
            else
                attacksLeftThisTurn = 1;

        }

        /// <summary>
        /// Es llamado cuando se acaba el turno de un jugador
        /// </summary>
        public void OnTurnEnd()
        {
            attacksLeftThisTurn = 0;
        }

        /// <summary>
        /// Ataca a un minion enemigo, recibiendo ambos daño
        /// </summary>
        /// <param name="target"></param>
        public void AttackMinion(Minion target)
        {
            OnAttack();

            new MinionAttackCommand(this, target).EnqueueComand();
        }

        /// <summary>
        /// Ataca un fuerte enemigo, recibiendo el fuerte daño
        /// </summary>
        /// <param name="target"></param>
        public void AttackFort(Fort target)
        {
            OnAttack();

            new MinionAttackFaceCommand(this, target).EnqueueComand();
        }

        /// <summary>
        /// Actualiza el número de ataques y quita sigilo
        /// </summary>
        private void OnAttack()
        {
            attacksLeftThisTurn--;
            attacksThisTurn++;

            if (Stealth)
            {
                Stealth = false;
                stealthImage.SetActive(false);
            }
        }

        /// <summary>
        /// Devuelve si el minion puede atacar actualmente
        /// </summary>
        /// <returns></returns>
        public bool CanAttack()
        {
            return (GameManager.Instance.WhoseTurn == Owner && m_attacksLeftThisTurn > 0);
        }

        /// <summary>
        /// Aplica un efecto de un hechizo al minion
        /// </summary>
        /// <param name="effect"></param>
        public void ApplyEffect(Effect effect)
        {
            Attack += effect.Attack;
            maxHealth += effect.MaxHealth;

            if (effect.Health != 0)
            {
                CurrentHealth += effect.Health;
                DamageEffect.CreateDamageEffect(transform.position, effect.Health);
            }

            if (effect.Taunt)
                ApplyTaunt();

            if (effect.Charge)
                ApplyCharge();

            if (effect.Stealth)
                ApplyStealth();

            if (effect.Windfury)
                ApplyWindfury();

        }

        private void ApplyTaunt()
        {
            if (!Taunt)
            {
                tauntImage.SetActive(true);
                Taunt = true;
            }
        }

        private void ApplyCharge()
        {
            if (!Charge)
            {
                chargeImage.SetActive(true);
                Charge = true;

                attacksLeftThisTurn = attacksForOneTurn - attacksThisTurn;
            }
        }

        private void ApplyWindfury()
        {
            if (!Windfury)
            {
                windfuryImage.SetActive(true);
                Windfury = true;

                attacksForOneTurn = 2;

                if (Charge)
                    attacksLeftThisTurn = attacksForOneTurn - attacksThisTurn;
            }
        }

        private void ApplyStealth()
        {
            if (!Stealth)
            {
                stealthImage.SetActive(true);
                Stealth = true;
            }
        }

        /// <summary>
        /// Método heredado de DropHandler. Es llamado cuando se suelta un objeto en Minion
        /// Detecta si es un minion o un hechizo con target y si lo es, establece su target a este
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

        /// <summary>
        /// Es llamado cuando la vida del personaje es <= 0.
        /// Destruye el minion despues de una animación
        /// </summary>
        private void MinionDie()
        {
            transform.DOShakeScale(1.0f).SetEase(Ease.OutQuint).onComplete += OnEndAnimDie;
        }

        private void OnEndAnimDie()
        {
            Owner.PlayerBattlefield.RemoveMinion(this);
            Destroy(gameObject);
        }
    }

}