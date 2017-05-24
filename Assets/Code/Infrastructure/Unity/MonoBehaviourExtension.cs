using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Infrastructure.Unity
{
    /// <summary>
    /// This class is used to extend the MonoBehavior functionality
    /// </summary>
    public class MonoBehaviourExtension : MonoBehaviour
    {
        /// <summary>
        /// Used to store action to be executed in FixedUpdate insted of the Update loop.
        /// </summary>
        protected Dictionary<string, Action> actions;

        public MonoBehaviourExtension()
        {
            actions = new Dictionary<string, Action>();
        }

        protected virtual void ExecuteActions()
        {
            List<string> keysToRemove = new List<string>();
            foreach (var actionPair in actions)
            {
                actionPair.Value();
                keysToRemove.Add(actionPair.Key);
            }

            foreach (var key in keysToRemove)
            {
                actions.Remove(key);
            }
        }
    }
}