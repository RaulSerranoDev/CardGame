using System.Collections.Generic;

namespace Imperio
{
    /// <summary>
    /// Clase base para todos los comandos
    /// El manage de los commands se hace en esta misma clase, con métodos estáticos, en vez de tener un command manager
    /// Envian mensajes a la parte visual, le dice todo lo que tiene que mostrar
    /// </summary>
    public class Command
    {
        /// <summary>
        /// Colección de comandos
        /// </summary>
        public static Queue<Command> CommandQueue = new Queue<Command>();

        /// <summary>
        /// Cuando empezamos a reproducir comandos, esta variable se actualiza a true
        /// Cuando acabamos de mostrar todo al usuario, se vuelve a poner a false
        /// </summary>
        private static bool PlayingQueue = false;

        /// <summary>
        /// Añade un comando y lo reproduce si no hay nada reproduciendose
        /// </summary>
        public virtual void EnqueueComand()
        {
            CommandQueue.Enqueue(this);

            //Si no estamos reproduciendo, le decimos que reproduzca
            if (!PlayingQueue)
                PlayNextCommand();
        }

        /// <summary>
        /// Cada clase commando lo implementa
        /// Empieza la reproducción del comando especifico
        /// </summary>
        public virtual void CommandStart()
        {
            //Tenemos una lista de todo lo que tenemos que hacer con este comando (robar carta, jugar carta, jugar hechizo,etc...)
            //2 opciones para timing:
            //1) Usar tween sequences y llamar a CommandExecutionComplete in OnComplete()
            //2) Usar corrutinas (IEnumerator) y WaitFor... para introducir delays, 
            //llamamos CommandExecutionComplete() cuando acabe la corrutina
        }

        /// <summary>
        /// Es llamado cuando acaba la reproducción de un comando. Si hay más comandos, los reproduce
        /// </summary>
        public static void CommandComplete()
        {
            //Comprobamos si quedan comandos por reproducir y los reproducimos
            if (CommandQueue.Count > 0)
                PlayNextCommand();

            //No hay más comandos para reproducir
            else
                PlayingQueue = false;

        }

        /// <summary>
        /// Elimina de la cola el primer comando y empieza su ejecucón
        /// </summary>
        public static void PlayNextCommand()
        {
            PlayingQueue = true;
            CommandQueue.Dequeue().CommandStart();
        }
    }
}