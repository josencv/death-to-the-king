using Assets.Code.Components.Body;
using Assets.Code.Components.Weapon;
using Assets.Code.Constants;
using Assets.Code.Infrastructure.Unity;
using UnityEngine;
using Zenject;

namespace Assets.Code.Components.Movement
{
    /// <summary>
    /// This component enables normal movement for an entity
    /// </summary>
    public class NormalMovement : MonoBehaviourExtension, IMovable
    {
        [SerializeField]
        private float speed;

        private IWeapon weapon;
        private IBody logicalBody;
        private Rigidbody body;
        private Animator animator;

        [Inject]
        private void Inject(Rigidbody body, Animator animator, [InjectOptional] IBody logicalBody, [InjectOptional] IWeapon weapon)
        {
            this.body = body;
            this.animator = animator;
            this.weapon = weapon;
            this.logicalBody = logicalBody;
        }

        public void Move(float x, float z)
        {
            if (CanMove())
            {
               actions["Move"] = () => ApplyMovement(x, z);
            }
        }

        public void Stop()
        {
            actions["Move"] = () => ApplyMovement(0, 0);
        }

        private void ApplyMovement(float x, float z)
        {
            Vector3 movement = Vector3.ClampMagnitude(new Vector3(x, 0, z), 1);
            body.velocity = movement * speed;
            if (movement != Vector3.zero)
            {
                animator.SetBool(AnimatorParameters.IsWalking, true);
                transform.rotation = Quaternion.LookRotation(movement);
            }
            else
            {
                animator.SetBool(AnimatorParameters.IsWalking, false);
            }
        }

        private bool CanMove()
        {
            return (weapon == null || !weapon.IsAttacking) &&
                   (logicalBody == null || !logicalBody.IsInKnockback);
        }

        private void FixedUpdate()
        {
            ExecuteActions();
        }
    }
}
