using UnityEngine;

namespace Imperio
{
    /// <summary>
    /// Clase contador de turnos
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// Guarda cuanto tiempo queda hasta que el Timer llegue a 0
        /// </summary>
        public float TimeTillZero;

        /// <summary>
        /// Es true cuando esté avanzando el tiempo
        /// </summary>
        public bool Counting;

        public Timer()
        {
            Counting = false;
            TimeTillZero = 0;
        }

        /// <summary>
        /// Es llamado por Scripts externos para empezar a contar
        /// </summary>
        public void StartTimer()
        {
            //Empieza a contar
            TimeTillZero = GameManager.TIMEPERTURN; //El tiempo que queda es el tiempo total de turno
            Counting = true;
        }

        /// <summary>
        /// Deja de contar
        /// </summary>
        public void StopTimer()
        {
            Counting = false;
        }

        /// <summary>
        /// Muestra el tiempo en un formato de reloj
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int inSeconds = Mathf.RoundToInt(TimeTillZero);
            string justSeconds = (inSeconds % 60).ToString();
            if (justSeconds.Length == 1)
                justSeconds = "0" + justSeconds;
            string justMinutes = (inSeconds / 60).ToString();
            if (justMinutes.Length == 1)
                justMinutes = "0" + justMinutes;

            return string.Format("{0}:{1}", justMinutes, justSeconds);
        }
    }
}