using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Imperio
{
    /// <summary>
    /// Componente adjuntado a los hechizos que permite lanzarlos a un objetivo
    /// </summary>
    public class DragTargetSpellCard : DragOnTarget
    {
        /// <summary>
        /// Referencia al minion que ataca
        /// </summary>
        private SpellCard spell;

        /// <summary>
        /// Obtiene referencias y pone el color inicial
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            spell = GetComponentInParent<SpellCard>();
        }

        /// <summary>
        /// Oculta la diana y juega el hechizo si tiene un objetivo válido
        /// </summary>
        public override void OnEndDrag()
        {
            base.OnEndDrag();

            if (DragSuccessful())
            {
                //Si estamos atacando a un minion
                Minion minion = Target.GetComponent<Minion>();
                if (minion != null)
                    spell.Owner.PlayATargetSpellFromHand(spell, minion);

                //Si estamos atacando a un fuerte
                else
                {
                    Fort fort = Target.GetComponent<Fort>();
                    if (fort != null)
                        spell.Owner.PlayATargetSpellFromHand(spell, fort);
                }
            }

        }

        /// <summary>
        /// Devuelve true si el objeto es válido para el hechizo
        /// </summary>
        /// <returns></returns>
        protected override bool DragSuccessful()
        {
            if (Target != null)
            {
                Minion minion = Target.GetComponent<Minion>();
                if (minion != null)
                {
                    switch (spell.SpellTarget)
                    {
                        case SpellTarget.MINION:
                            return true;

                        case SpellTarget.ENEMYMINION:
                            return (minion.Owner != spell.Owner);

                        case SpellTarget.ALLYMINION:
                            return (minion.Owner == spell.Owner);

                        case SpellTarget.CHARACTER:
                            return true;

                        case SpellTarget.ALLYCHARACTER:
                            return (minion.Owner == spell.Owner);

                        case SpellTarget.ENEMYCHARACTER:
                            return (minion.Owner != spell.Owner);
                    }

                }
                else
                {
                    Fort fort = Target.GetComponent<Fort>();

                    if (fort != null)
                    {
                        switch (spell.SpellTarget)
                        {
                            case SpellTarget.CHARACTER:
                                return true;

                            case SpellTarget.ALLYCHARACTER:
                                return (fort.Player == spell.Owner);

                            case SpellTarget.ENEMYCHARACTER:
                                return (fort.Player != spell.Owner);
                        }

                    }
                }

            }

            //TODO CON LOS HECHIZOS
            return false;
        }

        /// <summary>
        /// Devuelve si podemos coger esta carta actualmente
        /// </summary>
        /// <returns></returns>
        public override bool CanDrag()
        {
            return spell.CanBePlayed();
        }
    }
}