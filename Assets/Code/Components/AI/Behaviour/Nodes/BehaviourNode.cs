using Assets.Code.Components.Body;

namespace Assets.Code.Components.AI.Behaviour.Nodes
{
    public abstract class BehaviourNode
    {
        protected NodeState currentState;
        protected IBody body;

        public BehaviourNode(BehaviourTreeContext context)
        {
            body = context.AI.GetComponent<IBody>();
            currentState = NodeState.Stopped;
        }

        public virtual void Start()
        {
            this.currentState = NodeState.Running;
        }

        public virtual void Act()
        {
            if (body.IsDead)
            {
                this.Fail();
            }
        }

        public virtual void Stop()
        {
            this.currentState = NodeState.Stopped;
        }

        public abstract void Reset();

        public virtual void Restart()
        {
            this.Stop();
            this.Reset();
            this.Start();
        }

        public virtual void Succeed()
        {
            this.currentState = NodeState.Success;
        }

        public virtual void Fail()
        {
            this.currentState = NodeState.Failure;
        }

        public bool IsStopped { get { return currentState == NodeState.Stopped; } }
        public bool IsRunning { get { return currentState == NodeState.Running; } }
        public bool HasSucceeded { get { return currentState == NodeState.Success; } }
        public bool HasFailed { get { return currentState == NodeState.Failure; } }
    }
}
