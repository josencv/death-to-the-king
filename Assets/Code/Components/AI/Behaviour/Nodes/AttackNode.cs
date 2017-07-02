using Assets.Code.Components.Weapon;

namespace Assets.Code.Components.AI.Behaviour.Nodes
{
    public class AttackNode : BehaviourNode
    {
        private BehaviourTreeContext context;
        private IWeapon weapon;

        public AttackNode(BehaviourTreeContext context) : base(context)
        {
            this.context = context;
        }

        public override void Start()
        {
            base.Start();
            weapon = context.AI.GetComponent<IWeapon>();
            weapon.AttackFinished += Succeed;
        }

        public override void Act()
        {
            base.Act();

            if (!weapon.IsAttacking)
            {
                weapon.Attack();
            }
        }

        public override void Stop()
        {
            base.Stop();
        }

        public override void Reset()
        {
        }
    }
}
