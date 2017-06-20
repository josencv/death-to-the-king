using System;

namespace Assets.Code.Components.AI.Behaviour.Nodes
{
    public class ConditionNode : BehaviourNode
    {
        private Condition condition;
        private Field fieldToWatch;

        public ConditionNode(BehaviourTreeContext context, Condition condition) : base(context)
        {
            this.condition = condition;
            this.fieldToWatch = context.StateFields[condition.FieldName];
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Reset() { }

        public override void Act()
        {
            base.Act();

            if (condition.IsConditionMet(fieldToWatch.Value))
            {
                this.Succeed();
            }
            else
            {
                this.Fail();
            }
        }
    }
}
