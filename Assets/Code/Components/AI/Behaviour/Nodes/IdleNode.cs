using Assets.Code.Components.Movement;
using UnityEngine;

namespace Assets.Code.Components.AI.Behaviour.Nodes
{
    class IdleNode : BehaviourNode
    {
        private IMovable movement;
        private float idleDuration;
        private float currentTime;

        public IdleNode(BehaviourTreeContext context, float idleDuration) : base(context)
        {
            movement = context.AI.GetComponent<IMovable>();
            this.idleDuration = idleDuration;
            currentTime = 0;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Act()
        {
            base.Act();
            currentTime += Time.deltaTime;

            if (currentTime >= idleDuration)
            {
                this.Succeed();
            }
        }

        public override void Reset()
        {
            currentTime = 0;
        }
    }
}
