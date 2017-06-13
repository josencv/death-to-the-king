using System;

namespace Assets.Code.Components.AI.Behaviour.Nodes
{
    public class ConditionNode : BehaviourNode
    {
        private Field field;

        public ConditionNode(BtContext context) : base(context)
        {
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
