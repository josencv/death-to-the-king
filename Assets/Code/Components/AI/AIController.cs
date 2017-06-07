using Assets.Code.Infrastructure.Unity;
using UnityEngine;
using Zenject;

namespace Assets.Code.Components.AI
{
    public class AIController : MonoBehaviourExtension, IComponent 
    {
        private DiContainer container;
        private BehaviourTree tree;

        [SerializeField]
        private float sightDistance;
        [SerializeField]
        private float fieldOfVision;

        /// <summary>
        /// The eyes y axis position relative to the transform
        /// </summary>
        [SerializeField]
        private float eyesRelativePosition;

        [Inject]
        public void Inject(DiContainer container)
        {
            this.container = container;
        }

        private void Awake()
        {
            tree = new BehaviourTree(this);
            container.Inject(tree);
            tree.Initialize();
        }

        private void Start()
        {
            tree.Start();
        }

        private void Update()
        {
            tree.Update();
            Debug.DrawRay(this.transform.position + Vector3.up * eyesRelativePosition, Quaternion.Euler(0, -fieldOfVision, 0) * this.transform.forward * this.sightDistance, Color.red);
            Debug.DrawRay(this.transform.position + Vector3.up * eyesRelativePosition, Quaternion.Euler(0, fieldOfVision, 0) * this.transform.forward * this.sightDistance, Color.red);
        }

        public float SightDistance { get { return sightDistance; } }
        public float FieldOfVision { get { return fieldOfVision; } }
        public float EyesRelativePosition { get { return eyesRelativePosition; } }
        public Vector3 EyesPosition { get { return this.transform.position + Vector3.up * eyesRelativePosition; } }
    }
}
