using UnityEngine;
using DG.Tweening;

namespace Imperio
{
    //TODO: FACE Y MINION??
    /// <summary>
    /// Comando ataca al player
    /// </summary>
    public class MinionAttackFaceCommand : Command
    {
        private Minion attacker;
        private Fort target;
        private Vector3 initialPos;

        public MinionAttackFaceCommand(Minion attacker, Fort target)
        {
            this.attacker = attacker;
            this.target = target;
            initialPos = attacker.transform.position;
        }

        /// <summary>
        /// Empieza la reproducción del comando: atacar.
        /// Mueve la carta haciendo un efecto de golpe.
        /// Actualiza la vida de la carta.
        /// </summary>
        public override void CommandStart()
        {
            attacker.transform.DOMove(target.transform.position, 0.5f).SetEase(Ease.OutQuint).onComplete += OnEndAttack;
        }

        void OnEndAttack()
        {
            target.Health -= attacker.Attack;

            attacker.transform.DOMove(initialPos, 0.5f).SetEase(Ease.OutQuint).onComplete += OnEndBack;
            DamageEffect.CreateDamageEffect(target.transform.position, -attacker.Attack);
        }

        void OnEndBack()
        {
            CommandComplete();
        }

    }
}