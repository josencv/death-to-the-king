using Assets.Code.Components.Movement;
using UnityEngine;

namespace Assets.Code.Components.AI.Routines
{
    class IdleRoutine : Routine
    {
        private IMovable movement;
        private float idleDuration;
        private float currentTime;

        public IdleRoutine(IComponent entity, float idleDuration) : base(entity)
        {
            movement = entity.GetComponent<IMovable>();
            this.idleDuration = idleDuration;
            currentTime = 0;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Act()
        {
            base.Act();
            currentTime += Time.deltaTime;

            if (currentTime >= idleDuration)
            {
                this.Succeed();
            }
        }

        public override void Reset()
        {
            currentTime = 0;
        }
    }
}
