using Assets.Code.Constants;
using Assets.Code.Infrastructure.Unity;
using UnityEngine;
using Zenject;

namespace Assets.Code.Components.Weapon
{
    public class MeleeWeapon : MonoBehaviourExtension, IWeapon
    {
        private Animator animator;

        [Inject]
        private void Inject(Animator animator)
        {
            this.animator = animator;
        }

        public void Attack()
        {
            animator.SetTrigger(AnimatorParameters.Attack);
        }
    }
}
