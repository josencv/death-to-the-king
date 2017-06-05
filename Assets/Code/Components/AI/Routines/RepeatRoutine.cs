using UnityEngine;

namespace Assets.Code.Components.AI.Routines
{
    class RepeatRoutine : Routine
    {
        private Routine routine;
        private int times;
        private int counter;

        /// <summary>
        /// Initializes the repeat routine. 
        /// </summary>
        /// <param name="routine">The routine to repeat</param>
        /// <param name="times">How many times should the routine be repeated. If not set irt will wun indefinitely until it fails.</param>
        public RepeatRoutine(IComponent entity, Routine routine, int times = -1) : base(entity)
        {
            this.routine = routine;
            this.times = times;
            this.counter = 0;
        }

        public override void Start()
        {
            counter++;
            base.Start();
            this.routine.Start();
        }

        public override void Reset()
        {
            counter = 0;
        }

        public override void Act()
        {
            base.Act();

            if (routine.IsStopped)
            {
                routine.Start();
            }
            else if (routine.HasFailed)
            {
                this.Fail();
            }
            else if (routine.HasSucceeded)
            {
                if (times == -1 || counter < times)
                {
                    counter++;
                    routine.Restart();
                }
                else
                {
                    this.Succeed();
                }
            }
            else
            {
                routine.Act();
            }
        }
    }
}
