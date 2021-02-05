using System.Collections.Generic;
using Assets.Scripts.HSM.Abstracts;
using Assets.Scripts.HSM.Concretes;
using Assets.Scripts.HSM.Concretes.Actions;
using Assets.Scripts.HSM.Concretes.Conditions;
using UnityEngine;

namespace Assets.Scripts.HSM
{
    public class AgentHSM : MonoBehaviour
    {
        public string CurrentStateName;

        HierarchicalStateMachine _fleer;
        HierarchicalStateMachine _chaser;


        private void Start()
        {
            InitializeHsm();
        }

        private void FixedUpdate()
        {
            UpdateHSM();
        }

        private void UpdateHSM()
        {

            foreach (var action in _fleer.Update().Actions)
                action.Execute();

            CurrentStateName = _fleer.CurState.Name;
        }

        private void InitializeHsm()
        {
            // start by creating the states

            // fleer
            var stateFlock = new State("Flock");
            var stateEvade = new State("Evade");
            var stateHide = new State("Hide");

            // chaser
            var stateWander = new State("Wander");
            var statePursue = new State("Pursue");

            _fleer = new HierarchicalStateMachine(stateFlock, "Fleer");
            _fleer.States = new HashSet<IState> { stateFlock, stateEvade, stateHide };


            //
            // FLOCK STATE
            //
            stateFlock.Parent = _fleer;

            // cache for performance
            var go = gameObject;
            // flock actions
            stateFlock.EntryActions = new HashSet<IAction> { new FlockEntryAction(go) };
            stateFlock.ActiveActions = new HashSet<IAction> { new EmptyAction() };
            stateFlock.ExitActions = new HashSet<IAction> { new FlockExitAction(go) };

            var trFlockToHide = new Transition(
                0,
                new HashSet<IAction> { new EmptyAction() },
                stateHide,
                "Flock to Hide",
                new List<ICondition>{new HidingSpotNearby(), new ChaserNearby()},
                go);
            
            var trFlockToEvade = new Transition(
                0,
                new HashSet<IAction> { new EmptyAction() },
                stateEvade,
                "Flock to Evade",
                new List<ICondition> { new NoHidingSpot(), new ChaserNearby() },
                go);

            // flock transitions (from flock to somewhere)
            stateFlock.Transitions = new HashSet<ITransition>{trFlockToHide, trFlockToEvade};


            //
            // HIDE STATE
            //

            stateHide.Parent = _fleer;

            // hide state actions
            // for hiding, we need to know from who we are hiding from, so the active action will be to find the closest one.
            stateHide.EntryActions = new HashSet<IAction> { new HideEntryAction(go) };
            stateHide.ActiveActions = new HashSet<IAction> { new HideActiveAction(go) };
            stateHide.ExitActions = new HashSet<IAction> { new HideExitAction(go) };


            var trHideToFlock = new Transition(
                0,
                new HashSet<IAction> { new EmptyAction() },
                stateFlock,
                "Hide to Flock",
                new List<ICondition> { new NoChaserNearby() },
                go);

            var trHideToEvade = new Transition(
                0,
                new HashSet<IAction> { new EmptyAction() },
                stateHide,
                "Hide to Evade",
                new List<ICondition> { new ChaserNearby(), new NoHidingSpot() },
                go);

            // hide state transitions
            stateHide.Transitions = new HashSet<ITransition>{trHideToEvade, trHideToFlock};




            //
            // EVADE STATE
            //

            stateEvade.Parent = _fleer;
            // for hiding, we need to know from how we are hiding from, so the active action will be to find the closest one.
            stateEvade.EntryActions = new HashSet<IAction> { new EvadeEntryAction(go) };
            stateEvade.ActiveActions = new HashSet<IAction> { new HideActiveAction(go) };
            stateEvade.ExitActions = new HashSet<IAction> { new EvadeExitActions(go) };


            var trEvadeToFlock = new Transition(
                0,
                new HashSet<IAction> { new EmptyAction() },
                stateFlock,
                "Evade to Flock",
                new List<ICondition> { new NoChaserNearby() },
                go);

            var trEvadeToHide = new Transition(
                0,
                new HashSet<IAction> { new EmptyAction() },
                stateHide,
                "Evade to Hide",
                new List<ICondition> { new ChaserNearby(), new HidingSpotNearby() },
                go);

            stateEvade.Transitions = new HashSet<ITransition>{trEvadeToHide, trEvadeToFlock};




            // evade state actions
            // for evading, we need to know from how we are hiding from, so the active action will be to find the closest one.

            _chaser = new HierarchicalStateMachine(stateWander, "Chaser");

        }
    }
}
