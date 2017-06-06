using Assets.Code.Components.AI.Routines;

namespace Assets.Code.Components.AI
{
    public class BehaviourTree
    {
        private Routine root;
        private IComponent entity;

        public BehaviourTree(IComponent entity)
        {
            // TODO: this should be built from a file... probably
            var wander = new WanderRoutine(entity);
            root = new RepeatRoutine(entity, wander);
        }

        public void Start()
        {
            root.Start();
        }

        public void Update()
        {
            root.Act();
        }
    }
}
