﻿using Assets.Code.Constants;
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

        private bool playerOnsight;

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
        public void Inject(DiContainer container, WorldData worldData)
        {
            this.container = container;
            this.worldData = worldData;
        }

        private void Awake()
        {
            playerOnsight = false;
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
            Debug.DrawRay(this.transform.position + Vector3.up * eyesRelativePosition, Quaternion.Euler(0, -fieldOfVision, 0) * this.transform.forward * this.sightDistance, Color.red);
            Debug.DrawRay(this.transform.position + Vector3.up * eyesRelativePosition, Quaternion.Euler(0, fieldOfVision, 0) * this.transform.forward * this.sightDistance, Color.red);
        }

        /// <summary>
        /// Checks if a player is on sight, and updates the property 'PlayerOnSight'. In the future the BT should store the state variables that can affect the behviour.
        /// </summary>
        public void CheckIfPlayerIsInSight()
        {
            foreach (var player in worldData.Players)
            {
                var eyesPosition = this.EyesPosition;
                var vectorBetween = player.GetComponent<CapsuleCollider>().bounds.center - eyesPosition;
                if (vectorBetween.magnitude <= this.SightDistance &&
                    Vector3.Angle(this.transform.forward, vectorBetween) <= this.FieldOfVision)
                {
                    RaycastHit hit;
                    Ray ray = new Ray(eyesPosition, vectorBetween);
                    Debug.DrawRay(eyesPosition, vectorBetween, Color.red);

                    // TODO: in the future the raycast should ignore some layers
                    if (Physics.Raycast(ray, out hit, this.SightDistance) &&
                        hit.collider.tag == Tags.Player)
                    {
                        PlayerOnSight = true;
                        return;
                    }
                }
            }

            PlayerOnSight = false;
        }

        public bool PlayerOnSight
        {
            get
            {
                return playerOnsight;
            }
            set
            {
                bool valueHasChanged = playerOnsight != value;
                playerOnsight = value;

                if (valueHasChanged)
                {
                    tree.Reevaluate();
                }
            }
        }

        public float SightDistance { get { return sightDistance; } }
        public float FieldOfVision { get { return fieldOfVision; } }
        public float EyesRelativePosition { get { return eyesRelativePosition; } }
        public Vector3 EyesPosition { get { return this.transform.position + Vector3.up * eyesRelativePosition; } }
    }
}
