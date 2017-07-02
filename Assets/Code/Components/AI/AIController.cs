using Assets.Code.Components.AI.Behaviour;
using Assets.Code.Components.Containers;
using Assets.Code.Constants;
using Assets.Code.Infrastructure.Unity;
using Assets.Code.Shared;
using UnityEngine;
using Zenject;

namespace Assets.Code.Components.AI
{
    public class AIController : MonoBehaviourExtension, IComponent 
    {
        private DiContainer container;
        private BehaviourTree tree;
        private WorldData worldData;

        [SerializeField]
        private float sightDistance;
        [SerializeField]
        private float fieldOfVision;

        /// <summary>
        /// Indicates a threshold inside which the target is detected regardless the vision logic.
        /// </summary>
        [SerializeField]
        private float detectionThreshold;

        /// <summary>
        /// The logic eyes GameObject transform
        /// </summary>
        [SerializeField]
        private Transform eyesTransform;

        [Inject]
        public void Inject(DiContainer container, WorldData worldData)
        {
            this.container = container;
            this.worldData = worldData;
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
            CheckIfPlayerIsInSight();
            tree.Update();
            Debug.DrawRay(this.eyesTransform.position, Quaternion.Euler(0, -fieldOfVision, 0) * this.transform.forward * this.sightDistance, Color.red);
            Debug.DrawRay(this.eyesTransform.position, Quaternion.Euler(0, fieldOfVision, 0) * this.transform.forward * this.sightDistance, Color.red);
        }

        /// <summary>
        /// Checks if a player is on sight, and updates the property 'PlayerOnSight'. In the future the BT should store the state variables that can affect the behviour.
        /// </summary>
        public void CheckIfPlayerIsInSight()
        {
            foreach (var player in worldData.Players)
            {
                // We use the center of the collider of the player to avoid raycasting to the feet of the player. An game object could be used to simplify the calculation.
                var vectorBetween = player.GetComponent<CapsuleCollider>().bounds.center - eyesTransform.position;

                if (IsTargetInsideDetectionThreshold(vectorBetween))
                {
                    SetTargetAsDetected(player);
                    return;
                }

                if (IsTargetInFieldOfVision(vectorBetween))
                {
                    RaycastHit hit;
                    Ray ray = new Ray(eyesTransform.position, vectorBetween);
                    Debug.DrawRay(eyesTransform.position, vectorBetween, Color.red);

                    // TODO: in the future the raycast should ignore some layers
                    // TODO: add logic to choose target if more than one player is in sight
                    if (Physics.Raycast(ray, out hit, this.SightDistance) &&
                        hit.collider.tag == Tags.Player)
                    {
                        SetTargetAsDetected(hit.collider.GetComponent<Character>());
                        return;
                    }
                }
            }

            UnsetTarget();
        }

        /// <summary>
        /// Returns true if the target is inside the vision area regardless of obstacles blocking the sight line
        /// </summary>
        /// <param name="vectorBetween">The vector between the entity and the target</param>
        /// <returns>True if the target</returns>
        private bool IsTargetInFieldOfVision(Vector3 vectorBetween)
        {
            return vectorBetween.magnitude <= this.SightDistance &&
                    Vector3.Angle(this.transform.forward, vectorBetween) <= this.FieldOfVision;
        }

        private bool IsTargetInsideDetectionThreshold(Vector3 vectorBetween)
        {
            return vectorBetween.magnitude <= detectionThreshold;
        }

        private void SetTargetAsDetected(Character player)
        {
            tree.SetBoolField(RegisteredFieldNames.PlayerInSight, true);
            tree.SetTarget(player);
        }

        private void UnsetTarget()
        {
            tree.SetBoolField(RegisteredFieldNames.PlayerInSight, false);
            tree.SetTarget(null);
        }

        public float SightDistance { get { return sightDistance; } }
        public float FieldOfVision { get { return fieldOfVision; } }
    }
}
