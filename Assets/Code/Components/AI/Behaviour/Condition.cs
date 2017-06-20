using System;

namespace Assets.Code.Components.AI.Behaviour
{
    public class Condition
    {
        public FieldType ConditionType { get; set; }
        public Operator ConditionOperator { get; set; }
        public string FieldName { get; set; }
        public float OperationValue { get; set; }

        /// <summary>
        /// Initializes an instance of the TransitionCondition class.
        /// Will throw error if an invalid condition is constructed (e.g. a boolean type condition with a 'greater than' operator is NOT valid)
        /// </summary>
        /// <param name="conditionType">The type of the condition</param>
        /// <param name="conditionOperator">The operator of the condition.</param>
        /// <param name="fieldName">The name of the state machine field to use in the condition operation</param>
        /// <param name="operationValue">The value of the right operand of the condition</param>
        public Condition(FieldType conditionType, Operator conditionOperator, string fieldName, float operationValue)
        {
            ConditionType = conditionType;
            ConditionOperator = conditionOperator;
            FieldName = fieldName;
            OperationValue = operationValue;

            CheckOperationCorrectness();
        }

        /// <summary>
        /// Check if the TransitionCondition is met
        /// </summary>
        /// <param name="value">The value to compare (left operand of the operation for '')</param>
        /// <returns>True if the condition is met. False otherwise</returns>
        public bool IsConditionMet(float value)
        {
            bool conditionMet = false;

            switch (ConditionType)
            {
                case FieldType.Float:
                    conditionMet = IsFloatConditionMet(value);
                    break;
                case FieldType.Int:
                    conditionMet = IsIntConditionMet((int)value);
                    break;
                case FieldType.Bool:
                    conditionMet = ((int)value == (int)ConditionOperator);
                    break;
                case FieldType.Trigger:
                    conditionMet = ((int)value == 1);
                    break;
            }

            return conditionMet;
        }

        /// <summary>
        /// Check if the condition is met for a 'float' ConditionType
        /// </summary>
        /// <param name="value">The value to compare (left operand of the operation)</param>
        /// <returns>True if the condition is met. False otherwise</returns>
        private bool IsFloatConditionMet(float value)
        {
            bool conditionMet = false;
            float rightOperand = OperationValue;
            if (ConditionOperator == Operator.Greater)
            {
                conditionMet = (value > rightOperand);
            }
            else if (ConditionOperator == Operator.Less)
            {
                conditionMet = (value < rightOperand);
            }

            return conditionMet;
        }

        /// <summary>
        /// Check if the condition is met for an 'int' ConditionType
        /// </summary>
        /// <param name="value">The value to compare (left operand of the operation)</param>
        /// <returns>True if the condition is met. False otherwise</returns>
        private bool IsIntConditionMet(int value)
        {
            bool conditionMet = false;
            int rightOperand = (int)OperationValue;

            if (ConditionOperator == Operator.Greater)
            {
                conditionMet = (value > rightOperand);
            }
            else if (ConditionOperator == Operator.Less)
            {
                conditionMet = (value < rightOperand);
            }
            else if (ConditionOperator == Operator.Equal)
            {
                conditionMet = (value == rightOperand);
            }
            else
            {
                conditionMet = (value != rightOperand);
            }

            return conditionMet;
        }

        /// <summary>
        /// Checks if the TransitionCondition has been constructed correctly. For example, a ConditionType 'float' can not have
        /// a 'False' ConditionOperator because they are not strictly comparable
        /// </summary>
        private void CheckOperationCorrectness()
        {
            ArgumentException exception = null;
            switch (ConditionType)
            {
                case FieldType.Float:
                    if (ConditionOperator == Operator.Equal ||
                        ConditionOperator == Operator.NotEqual ||
                        ConditionOperator == Operator.True ||
                        ConditionOperator == Operator.False)
                    {
                        exception = new ArgumentException("Invalid condition operator for float type (Hint: only 'Greater' and 'Less'  valid)");
                    }
                    break;
                case FieldType.Int:
                    if (ConditionOperator == Operator.True ||
                        ConditionOperator == Operator.False)
                    {
                        exception = new ArgumentException("Invalid condition operator for int type (Hint: only 'Greater' and 'Less', 'Equal' and 'NotEqual' are valid)");
                    }
                    break;
                case FieldType.Bool:
                    if (ConditionOperator == Operator.Equal ||
                        ConditionOperator == Operator.NotEqual ||
                        ConditionOperator == Operator.Greater ||
                        ConditionOperator == Operator.Less)
                    {
                        exception = new ArgumentException("Invalid condition operator for bool type (Hint: only 'True' and 'False' are valid)");
                    }
                    break;
                case FieldType.Trigger:
                    if (ConditionOperator != Operator.None)
                    {
                        exception = new ArgumentException("Invalid condition operator for trigger type (only 'None' is valid)");
                    }
                    break;
            }

            if (exception != null)
            {
                throw exception;
            }
        }
    }
}
