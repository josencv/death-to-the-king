using Assets.Code.Components.Containers;
using System.Collections.Generic;

namespace Assets.Code.Components.AI.Behaviour
{
    public class BtContext
    {
        private readonly AIController ai;
        private readonly IDictionary<string, Field> stateFields;
        public Character Target { get; set; }

        public BtContext(AIController ai)
        {
            this.ai = ai;
            stateFields = new Dictionary<string, Field>();
        }

        public AIController AI { get { return ai; } }
        public IDictionary<string, Field> StateFields { get { return stateFields; } }
    }
}
