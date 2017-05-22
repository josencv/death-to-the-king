using Assets.Code.Components.Body;
using Assets.Code.Constants;
using Assets.Code.Infrastructure.Unity;
using UnityEngine;
using Zenject;

namespace Assets.Code.Components.Weapon
{
    public class MeleeWeapon : MonoBehaviourExtension, IWeapon
    {
        private Animator animator;

        [SerializeField]
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

        private void Awake()
        {
            isAttacking = false;
            hitboxActive = false;
        }

        private void Start()
        {
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

        private void OnTriggerEnter(Collider other)
        {
            IBody otherBody = other.GetComponent<IBody>();

            // Makes character BIG
            //gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * 1.1f, gameObject.transform.localScale.y * 1.1f, 0);

            if (otherBody != null && otherBody.tag == Tags.Enemy)
            {
                Vector3 direction = otherBody.transform.position - transform.position;
                direction = new Vector3(direction.x, 0, direction.z);
                otherBody.Hit(this.baseDamage, direction);
            }
        }
    }
}
