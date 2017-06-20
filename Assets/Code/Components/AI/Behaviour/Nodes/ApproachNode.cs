using Assets.Code.Components.Weapon;
using UnityEngine;

namespace Assets.Code.Components.AI.Behaviour.Nodes
{
    public class ApproachNode : BehaviourNode
    {
        private WalkNode walk;
        private BtContext context;
        private float distanceThreshold;    /// The minimum distance between the entity and the target to consider successful the approach action

        public ApproachNode(BtContext context) : base(context)
        {
            this.context = context;
        }

        public override void Start()
        {
            walk = new WalkNode(context, context.Target.transform.position, context.AI.GetComponent<IWeapon>().AttackRange);
            walk.Start();
            base.Start();
        }

        public override void Act()
        {
            base.Act();
            if (walk.HasSucceeded)
            {
                this.Succeed();
            }
            else if (walk.HasFailed)
            {
                this.Fail();
            }
            else
            {
                float thresholdDistance = context.AI.GetComponent<IWeapon>().AttackRange + 0.4f; // TODO: Change arbitrary float value to the actual closest point to the collider boundary or something similar.
                walk.UpdateDestination(context.Target.transform.position, thresholdDistance);
                walk.Act();
            }
        }

        public override void Stop()
        {
            if (walk != null)
            {
                walk.Stop();
            }

            base.Stop();
        }

        public override void Reset() { }
    }
}
