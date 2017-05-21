using Assets.Code.Infrastructure.Unity;
using UnityEngine;
using Zenject;

namespace Assets.Code.Components.Health
{
    public class Health : MonoBehaviourExtension, IHealth
    {
        private Animator animator;

        [SerializeField]
        private int maxHealth;

        private bool isDead;
        private float currentHealth;

        public void Awake()
        {
            isDead = false;
            currentHealth = maxHealth;
        }

        [Inject]
        private void Inject(Animator animator)
        {
            this.animator = animator;
        }
         
        public void TakeDamage(float amount)
        {
            if (!isDead)
            {
                currentHealth -= amount;
                if (currentHealth <= 0)
                {
                    Die();
                }
            }
        }

        private void Die()
        {
            isDead = true;
            Destroy(gameObject);
        }
    }
}
