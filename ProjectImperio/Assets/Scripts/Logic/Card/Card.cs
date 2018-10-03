using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Imperio
{
    /// <summary>
    /// Clase base del Objeto Carta
    /// Tiene los atributos y métodos comunes de todas las cartas
    /// </summary>
    public abstract class Card : MonoBehaviour
    {
        //------------Atributos inspector-------------

        [Header("Text Component References")]
        [SerializeField] private Text nameText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Text goldText;

        [Header("Image References")]
        [SerializeField] private Image cardGraphicImage;      //Artwork de la carta
        [SerializeField] private Image cardFaceGlowImage;     //Aura de carta seleccionada

        [Header("Rotation References")]
        [SerializeField] private RectTransform cardFront;     //GameObject que representa el Front de la carta
        [SerializeField] private RectTransform cardBack;      //GameObject que representa el Back de la carta

        //------------Atributos inspector-------------

        //------------Propiedades---------------------

        /// <summary>
        /// Referencia al contenido de la carta
        /// </summary>
        public CardInfo CardInfo { get; set; }

        /// <summary>
        /// Jugador al que pertenece la carta
        /// </summary>
        public Player Owner { get; set; }

        /// <summary>
        /// El coste de una carta puede cambiar, queremos guardar el coste actual
        /// </summary>
        public int CurrentGoldCost { get; set; }

        //------------Propiedades---------------------

        //------------Atributos privados---------------------

        /// <summary>
        /// Atributo que representa si la carta está boca abajo o boca arriba
        /// </summary>
        private bool showingBack = false;

        //------------Atributos privados---------------------

        /// <summary>
        /// Construye la carta, establece todos los atributos de cardInfo
        /// </summary>
        /// <param name="cardInfo"></param>
        public void BuildCard(CardInfo cardInfo)
        {
            CardInfo = cardInfo;

            ReadCardFromInfo();
        }

        /// <summary>
        /// Lee el CardInfo y establece su relación con el Canvas
        /// </summary>
        protected virtual void ReadCardFromInfo()
        {
            CurrentGoldCost = CardInfo.GoldCost;

            //Damos valor a las referencias
            nameText.text = CardInfo.Name;
            descriptionText.text = CardInfo.Description;
            cardGraphicImage.sprite = CardInfo.Image;
            goldText.text = CardInfo.GoldCost.ToString();
        }

        /// <summary>
        /// Rota la carta y muestra la parte trasera o delantera
        /// </summary>
        public void Rotate()
        {
            //Mostramos el top
            if (showingBack)
            {
                this.transform.DORotate(new Vector3(0, 270, 0), 0.5f).OnComplete(() =>
                {
                    cardFront.gameObject.SetActive(true);
                    cardBack.gameObject.SetActive(false);

                    this.transform.DORotate(new Vector3(0, 0, 0), 0.5f);
                });
            }
            else
            {
                this.transform.DORotate(new Vector3(0, 90, 0), 0.5f).OnComplete(() =>
                 {
                     cardFront.gameObject.SetActive(false);
                     cardBack.gameObject.SetActive(true);

                     this.transform.DORotate(new Vector3(0, 180, 0), 0.5f);
                 });
            }
            showingBack = !showingBack;
        }

        /// <summary>
        /// Devuelve si una carta puede ser jugada o no.
        /// Puede ser jugada si es el turno del jugador y si hay oro
        /// </summary>
        /// <returns></returns>
        public virtual bool CanBePlayed()
        {
            bool ownersTurn = GameManager.Instance.WhoseTurn == Owner;
            bool haveGold = Owner.Gold.CanSpendGold(CurrentGoldCost);

            return ownersTurn && haveGold;
        }

        /// <summary>
        /// Establece el brillo de la carta
        /// </summary>
        /// <param name="value"></param>
        public void SetGlowImage(bool value)
        {
            cardFaceGlowImage.enabled = value;
        }
    }
}