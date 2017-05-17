using Assets.Code.Components.Movement;
using Assets.Code.Components.Weapon;
using Assets.Code.Constants;
using Assets.Code.Infrastructure.Input;
using Assets.Code.Infrastructure.Unity;
using UnityEngine;
using Zenject;

namespace Assets.Code.Components.Controller
{
    public class EntityController : MonoBehaviourExtension, IEntityController
    {
        private IMovable movement;
        private IWeapon weapon;
        private Animator animator;

        [Inject]
        private void Inject(IMovable movement, IWeapon weapon, Animator animator)
        {
            this.movement = movement;
            this.weapon = weapon;
            this.animator = animator;
        }

        public void Initialize(IGameInput gameInput)
        {
            gameInput.PlayerMoveSignal += movement.Move;
            gameInput.PlayerAttackSignal += weapon.Attack;
        }

        public bool IsAttacking
        {
            get { return animator.GetCurrentAnimatorStateInfo(0).IsName(AnimatorStates.Attack); }
        }
    }
}
