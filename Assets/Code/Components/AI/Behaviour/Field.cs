namespace Assets.Code.Components.AI.Behaviour
{
    /// <summary>
    /// A field of a behaviour tree. These fields are used to add context to the behaviour routines.
    /// Can be a float, int, bool or trigger field.
    /// Important note: field value is always a float, regardless the field type, as any of those types
    /// can be represented with a float value (e.g. boolean value 'true' is 1.0f)
    /// </summary>
    public class Field
    {
        public FieldType Type { get; set; }
        public float Value { get; set; }

        /// <summary>
        /// Initializes an instance of the BehaviourTreeField class
        /// </summary>
        /// <param name="type">The type of the field</param>
        /// <param name="value">The starting value of the field</param>
        public Field(FieldType type, float value)
        {
            Type = type;
            Value = value;
        }
    }
}
