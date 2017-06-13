using Assets.Code.Components.AI.Behaviour.Nodes;
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
        }

        [Inject]
        public void Inject(DiContainer container)
        {
            this.container = container;
        }

        public void Initialize()
        {
            // TODO: this should be built from a file... probably
            var wander = new WanderNode(context);
            var selector = new SelectorNode(context, new BehaviourNode[] { wander });
            var repeat = new RepeatNode(context, selector);
            root = repeat;
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
        /// Registers a boolean field to the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="startingValue">The value of the field when the behaviour tree starts</param>
        public void RegisterBoolField(string fieldName, bool startingValue = false)
        {
            float fvalue = startingValue ? 1 : 0;
            Field field = new Field(FieldType.Bool, fvalue);
            context.Fields.Add(fieldName, field);
        }

        /// <summary>
        /// Registers an integer field to the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="startingValue">The value of the field when the behaviour tree starts</param>
        public void RegisterIntField(string fieldName, int startingValue = 0)
        {
            Field field = new Field(FieldType.Int, startingValue);
            context.Fields.Add(fieldName, field);
        }

        /// <summary>
        /// Registers a float field to the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="startingValue">The value of the field when the behaviour tree starts</param>
        public void RegisterFloatField(string fieldName, float startingValue = 0)
        {
            Field field = new Field(FieldType.Float, startingValue);
            context.Fields.Add(fieldName, field);
        }

        /// <summary>
        /// Registers a trigger field to the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="startingValue">The value of the field when the behaviour tree starts</param>
        public void RegisterTriggerField(string fieldName)
        {
            Field field = new Field(FieldType.Trigger, 0);
            context.Fields.Add(fieldName, field);
        }

        /// <summary>
        /// Changes the value of a previously registered boolean field of the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="value">The new value to set to the field</param>
        public void SetBoolField(string fieldName, bool value)
        {
            context.Fields[fieldName].Value = value ? 1 : 0;
        }

        /// <summary>
        /// Changes the value of a previously registered integer field of the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="value">The new value to set to the field</param>
        public void SetIntField(string fieldName, int value)
        {
            context.Fields[fieldName].Value = value;
        }

        /// <summary>
        /// Changes the value of a previously registered float field of the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="value">The new value to set to the field</param>
        public void SetFloatField(string fieldName, float value)
        {
            context.Fields[fieldName].Value = value;
        }

        /// <summary>
        /// Activates a previously registered trigger field of the behaviour tree
        /// </summary>
        /// <param name="fieldName">The name of the field to register</param>
        /// <param name="value">The new value to set to the field</param>
        public void SetTrigger(string fieldName)
        {
            context.Fields[fieldName].Value = 1.0f;
        }
    }
}
