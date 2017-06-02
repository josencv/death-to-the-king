using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code.Components.AI
{
    class WanderRoutine : Routine
    {
        private IComponent entity;
        private WalkRoutine walkRoutine;
        private Vector3 destination;
        private Transform transform;
        private float wanderRadius;

        /// <summary>
        /// Initiales a Wander routine
        /// </summary>
        /// <param name="wanderRadius">The max possible radius to wander to from current position.</param>
        public WanderRoutine(IComponent entity, float wanderRadius = 1) : base(entity)
        {
            this.entity = entity;
            this.wanderRadius = wanderRadius;
            transform = entity.GetComponent<Transform>();
            destination = GenerateDestination(wanderRadius);
            walkRoutine = new WalkRoutine(entity, destination);
        }

        public override void Start()
        {
            Debug.logger.Log("Wander routine: Start");
            walkRoutine.Start();
            base.Start();
        }

        public override void Reset()
        {
            Debug.logger.Log("Wander routine: Reset");
            destination = GenerateDestination(wanderRadius);
            walkRoutine = new WalkRoutine(entity, destination);
            this.Start();
        }

        public override void Act()
        {
            Debug.logger.Log("Wander routine: Act");
            walkRoutine.Act();
            base.Act();

            if (walkRoutine.HasFailed)
            {
                Debug.logger.Log("Wander routine: Failed");
                this.Fail();
            }
            else if (walkRoutine.HasSucceeded)
            {
                Debug.logger.Log("Wander routine: Succeeded");
                this.Succeed();
            }
            else
            {
                walkRoutine.Act();
            }
        }

        private Vector3 GenerateDestination(float radius)
        {
            return new Vector3(
                transform.position.x + Random.Range(-radius, radius),
                transform.position.y,
                transform.position.z + Random.Range(-radius, radius));
        }
    }
}
