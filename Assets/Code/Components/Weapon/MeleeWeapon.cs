using Assets.Code.Components.Health;
using Assets.Code.Constants;
using Assets.Code.Infrastructure.Unity;
using UnityEngine;
using Zenject;

namespace Assets.Code.Components.Weapon
{
    public class MeleeWeapon : MonoBehaviourExtension, IWeapon
    {
        private Animator animator;

        public float baseDamage;

        [Inject]
        private void Inject(Animator animator)
        {
            this.animator = animator;
        }

        public void Attack()
        {
            animator.SetTrigger(AnimatorParameters.Attack);
        }

        public bool IsAttacking
        {
            get { return animator.GetCurrentAnimatorStateInfo(0).IsName(AnimatorStates.Attack); }
        }

        void OnTriggerEnter(Collider other)
        {
            IHealth otherHealth = other.GetComponent<IHealth>();

            // Makes character BIG
            //gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * 1.1f, gameObject.transform.localScale.y * 1.1f, 0);

            if (otherHealth != null && otherHealth.tag == "Enemy")
            {
                otherHealth.TakeDamage(this.baseDamage);
            }
        }
    }
}
