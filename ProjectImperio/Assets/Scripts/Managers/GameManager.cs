using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Imperio
{
    /// <summary>
    /// Controla el flujo del juego
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        //Esta clase es SINGLETON
        public static GameManager Instance = null;

        //-----------------ATRIBUTOS INSPECTOR-----------------

        //Prefabs de las cartas
        public GameObject MinionCardPrefab;
        public GameObject TargetSpellCardPrefab;
        public GameObject NoTargetSpellCardPrefab;
        public GameObject MinionPrefab;
        public GameObject FatigueCard;
        public GameObject DamageEffectPrefab;

        //Referencias
        public Player[] Players;
        public GameObject GlobalCanvas;

        [SerializeField] private Text timerText;
        [SerializeField] private Button endTurnButton;

        //-----------------ATRIBUTOS INSPECTOR-----------------

        //-----------------CONSTANTES-----------------

        public const int MAXDECK = 30;
        public const int MAXHAND = 7;
        public const int MAXBATTLEFIELD = 7;
        public const int MAXGOLD = 15;
        public const int TIMEPERTURN = 30;
        public const int MAXCOLLECTIBLEGOLDPERTURN = 6;
        public const int MINCOLLECTIBLEGOLDPERTURN = 3;
        public const int LIFEOFTHEFORT = 15;
        public const int INITDRAW = 4;

        //-----------------CONSTANTES-----------------

        //-----------------PROPERTIES---------------

        /// <summary>
        /// Guarda el jugador actual
        /// Cuando se hace un set, se inicializa el turno
        /// </summary>
        public Player WhoseTurn
        {
            get
            {
                return whoseTurn;
            }

            set
            {
                //Cuando se hace un set, se cambia de turno
                whoseTurn = value;
                timer.StartTimer();

                //Ocultamos todas las cartas del otro jugador
                whoseTurn.OtherPlayer.OnTurnEnd();

                //Informamos al jugador de que ha empezado su turno
                whoseTurn.OnTurnStart();

                //TODO: HIGHLIGHT PLAYABLE CARDS AND REMOVE HIGHLIGHTS FOR OPPONENT

                //TODO: Solución para que las cartas se renderizen en orden correcto. Esta no es una buena implementación
                whoseTurn.Canvas.sortingOrder = 2;
                whoseTurn.OtherPlayer.Canvas.sortingOrder = 1;

                //TODO: Puede no funcionar correctamente
                endTurnButton.interactable = true;
            }
        }
        private Player whoseTurn;

        public bool GameOver { get; set; }

        //-----------------PROPERTIES---------------


        //-----------------ATRIBUTOS PRIVADOS---------------

        /// <summary>
        /// Controla la duración de los turnos
        /// </summary>
        private Timer timer;

        //-----------------ATRIBUTOS PRIVADOS---------------

        //Cogemos referencias
        void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Inicio de la partida
        /// </summary>
        private void Start()
        {
            GameOver = false;
            for (int i = 0; i < 2; i++)
            {
                Players[i].BuildPlayer(i, TextManager.Instance.Decks[i]);
            }

            //Generamos un random para ver quien empieza el juego
            int rnd = Random.Range(0, 2);

            Player whoGoesSecond;
            if (rnd == 0)
            {
                whoseTurn = Players[0];
                whoGoesSecond = Players[1];
            }
            else
            {
                whoseTurn = Players[1];
                whoGoesSecond = Players[0];
            }

            //Roba 4 cartas el primer jugador y 5 el segundo jugador
            for (int i = 0; i < INITDRAW; i++)
            {
                //Roba el segundo
                whoGoesSecond.DrawACard();
                //Roba el primero
                whoseTurn.DrawACard();

                //Cada DrawACard genera su propio comand que se añadirá a la cola
            }

            //Una carta de más al segundo jugador
            whoGoesSecond.DrawACard();
            new StartATurnCommand(WhoseTurn).EnqueueComand();

            //TODO: Dar moneda al jugador 2?

            //Iniciamos contador
            timer = new Timer();

            StartCoroutine("TimeCount");
        }

        /// <summary>
        /// Corrutina que avanza el contador del turno
        /// </summary>
        /// <returns></returns>
        IEnumerator TimeCount()
        {
            while (!GameOver)
            {
                //Actualizar contador
                if (timer.Counting)
                {
                    //Avanza un frame en el contador
                    timer.TimeTillZero -= 1;

                    //Actualiza el texto
                    if (timerText != null)
                        timerText.text = timer.ToString();//Método de esta clase

                    //Comprobamos si ha acabado el turno
                    if (timer.TimeTillZero <= 0)
                        EndTurn();

                }
                yield return new WaitForSeconds(1.0f);
            }
        }

        /// <summary>
        /// Es llamado cuando pulsamos el boton de endTurn o el contador llega a cero
        /// </summary>
        public void EndTurn()
        {
            //Paramos el tiempo
            StopTheTimer();

            //Informamos al jugador actual que ha acabado su turno
            whoseTurn.OnTurnEnd();

            new StartATurnCommand(WhoseTurn.OtherPlayer).EnqueueComand();
        }

        /// <summary>
        /// Para el tiempo del turno
        /// </summary>
        public void StopTheTimer()
        {
            timer.StopTimer();

            endTurnButton.interactable = false;
        }

        /// <summary>
        /// Es llamado cuando algún jugador pierde
        /// </summary>
        public void ReloadScene()
        {        
            Debug.Log("Scene reloaded");
            // TODO: Reset ID?

            //Limpiar la lista de comandos
            Command.CommandQueue.Clear();
            Command.CommandComplete();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}