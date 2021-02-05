using System.Collections.Generic;

namespace Assets.Scripts.HSM.Abstracts
{
    public interface IUpdateResult
    {
        /// <summary>
        /// List of actions resulting from this update
        /// </summary>
        IEnumerable<IAction> Actions { get; }

        /// <summary>
        /// Transitions exising in this update
        /// </summary>
        ITransition Transition { get; set; }

        /// <summary>
        /// Level of this update
        /// </summary>
        int Level { get; set; }

        /// <summary>
        /// Add a new action
        /// </summary>
        /// <param name="action"></param>
        void AddAction(IAction action);

        /// <summary>
        /// Add the received actions
        /// </summary>
        /// <param name="actions"></param>
        void AddAction(IEnumerable<IAction> actions);
    }
}