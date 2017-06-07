using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code.Components.AI.Routines
{
    class WanderRoutine : Routine
    {
        private SequenceRoutine sequenceRoutine;
        private WalkRoutine walkRoutine;
        private IdleRoutine idleRoutine;
        private Transform transform;
        private float wanderRadius;

        /// <summary>
        /// Initiales a Wander routine
        /// </summary>
        /// <param name="wanderRadius">The max possible radius to wander to from current position.</param>
        /// <param name="idleDuration">The iddle duration between movement in seconds.</param>
        public WanderRoutine(AIController ai, float wanderRadius = 1, float idleDuration = 4) : base(ai)
        {
            this.wanderRadius = wanderRadius;
            transform = ai.GetComponent<Transform>();
            walkRoutine = new WalkRoutine(ai, GenerateDestination(wanderRadius));
            idleRoutine = new IdleRoutine(ai, idleDuration);
            sequenceRoutine = new SequenceRoutine(ai, new Routine[] { idleRoutine, walkRoutine });
        }

        public override void Start()
        {
            sequenceRoutine.Start();
            base.Start();
        }

        public override void Reset()
        {
            sequenceRoutine.Reset();
            walkRoutine.Destination = GenerateDestination(wanderRadius);
        }

        public override void Act()
        {
            sequenceRoutine.Act();
            base.Act();

            if (sequenceRoutine.HasFailed)
            {
                this.Fail();
            }
            else if (sequenceRoutine.HasSucceeded)
            {
                this.Succeed();
            }
            else
            {
                sequenceRoutine.Act();
            }
        }

        private Vector3 GenerateDestination(float radius)
        {
            Vector2 randomOffset = Random.insideUnitCircle.normalized * wanderRadius;
            return new Vector3(
                transform.position.x + randomOffset.x,
                transform.position.y,
                transform.position.z + randomOffset.y);
        }
    }
}
