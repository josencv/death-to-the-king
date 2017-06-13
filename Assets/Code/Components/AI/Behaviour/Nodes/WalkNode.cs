using Assets.Code.Components.Movement;
using UnityEngine;

namespace Assets.Code.Components.AI.Behaviour.Nodes
{
    public class WalkNode : BehaviourNode
    {
        private const float distanceThreshold = 0.3f;
        Vector3 destination;
        IMovable movement;

        public WalkNode(BtContext context, Vector3 destination) : base(context)
        {
            this.destination = destination;
            this.movement = context.AI.GetComponent<IMovable>();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Reset()
        {
            currentState = NodeState.Running;
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
