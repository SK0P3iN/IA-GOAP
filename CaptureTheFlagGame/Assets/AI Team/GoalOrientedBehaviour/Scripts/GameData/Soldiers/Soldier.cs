using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.AI_Team.GoalOrientedBehaviour.Scripts.AI.GOAP;
using Assets.EOTS;
using Assets.General_Scripts;
using Assets.Scripts.Pathfinding;
using Assets.Scripts.Pathfinding.Scripts.AStar;
using Assets.Scripts.SteeringBehaviours.Basics;
using UnityEngine;

namespace Assets.AI_Team.GoalOrientedBehaviour.Scripts.GameData.Soldiers
{
    public class Soldier : MonoBehaviour, IGoap, ISoldier
    {
        public bool DroppedFlag;
        public bool HasFlag { get; set; }

        public Transform MyTransform { get; set; }

        private DemoIaTeamManager _myTM;
        private PathfindingUnit _pathfinding;

        public Teams MyTeam
        {
            get => _myTeam;
            set => _myTeam = value;
        }

        public bool Invulnerable
        {
            get => _invulnerable;
            set => _invulnerable = value;
        }


        private GoapAgent _agent;
        private Respawner _myRespawner;
        private IPathfindingUnit _pathfindingUnit;
        [SerializeField] private Teams _myTeam;
        [SerializeField] private bool _invulnerable;
        private SteeringBasics _mtSB;


        private void Awake()
        {
            _pathfinding = GetComponent<PathfindingUnit>();
            _myTM = FindObjectOfType<DemoIaTeamManager>();
            _pathfindingUnit = GetComponent<IPathfindingUnit>();
            _agent = GetComponent<GoapAgent>();
            _myRespawner = FindObjectsOfType<Respawner>().First(sp => sp.MyTeam == MyTeam);
            MyTransform = transform;
            _mtSB = GetComponent<SteeringBasics>();
        }



        /// <summary>
        /// Key-Value data that will feed the GOAP actions and system while planning.
        /// </summary>
        /// <returns></returns>
        public HashSet<KeyValuePair<string, object>> GetWorldState()
        {
            var worldData = new HashSet<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("hasFlag", HasFlag),
                new KeyValuePair<string, object>("scored", DroppedFlag),
            };

            return worldData;
        }


        /// <summary>
        /// Our only goal will ever be to drop flags.
        /// The ScoreFlag action will be able to fulfill this goal.
        /// </summary>
        /// <returns></returns>
        public HashSet<KeyValuePair<string, object>> CreateGoalState()
        {
            var goal = new HashSet<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("conquerBase", true),
            };

            return goal;
        }

        /// <summary>
        /// Plan failed. Add cleanup code if necessary
        /// </summary>
        /// <param name="failedGoal"></param>
        public void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal)
        {
            // Not handling this here since we are making sure our goals will always succeed.
            // But normally you want to make sure the world state has changed before running
            // the same goal again, or else it will just fail.
        }

        /// <summary>
        /// Plan found.
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="actions"></param>
        public void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<IGoapAction> actions)
        {
            // Yay we found a plan for our goal
            //Debug.Log("<color=green>Plan found</color> " + GoapAgent.PrettyPrint(actions));

        }

        /// <summary>
        /// Everything is done, we completed our actions for this gool. Hooray! Add code if necessary
        /// </summary>
        public void ActionsFinished()
        {
        }
         /// <summary>
         /// Current action is finished. We are no longer using the workstation.
         /// </summary>
         /// <param name="currentAction"></param>
        public void CurrentActionFinished(IGoapAction currentAction)
        {
        }

        /// <summary>
        /// Clean up for the current action and setup for the next action.
        /// </summary>
        /// <param name="currentAction"></param>
        /// <param name="nextAction"></param>
        public void CurrentActionFinished(IGoapAction currentAction, IGoapAction nextAction)
        {
            CurrentActionFinished(currentAction);
            // if we have more actions, verify that it is still possible to perform the plan. Abort if there is no longer an available target.
            if (nextAction != null && nextAction.CheckProceduralPrecondition(gameObject) == false)
                PlanAborted(nextAction);
        }

        /// <summary>
        /// An action bailed out of the plan. State has been reset to plan again.
        /// Take note of what happened and make sure if you run the same goal again
        /// that it can succeed.
        /// </summary>
        /// <param name="aborter"></param>
        public void PlanAborted(IGoapAction aborter)
        {
            _agent.AbortPlan();
        }

        /// <summary>
        /// Moves the agent to the target of the next action. This implementation uses AStart pathfinding.
        /// </summary>
        /// <param name="nextAction"></param>
        /// <returns>Returns true if we are in range of the target</returns>
        public bool MoveAgent(IGoapAction nextAction)
        {
            if (nextAction.Target == null)
                return false; // we are not there yet
            
            _pathfindingUnit.SetTarget(nextAction.Target.transform);

            if (_pathfindingUnit.DoFollowPathStep() == false) // if we are not following the path anymore
            {
                // we are at the target location, we are done
                nextAction.InRange = true;


                return true; // we have arrived
            }

            return false; // we are not there yet
        }

        public void Died()
        {
            if(HasFlag)
                GetComponentInChildren<FlagComponent>().Drop();

            transform.position = _myRespawner.transform.position;

            StartCoroutine(CantMove(5f));
        }

        private IEnumerator CantMove(float seconds)
        {
            while (seconds > 0)
            {
                _mtSB.Stop();
                seconds -= Time.deltaTime;
                yield return null;
            }
            _myTM.RequestNewPlan(GetComponent<GoapAgent>());
        }
    }
}

