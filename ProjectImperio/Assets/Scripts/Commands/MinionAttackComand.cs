using UnityEngine;
using DG.Tweening;

namespace Imperio
{
    /// <summary>
    /// Comando ataca a un minion
    /// </summary>
    public class MinionAttackCommand : Command
    {
        private Minion attacker;
        private Minion target;
        private Vector3 initialPos;

        public MinionAttackCommand(Minion attacker, Minion target)
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
            DamageEffect.CreateDamageEffect(target.transform.position, -attacker.Attack);

            target.CurrentHealth -= attacker.Attack;
            attacker.CurrentHealth -= target.Attack;

            attacker.transform.DOMove(initialPos, 0.5f).SetEase(Ease.OutQuint).onComplete += OnEndBack;
        }

        void OnEndBack()
        {
            DamageEffect.CreateDamageEffect(attacker.transform.position, -target.Attack);

            CommandComplete();
        }

    }
}