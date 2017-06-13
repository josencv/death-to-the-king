using System;

namespace Assets.Code.Components.AI.Routines
{
    class SelectorRoutine : Routine
    {
        private Routine[] routines;
        private int index;  // The current routine index

        public SelectorRoutine(AIController ai, Routine[] routines) : base(ai)
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
            routines[index].Start();
        }

        public override void Act()
        {
            base.Act();
            routines[index].Act();

            if (routines[index].HasSucceeded)
            {
                this.Succeed();
            }
            else if (routines[index].HasFailed)
            {
                index++;
                if (index >= routines.Length)
                {
                    this.Fail();
                }
                else
                {
                    routines[index].Start();
                }
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
