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
        private Collider weaponCollider;

        [SerializeField]
        private float baseDamage;
        private bool isAttacking;
        private bool hitboxActive;

        [Inject]
        private void Inject(Animator animator)
        {
            this.animator = animator;
        }

        void Awake()
        {
            isAttacking = false;
            hitboxActive = false;
        }

        void Start()
        {
            weaponCollider = LookUpWeaponCollider();
            DisableHitbox();
        }

        public void Attack()
        {
            isAttacking = true;
            animator.SetTrigger(AnimatorParameters.Attack);
        }

        public void Stop()
        {
            isAttacking = false;
        }

        public void EnableHitbox()
        {
            hitboxActive = true;
            if (weaponCollider != null)
            {
                weaponCollider.enabled = true;
            }
        }

        public void DisableHitbox()
        {
            hitboxActive = false;
            if (weaponCollider != null)
            {
                weaponCollider.enabled = false;
            }
        }

        public bool IsAttacking
        {
            get { return isAttacking; }
        }

        public bool IsHitboxEnabled
        {
            get { return hitboxActive; }
        }

        void OnTriggerEnter(Collider other)
        {
            IHealth otherHealth = other.GetComponent<IHealth>();

            // Makes character BIG
            //gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * 1.1f, gameObject.transform.localScale.y * 1.1f, 0);

            if (otherHealth != null && otherHealth.tag == Tags.Enemy)
            {
                otherHealth.TakeDamage(this.baseDamage);
            }
        }

        private Collider LookUpWeaponCollider()
        {
            var colliders = GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
            {
                if (collider.tag == Tags.Weapon)
                {
                    return collider;
                }
            }

            return null;
        }
    }
}
