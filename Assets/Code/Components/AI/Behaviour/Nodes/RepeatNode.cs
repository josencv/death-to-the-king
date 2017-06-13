namespace Assets.Code.Components.AI.Behaviour.Nodes
{
    class RepeatNode : BehaviourNode
    {
        private BehaviourNode node;
        private int times;
        private int counter;

        /// <summary>
        /// Initializes the repeat routine. 
        /// </summary>
        /// <param name="node">The node to repeat</param>
        /// <param name="times">How many times should the routine be repeated. If not set irt will wun indefinitely until it fails.</param>
        public RepeatNode(BtContext context, BehaviourNode node, int times = -1) : base(context)
        {
            this.node = node;
            this.times = times;
            counter = 0;
        }

        public override void Start()
        {
            counter++;
            base.Start();
            this.node.Start();
        }

        public override void Reset()
        {
            counter = 0;
        }

        public override void Act()
        {
            base.Act();

            if (node.IsStopped)
            {
                node.Start();
            }
            else if (node.HasFailed)
            {
                this.Fail();
            }
            else if (node.HasSucceeded)
            {
                if (times == -1 || counter < times)
                {
                    counter++;
                    node.Restart();
                }
                else
                {
                    this.Succeed();
                }
            }
            else
            {
                node.Act();
            }
        }
    }
}
