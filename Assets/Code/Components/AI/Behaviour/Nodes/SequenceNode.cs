using System;

namespace Assets.Code.Components.AI.Behaviour.Nodes
{
    class SequenceNode : BehaviourNode
    {
        private BehaviourNode[] nodes;
        private int index;  // The current routine index

        public SequenceNode(BtContext context, BehaviourNode[] routines) : base(context)
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
                index++;
                if (index >= nodes.Length)
                {
                    this.Succeed();
                }
                else
                {
                    nodes[index].Start();
                }
            }
            else if (nodes[index].HasFailed)
            {
                this.Fail();
            }
        }

        public override void Reset()
        {
            index = 0;
            foreach (BehaviourNode ndoe in nodes)
            {
                ndoe.Reset();
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
