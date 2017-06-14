using System;

namespace Assets.Code.Components.AI.Behaviour.Nodes
{
    class SelectorNode : BehaviourNode
    {
        private BehaviourNode[] nodes;
        private int index;  // The current routine index

        public SelectorNode(BtContext context, BehaviourNode[] routines) : base(context)
        {
            if (routines == null || routines.Length == 0)
            {
                throw new ArgumentException("cannot be null or empty", "routines");
            }

            this.nodes = routines;
            index = 0;
        }

        public override void Start()
        {
            base.Start();
            nodes[index].Start();
        }

        public override void Act()
        {
            base.Act();
            nodes[index].Act();

            if (nodes[index].HasSucceeded)
            {
                this.Succeed();
            }
            else if (nodes[index].HasFailed)
            {
                index++;
                if (index >= nodes.Length)
                {
                    this.Fail();
                }
                else
                {
                    nodes[index].Start();
                }
            }
        }

        public override void Reset()
        {
            index = 0;
            foreach (BehaviourNode node in nodes)
            {
                node.Reset();
            }
        }

        public override void Stop()
        {
            index = 0;
            foreach (BehaviourNode node in nodes)
            {
                node.Stop();
            }
        }
    }
}
