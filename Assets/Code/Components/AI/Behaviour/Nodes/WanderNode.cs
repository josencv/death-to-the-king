using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code.Components.AI.Behaviour.Nodes
{
    class WanderNode : BehaviourNode
    {
        private SequenceNode sequenceNode;
        private WalkNode walkNode;
        private IdleNode idleNode;
        private Transform transform;
        private float wanderRadius;

        /// <summary>
        /// Initiales a Wander routine
        /// </summary>
        /// <param name="wanderRadius">The max possible radius to wander to from current position.</param>
        /// <param name="idleDuration">The iddle duration between movement in seconds.</param>
        public WanderNode(BehaviourTreeContext context, float wanderRadius = 1, float idleDuration = 4) : base(context)
        {
            this.wanderRadius = wanderRadius;
            transform = context.AI.GetComponent<Transform>();
            walkNode = new WalkNode(context, GenerateDestination(wanderRadius));
            idleNode = new IdleNode(context, idleDuration);
            sequenceNode = new SequenceNode(context, new BehaviourNode[] { idleNode, walkNode });
        }

        public override void Start()
        {
            sequenceNode.Start();
            base.Start();
        }

        public override void Act()
        {
            base.Act();

            if (sequenceNode.HasFailed)
            {
                this.Fail();
            }
            else if (sequenceNode.HasSucceeded)
            {
                this.Succeed();
            }
            else
            {
                sequenceNode.Act();
            }
        }

        public override void Stop()
        {
            idleNode.Stop();
            walkNode.Stop();
        }

        public override void Reset()
        {
            sequenceNode.Reset();
            walkNode.Reset();
            walkNode.UpdateDestination(GenerateDestination(wanderRadius));
        }

        private Vector3 GenerateDestination(float radius)
        {
            Vector2 randomOffset = Random.insideUnitCircle.normalized * wanderRadius;
            return new Vector3(
                transform.position.x + randomOffset.x,
                transform.position.y,
                transform.position.z + randomOffset.y);
        }
    }
}
