using Assets.Code.Components.AI.Behaviour.Nodes;
using Assets.Code.Components.Containers;
using Zenject;

namespace Assets.Code.Components.AI.Behaviour
{
    public class BehaviourTree
    {
        private BehaviourNode root;
        private DiContainer container;
        private BtContext context;

        public BehaviourTree(AIController ai)
        {
            context = new BtContext(ai);
            RegisterStateFields();
        }

        [Inject]
        public void Inject(DiContainer container)
        {
            this.container = container;
        }

        public void Initialize()
        {
            // TODO: this should be built from a file... probably
            var playerInSight = new ConditionNode(context, new Condition(FieldType.Bool, Operator.True, RegisteredFieldNames.PlayerInSight, 1.0f));
            var approach = new ApproachNode(context);
            var attack = new SequenceNode(context, new BehaviourNode[] { playerInSight, approach });
            var wander = new WanderNode(context);
            var selector = new SelectorNode(context, new BehaviourNode[] { attack, wander });
            var repeat = new RepeatNode(context, selector);

            root = repeat;
        }

        private void RegisterStateFields()
        {
            context.StateFields.Add(RegisteredFieldNames.PlayerInSight, new Field(FieldType.Bool, 0));
        }

        public void Start()
        {
            root.Start();
        }

        public void Update()
        {
            root.Act();
        }

        public void Reevaluate()
        {
            root.Restart();
        }

        /// <summary>
        /// Reevaluates the tree only if the passed criteria is true
        /// </summary>
        /// <param name="criteria">The criteria to check</param>
        private void ReevaluateIf(bool criteria)
        {
            if (criteria)
            {
                root.Restart();
            }
        }

        /// <summary>
        /// Sets the hostile target to the behaviour
        /// </summary>
        /// <param name="character"></param>
        public void SetTarget(Character character)
        {
            context.Target = character;
        }

        /// <summary>
        /// Registers a boolean field to the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="startingValue">The value of the field when the behaviour tree starts</param>
        public void RegisterBoolField(string fieldName, bool startingValue = false)
        {
            float fvalue = startingValue ? 1 : 0;
            Field field = new Field(FieldType.Bool, fvalue);
            context.StateFields.Add(fieldName, field);
        }

        /// <summary>
        /// Registers an integer field to the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="startingValue">The value of the field when the behaviour tree starts</param>
        public void RegisterIntField(string fieldName, int startingValue = 0)
        {
            Field field = new Field(FieldType.Int, startingValue);
            context.StateFields.Add(fieldName, field);
        }

        /// <summary>
        /// Registers a float field to the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="startingValue">The value of the field when the behaviour tree starts</param>
        public void RegisterFloatField(string fieldName, float startingValue = 0)
        {
            Field field = new Field(FieldType.Float, startingValue);
            context.StateFields.Add(fieldName, field);
        }

        /// <summary>
        /// Registers a trigger field to the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="startingValue">The value of the field when the behaviour tree starts</param>
        public void RegisterTriggerField(string fieldName)
        {
            Field field = new Field(FieldType.Trigger, 0);
            context.StateFields.Add(fieldName, field);
        }

        /// <summary>
        /// Changes the value of a previously registered boolean field of the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="value">The new value to set to the field</param>
        public void SetBoolField(string fieldName, bool value)
        {
            var convertedValue = value ? 1 : 0;
            var valueHasChanged = context.StateFields[fieldName].Value != convertedValue;
            context.StateFields[fieldName].Value = convertedValue;
            ReevaluateIf(valueHasChanged);
        }

        /// <summary>
        /// Changes the value of a previously registered integer field of the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="value">The new value to set to the field</param>
        public void SetIntField(string fieldName, int value)
        {
            var valueHasChanged = context.StateFields[fieldName].Value != value;
            context.StateFields[fieldName].Value = value;
            ReevaluateIf(valueHasChanged);
        }

        /// <summary>
        /// Changes the value of a previously registered float field of the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="value">The new value to set to the field</param>
        public void SetFloatField(string fieldName, float value)
        {
            var valueHasChanged = context.StateFields[fieldName].Value != value;
            context.StateFields[fieldName].Value = value;
            ReevaluateIf(valueHasChanged);
        }

        /// <summary>
        /// Activates a previously registered trigger field of the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="value">The new value to set to the field</param>
        public void SetTrigger(string fieldName)
        {
            context.StateFields[fieldName].Value = 1.0f;
            Reevaluate();
        }
    }
}
