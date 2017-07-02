using Assets.Code.Components.Movement;
using UnityEngine;

namespace Assets.Code.Components.AI.Behaviour.Nodes
{
    public class WalkNode : BehaviourNode
    {
        private const float minimumDistanceThreshold = 0.5f;
        private float distanceThreshold;
        Vector3 destination;
        IMovable movement;

        public WalkNode(BehaviourTreeContext context, Vector3 destination, float distanceThreshold = minimumDistanceThreshold) : base(context)
        {
            this.destination = destination;
            this.distanceThreshold = (minimumDistanceThreshold > distanceThreshold) ? minimumDistanceThreshold : distanceThreshold;
            this.movement = context.AI.GetComponent<IMovable>();
        }

        public void UpdateDestination(Vector3 destination, float distanceThreshold = minimumDistanceThreshold)
        {
            this.destination = destination;
            this.distanceThreshold = distanceThreshold;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Act()
        {
            base.Act();

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

        public override void Stop()
        {
            movement.Stop();
            base.Stop();
        }

        public override void Reset() { }
    }
}
