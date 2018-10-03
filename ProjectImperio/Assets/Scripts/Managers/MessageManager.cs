using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Imperio
{
    /// <summary>
    /// Manager de mensajes. Permite mostrar un panel con un mensaje en la duración establecida. Se añade a la cola de comandos
    /// Clase estática
    /// </summary>
    public class MessageManager : MonoBehaviour
    {
        // Singleton
        public static MessageManager Instance;

        /// <summary>
        /// Referencia al texto del mensaje
        /// </summary>
        public Text MessageText;

        /// <summary>
        /// Referencia al panel del mensaje
        /// </summary>
        public GameObject MessagePanel;//Referencia al panel del mensaje

        void Awake()
        {
            Instance = this;
            MessagePanel.SetActive(false);//Empieza inactivo
        }

        /// <summary>
        /// Muestra un mensaje con una duración
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Duration"></param>
        public void ShowMessage(string Message, float Duration)
        {
            StartCoroutine(ShowMessageCoroutine(Message, Duration));
        }

        /// <summary>
        /// Corrutina que muestra el mensaje y lo desactiva
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Duration"></param>
        /// <returns></returns>
        IEnumerator ShowMessageCoroutine(string Message, float Duration)
        {
            MessageText.text = Message;
            MessagePanel.SetActive(true);

            yield return new WaitForSeconds(Duration);

            MessagePanel.SetActive(false);
            Command.CommandComplete();//Tenemos que informar al manager de comandds
        }
    }
}