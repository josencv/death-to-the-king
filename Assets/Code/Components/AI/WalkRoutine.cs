using Assets.Code.Components.Movement;
using UnityEngine;

namespace Assets.Code.Components.AI
{
    public class WalkRoutine : Routine
    {
        private const float distanceThreshold = 0.3f;
        Vector3 destination;
        IMovable movement;

        public WalkRoutine(IComponent entity, Vector3 destination) : base(entity)
        {
            this.destination = destination;
            this.movement = entity.GetComponent<IMovable>();
        }

        public override void Start()
        {
            Debug.logger.Log("Walk routine: Start");
            base.Start();
        }

        public override void Reset()
        {
            Debug.logger.Log("Walk routine: Reset");
            currentState = RoutineState.Running;
        }

        public override void Act()
        {
            Debug.logger.Log("Walk routine: Act");
            if (Vector3.Distance(movement.transform.position, destination) < distanceThreshold)
            {
                Debug.logger.Log("Walk routine: Succeeded");
                movement.Move(0, 0);
                Succeed();
            }
            else
            {
                // TODO: change to a pathfinding algorithm and add exit condition
                Debug.logger.Log("Walk routine: Walking");
                Vector3 direction = destination - movement.transform.position;
                movement.Move(direction.x, direction.z);
            }
        }
    }
}
