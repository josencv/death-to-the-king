using Assets.Code.Constants;
using Assets.Code.Shared;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Code.Components.AI.Routines
{
    class DetectRoutine : Routine
    {
        private WorldData worldData;
        private AIController ai;

        public DetectRoutine(AIController ai) : base(ai)
        {
            this.ai = ai;
        }

        [Inject]
        private void Inject(WorldData worldData)
        {
            this.worldData = worldData;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Act()
        {
            base.Act();

            // Check if any of the player is on sight
            foreach (var player in worldData.Players)
            {
                var eyesPosition = ai.EyesPosition;
                var vectorBetween = player.GetComponent<CapsuleCollider>().bounds.center - eyesPosition;
                if (vectorBetween.magnitude <= ai.SightDistance &&
                    Vector3.Angle(ai.transform.forward, vectorBetween) <= ai.FieldOfVision)
                {
                    RaycastHit hit;
                    Ray ray = new Ray(eyesPosition, vectorBetween);
                    Debug.DrawRay(eyesPosition, vectorBetween, Color.red);

                    // TODO: in the future the raycast should ignore some layers
                    if (Physics.Raycast(ray, out hit, ai.SightDistance) &&
                        hit.collider.tag == Tags.Player)
                    {
                        this.Succeed();
                        return;
                    }
                }
            }

            this.Fail();
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
