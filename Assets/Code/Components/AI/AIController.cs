using Assets.Code.Infrastructure.Unity;

namespace Assets.Code.Components.AI
{
    public class AIController : MonoBehaviourExtension, IComponent 
    {
        private BehaviourTree tree;

        private void Awake()
        {
            tree = new BehaviourTree(this);
        }

        private void Start()
        {
            tree.Start();
        }

        private void Update()
        {
            tree.Update();
        }
    }
}
