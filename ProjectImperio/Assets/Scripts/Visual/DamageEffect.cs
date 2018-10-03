using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Imperio
{
    /// <summary>
    /// Efecto que muestra el daño hecho a minions o players
    /// </summary>
    public class DamageEffect : MonoBehaviour
    {
        /// <summary>
        /// Referencia para poder modificar el alpha el objeto
        /// </summary>
        [SerializeField]
        private CanvasGroup cg;

        /// <summary>
        /// Referencia al texto
        /// </summary>
        [SerializeField]
        private Text AmountText;

        /// <summary>
        /// Crea el damage effect en la posición y con el daño establecido
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="amount">Amount.</param>
        public static void CreateDamageEffect(Vector3 position, int amount)
        {
            if (amount == 0)
                return;

            GameObject newDamageEffect = GameObject.Instantiate(GameManager.Instance.DamageEffectPrefab, position, Quaternion.identity, GameManager.Instance.GlobalCanvas.transform);

            DamageEffect de = newDamageEffect.GetComponent<DamageEffect>();
            de.AmountText.text = amount.ToString();

            //Iniciamos la corrutina que lo vuelve transparente
            de.StartCoroutine(de.ShowDamageEffect());
        }

        //Corrutina que controla el Fading
        private IEnumerator ShowDamageEffect()
        {
            //Opaco
            cg.alpha = 1f;

            yield return new WaitForSeconds(1f);

            //Modificamos gradualmente el alpha
            while (cg.alpha > 0)
            {
                cg.alpha -= 0.05f;
                yield return new WaitForSeconds(0.05f);
            }

            //Cuando se vuelve transparente, se destruye
            Destroy(gameObject);
        }

    }
}