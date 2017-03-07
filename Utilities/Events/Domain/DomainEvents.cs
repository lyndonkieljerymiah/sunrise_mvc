using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Events.Abstract;

namespace Utilities.Events.Domain
{
    public static class DomainEvents
    {
        [ThreadStatic] //each has it's own thread
        private static List<Delegate> actions;
        
        /// <summary>
        /// TODO: Registers a callback for the given domain event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        public static void Register<T>(Action<T> callback) 
            where T : IDomainEvent
        {
            if (actions == null)
                actions = new List<Delegate>();

            actions.Add(callback);
        }

        /// <summary>
        /// TODO : Clears callbacks passed to Register on the current thread
        /// </summary>
        public static void ClearCallbacks()
        {
            actions = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        public static void Raise<T>(T args) 
            where T : IDomainEvent
        {
            if(actions != null)
            {
                foreach (var action in actions)
                {
                    ((Action<T>)action)(args);
                }
            }
        }
    }
}
