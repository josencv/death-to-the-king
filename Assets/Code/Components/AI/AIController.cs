using Assets.Code.Infrastructure.Unity;

namespace Assets.Code.Components.AI
{
    public class AIController : MonoBehaviourExtension, IComponent 
    {
        private Routine routine;

        private void Awake()
        {
            BuildBehaviourTree();
        }

        private void BuildBehaviourTree()
        {
            var wander = new WanderRoutine(this);
            var root = new RepeatRoutine(this, wander);
            routine = root;
        }

        private void Start()
        {
            routine.Start();
        }

        private void Update()
        {
            routine.Act();
        }
    }
}
