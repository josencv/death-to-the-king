using Assets.Code.Components.Containers;
using System.Collections.Generic;

namespace Assets.Code.Components.AI.Behaviour
{
    /// <summary>
    /// This class stores information that gives context to the running BT, allowing it to execute complex behaviours
    /// </summary>
    public class BehaviourTreeContext
    {
        private readonly AIController ai;
        private readonly IDictionary<string, Field> stateFields;
        public Character Target { get; set; }

        public BehaviourTreeContext(AIController ai)
        {
            this.ai = ai;
            stateFields = new Dictionary<string, Field>();
        }

        public AIController AI { get { return ai; } }
        public IDictionary<string, Field> StateFields { get { return stateFields; } }
    }
}
