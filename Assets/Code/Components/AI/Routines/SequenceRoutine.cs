using System;

namespace Assets.Code.Components.AI.Routines
{
    class SequenceRoutine : Routine
    {
        private Routine[] routines;
        private int index;  // The current routine index

        public SequenceRoutine(IComponent entity, Routine[] routines) : base(entity)
        {
            if (routines == null || routines.Length == 0)
            {
                throw new ArgumentException("cannot be null or empty", "routines");
            }

            this.routines = routines;
            index = 0;
        }

        public override void Start()
        {
            base.Start();
            routines[0].Start();
        }

        public override void Act()
        {
            base.Act();
            routines[index].Act();

            if (routines[index].HasSucceeded)
            {
                index++;
                if (index >= routines.Length)
                {
                    this.Succeed();
                }
                else
                {
                    routines[index].Start();
                }
            }
            else if (routines[index].HasFailed)
            {
                this.Fail();
            }
        }

        public override void Reset()
        {
            index = 0;
            foreach (Routine routine in routines)
            {
                routine.Reset();
            }
        }
    }
}
