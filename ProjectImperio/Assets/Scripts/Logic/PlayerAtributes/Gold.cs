using UnityEngine;
using UnityEngine.UI;

namespace Imperio
{
    /// <summary>
    /// Atributo de Player.
    /// Controla la gestión del oro.
    /// </summary>
    public class Gold : MonoBehaviour
    {
        //---------------INSPECTOR-------------

        /// <summary>
        /// Referencia al texto que muestra la cantidad de oro
        /// </summary>
        public Text GoldText;

        //---------------INSPECTOR-------------

        //---------------PROPERTIES-------------

        /// <summary>
        /// Oro actual
        /// Al modificarlo, se lanza un comando de actualizar el Oro
        /// </summary>
        public int CurrentGold
        {
            get { return currentGold; }
            set
            {
                currentGold = value;

                //Informamos al comando de que actualize el texto
                new UpdateGoldCommand(this).EnqueueComand();

                if (GameManager.Instance.WhoseTurn == player)
                    player.Hand.HighlightPlayableCards();
            }
        }

        //---------------PROPERTIES-------------

        //---------------Private attributes------------------

        /// <summary>
        /// Oro actual
        /// </summary>
        private int currentGold;

        /// <summary>
        /// Oro que añadimos en el siguiente turno
        /// </summary>
        private int goldNextTurn;

        /// <summary>
        /// Referencia al player
        /// </summary>
        private Player player;

        //---------------Private attributes------------------

        /// <summary>
        /// Constructora de la clase Oro
        /// Inicializa el oro
        /// </summary>
        /// <param name="player"></param>
        public void BuildGold(Player player)
        {
            this.player = player;
            currentGold = 0;
            goldNextTurn = GameManager.MINCOLLECTIBLEGOLDPERTURN;
        }

        /// <summary>
        /// Actualiza el oro actual, es llamado cada turno del jugador
        /// </summary>
        public void OnTurnStart()
        {
            if (currentGold + goldNextTurn <= GameManager.MAXGOLD)
                CurrentGold += goldNextTurn;
            else
                CurrentGold = GameManager.MAXGOLD;

            //Comprobamos si hemos llegado al final del ciclo de oro
            if (goldNextTurn == GameManager.MAXCOLLECTIBLEGOLDPERTURN)
                goldNextTurn = GameManager.MINCOLLECTIBLEGOLDPERTURN;          
            else         
                goldNextTurn++;   
        }

        /// <summary>
        /// Añade una cantidad de oro
        /// </summary>
        /// <param name="quantity"></param>
        public void AddGold(int quantity)
        {
            CurrentGold += quantity;
        }

        /// <summary>
        /// Devuelve si podemos utilizar una cantidad de oro
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool CanSpendGold(int quantity)
        {
            return (quantity <= currentGold);
        }
    }
}
//TODO: Sobrecarga, añadir mana y eso