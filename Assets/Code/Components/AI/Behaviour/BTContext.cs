using System.Collections.Generic;

namespace Assets.Code.Components.AI.Behaviour
{
    public class BtContext
    {
        private readonly AIController ai;
        private readonly IDictionary<string, Field> fields;

        public BtContext(AIController ai)
        {
            this.ai = ai;
            fields = new Dictionary<string, Field>();
        }

        public AIController AI { get { return ai; } }
        public IDictionary<string, Field> Fields { get { return fields; } }
    }
}
