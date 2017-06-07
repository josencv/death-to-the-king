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
            var detect = new DetectRoutine(ai);
            container.Inject(detect);
            var wander = new WanderRoutine(ai);
            root = new RepeatRoutine(ai, wander);
            root = detect;
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
