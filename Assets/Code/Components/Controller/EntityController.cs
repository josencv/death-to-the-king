using Assets.Code.Components.Movement;
using Assets.Code.Infrastructure.Input;
using Assets.Code.Infrastructure.Unity;
using Zenject;

namespace Assets.Code.Components.Controller
{
    public class EntityController : MonoBehaviourExtension, IEntityController
    {
        private IMovable movement;

        [Inject]
        private void Inject(IMovable movement)
        {
            this.movement = movement;
        }

        public void Initialize(IGameInput gameInput)
        {
            gameInput.PlayerMoveSignal += movement.Move;
        }
    }
}
