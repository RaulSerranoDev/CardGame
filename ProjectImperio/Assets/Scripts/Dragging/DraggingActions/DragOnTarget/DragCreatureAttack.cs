using UnityEngine;
using UnityEngine.UI;

namespace Imperio
{
    /// <summary>
    /// Componente adjuntado a los minions que permite atacar
    /// </summary>
    public class DragCreatureAttack : DragOnTarget
    {
        /// <summary>
        /// Referencia al minion que ataca
        /// </summary>
        private Minion attacker;

        /// <summary>
        /// Obtiene referencias y pone el color inicial
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            attacker = GetComponentInParent<Minion>();
        }

        /// <summary>
        /// Oculta la diana y ataca el objetivo en caso de que tenga.
        /// Distinguiendo entre Minion y Fuerte
        /// </summary>
        public override void OnEndDrag()
        {
            base.OnEndDrag();

            //Si se ha elegido un objetivo correcto (minion enemigo o fuerte enemigo)
            if (DragSuccessful())
            {
                //Si estamos atacando a un minion
                Minion minion = Target.GetComponent<Minion>();
                if (minion != null)
                    attacker.AttackMinion(minion);

                //Si estamos atacando a un fuerte
                else
                {
                    Fort fort = Target.GetComponent<Fort>();
                    if (fort != null)
                        attacker.AttackFort(fort);
                }
            }
        }

        //TODO: A LO MEJOR NO ESTÁ DEMASIADO BIEN
        /// <summary>
        /// Determina si el Drag es válido. Comprueba que el Target es un minion del enemigo o el fuerte
        /// </summary>
        /// <returns></returns>
        protected override bool DragSuccessful()
        {
            if (Target != null)
            {
                //Si estamos atacando a un minion
                Minion minion = Target.GetComponent<Minion>();
                if (minion != null)
                {
                    //Comprobamos si atacamos a un enemigo
                    if (minion.Owner != attacker.Owner)
                    {
                        //Comprobamos si no ha muerto ya
                        if (minion.CurrentHealth > 0)
                        {
                            //Comprobamos si el enemigo tiene STEALTH
                            if (!minion.Stealth)
                            {
                                //Comprobamos si el enemigo tiene TAUNT
                                if (minion.Taunt)
                                {
                                    return true;
                                }
                                //Si el enemigo no tiene TAUNT, comprobamos si hay algún minion con TAUNT en en BATTLEFIELD
                                else if (!minion.Owner.PlayerBattlefield.SomeoneWithTaunt())
                                {
                                    return true;
                                }
                                else
                                    new ShowMessageCommand("Hay esbirros que se interponen con provocar", 1.0f).EnqueueComand();
                            }
                            else
                                new ShowMessageCommand("No puedes atacar un esbirro con sigilo",1.0f).EnqueueComand();
                        }
                    }
                    return false;
                }
                //Si estamos atacando a un fuerte
                else
                {
                    Fort fort = Target.GetComponent<Fort>();
                    if (fort != null)
                    {
                        //Si estamos atacando al fuerte enemigo
                        if (attacker.Owner != fort.Player)
                        {
                            //Si hay algún minion con Taunt, no podemos atacar al fuerte
                            if (!fort.Player.PlayerBattlefield.SomeoneWithTaunt())
                                return true;
                            else
                                new ShowMessageCommand("Hay esbirros que se interponen con provocar", 1.0f).EnqueueComand();
                        }
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Devuelve si podemos coger esta carta actualmente
        /// </summary>
        /// <returns></returns>
        public override bool CanDrag()
        {
            return attacker.CanAttack();
        }
    }
}