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
            foreach (var action in actions)
            {
                action.Value();
                actions.Remove(action.Key);
            }
        }
    }
}