using Assets.Code.Components.Body;
using Assets.Code.Components.Movement;
using Assets.Code.Constants;
using Assets.Code.Infrastructure.Unity;
using UnityEngine;
using Zenject;

namespace Assets.Code.Components.Weapon
{
    public class MeleeWeapon : MonoBehaviourExtension, IWeapon
    {
        private Animator animator;
        private IMovable movement;

        [SerializeField]
        private Collider weaponCollider;

        [SerializeField]
        private float baseDamage;
        private bool isAttacking;
        private bool hitboxActive;
        private float attackRange;

        public event AttackFinishedHandler AttackFinished;

        [Inject]
        private void Inject(Animator animator, IMovable movement)
        {
            this.animator = animator;
            this.movement = movement;
        }

        private void Awake()
        {
            isAttacking = false;
            hitboxActive = false;
            attackRange = 0.3f; // TODO: maybe this distance can be calculated based in the weapon collider
        }

        private void Start()
        {
            DisableHitbox();
        }

        public void Attack()
        {
            if (!CanAttack())
            {
                return;
            }

            isAttacking = true;
            movement.Stop();
            animator.SetTrigger(AnimatorParameters.Attack);
        }

        public void Stop()
        {
            isAttacking = false;
            DisableHitbox();
            if (AttackFinished != null)
            {
                AttackFinished.Invoke();
            }
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

        public bool CanAttack()
        {
            return !isAttacking;
        }

        public float AttackRange { get { return attackRange; } }
        public float Damage { get { return baseDamage; } }
    }
}
