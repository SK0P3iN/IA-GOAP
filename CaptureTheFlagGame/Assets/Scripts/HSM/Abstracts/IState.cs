using System.Collections.Generic;

namespace Assets.Scripts.HSM.Abstracts
{
    public interface IState
    {
        /// <summary>
        /// List of Actions to be execute while the state is active
        /// </summary>
        IEnumerable<IAction> ActiveActions { get; set; }
        /// <summary>
        /// List of Actions to be Executed when this state is entered
        /// </summary>
        IEnumerable<IAction> EntryActions { get; set; }

        /// <summary>
        /// List of Actions to be Executed when this state is exited
        /// </summary>
        IEnumerable<IAction> ExitActions { get; set; }

        /// <summary>
        /// List of all outgoing transitions of this state
        /// </summary>
        IEnumerable<ITransition> Transitions { get; set; }

        /// <summary>
        /// States that belongs to this state. Used to build a Hierarchical State Machine.
        /// If any state of a state machine has states, then it is an multi level state machine.
        /// </summary>
        IEnumerable<IState> States { get; set; }

        /// <summary>
        /// State that parents this state.
        /// </summary>
        IState Parent { get; set; }

        /// <summary>
        /// Name of this sate
        /// </summary>
        string Name { get; set; }

        string ToString();


        /// <summary>
        /// <para>Returns ActiveActions if implemented in a State.</para>
        /// <para>Recursively updates the sub state machine and returns all of its relevant actions 
        /// (may be active, transition, entry or exit actions)</para>
        /// </summary>
        IUpdateResult Update();

        /// <summary>
        /// Recurses up the parent hierarchy, transitioning into each state in turn for the given number of levels
        /// </summary>
        /// <returns></returns>
        IEnumerable<IAction> UpdateDown(IState state, int level);

    }
}