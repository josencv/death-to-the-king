using Assets.Code.Components.AI.Routines;
using Zenject;

namespace Assets.Code.Components.AI
{
    public class BehaviourTree
    {
        private Routine root;
        private AIController ai;
        private DiContainer container;

        public BehaviourTree(AIController ai)
        {
            this.ai = ai;
        }

        [Inject]
        public void Inject(DiContainer container)
        {
            this.container = container;
        }

        public void Initialize()
        {
            // TODO: this should be built from a file... probably
            var wander = new WanderRoutine(ai);
            var selector = new SelectorRoutine(ai, new Routine[] { wander });
            var repeat = new RepeatRoutine(ai, selector);
            root = repeat;
        }

        public void Start()
        {
            root.Start();
        }

        public void Update()
        {
            root.Act();
        }

        public void Reevaluate()
        {
            root.Restart();
        }
    }
}
