namespace Assets.Code.Components.AI.Behaviour.Nodes
{
    public class ApproachNode : BehaviourNode
    {
        private WalkNode walk;
        private BtContext context;

        public ApproachNode(BtContext context) : base(context)
        {
            this.context = context;
        }

        public override void Start()
        {
            walk = new WalkNode(context, context.Target.transform.position);
            walk.Start();
            base.Start();
        }

        public override void Reset() { }

        public override void Act()
        {
            base.Act();
            if (walk.HasSucceeded)
            {
                this.Succeed();
            }
            else if (walk.HasFailed)
            {
                this.Fail();
            }
            else
            {
                walk.UpdateDestination(context.Target.transform.position);
                walk.Act();
            }
        }

        public override void Stop()
        {
            if (walk != null)
            {
                walk.Stop();
            }

            base.Stop();
        }
    }
}
