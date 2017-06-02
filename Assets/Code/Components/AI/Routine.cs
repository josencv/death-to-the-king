using Assets.Code.Components.Body;

namespace Assets.Code.Components.AI
{
    public abstract class Routine
    {
        protected RoutineState currentState;
        protected IBody body;

        public Routine(IComponent entity)
        {
            body = entity.GetComponent<IBody>();
            currentState = RoutineState.Stopped;
        }

        public abstract void Reset();

        public virtual void Act()
        {
            if (body.IsDead)
            {
                this.Fail();
            }
        }

        public virtual void Start()
        {
            this.currentState = RoutineState.Running;
        }

        public virtual void Succeed()
        {
            this.currentState = RoutineState.Success;
        }

        public virtual void Fail()
        {
            this.currentState = RoutineState.Failure;
        }

        public bool IsStopped { get { return currentState == RoutineState.Stopped; } }
        public bool IsRunning { get { return currentState == RoutineState.Running; } }
        public bool HasSucceeded { get { return currentState == RoutineState.Success; } }
        public bool HasFailed { get { return currentState == RoutineState.Failure; } }
    }
}
