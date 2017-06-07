using Assets.Code.Components.Movement;
using UnityEngine;

namespace Assets.Code.Components.AI.Routines
{
    public class WalkRoutine : Routine
    {
        private const float distanceThreshold = 0.3f;
        Vector3 destination;
        IMovable movement;

        public WalkRoutine(AIController ai, Vector3 destination) : base(ai)
        {
            this.destination = destination;
            this.movement = ai.GetComponent<IMovable>();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Reset()
        {
            currentState = RoutineState.Running;
        }

        public override void Act()
        {
            if (Vector3.Distance(movement.transform.position, destination) < distanceThreshold)
            {
                movement.Move(0, 0);
                Succeed();
            }
            else
            {
                // TODO: change to a pathfinding algorithm and add exit condition
                Vector3 direction = destination - movement.transform.position;
                movement.Move(direction.x, direction.z);
            }
        }

        public Vector3 Destination
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;
            }
        }
    }
}
