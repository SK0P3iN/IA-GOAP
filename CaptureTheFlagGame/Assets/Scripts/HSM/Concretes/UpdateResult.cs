using System.Collections.Generic;
using Assets.Scripts.HSM.Abstracts;

namespace Assets.Scripts.HSM.Concretes
{
    public class UpdateResult : IUpdateResult
    {
        /// <summary>
        /// List of actions resulting from this update
        /// </summary>
        private readonly HashSet<IAction> _actions;

        /// <summary>
        /// List of actions resulting from this update
        /// </summary>
        public IEnumerable<IAction> Actions
        {
            get { return _actions; }
        }

        /// <summary>
        /// Transitions exising in this update
        /// </summary>
        public ITransition Transition { get; set; }
        /// <summary>
        /// Level of this update
        /// </summary>
        public int Level { get; set; }

        public UpdateResult()
        {
            _actions = new HashSet<IAction>();
            Transition = null;
            Level = 0;
        }

        public UpdateResult(IEnumerable<IAction> actions)
        {
            _actions = new HashSet<IAction>(actions ?? new HashSet<IAction>());
            Transition = null;
            Level = 0;
        }

        /// <summary>
        /// Add a new action
        /// </summary>
        /// <param name="action"></param>
        public void AddAction(IAction action)
        {
            if (action != null)
                _actions.Add(action);
        }

        /// <summary>
        /// Add the received actions
        /// </summary>
        /// <param name="actions"></param>
        public void AddAction(IEnumerable<IAction> actions)
        {
            if (actions != null)
                _actions.UnionWith(actions);
        }
    }
}