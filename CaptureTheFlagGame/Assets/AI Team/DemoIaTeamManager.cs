using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.AI_Team.GoalOrientedBehaviour.Scripts.AI.GOAP;
using Assets.EOTS;
using Assets.General_Scripts;
using UnityEngine;
using Soldier = Assets.AI_Team.GoalOrientedBehaviour.Scripts.GameData.Soldiers.Soldier;

namespace Assets.AI_Team
{
    public class DemoIaTeamManager : MonoBehaviour
    {
        public List<Soldier> Army;
        public Dictionary<Base, bool> AllBases;
        public FlagComponent Flag;
        public Teams MyTeam;
        public Teams OtherTeam;

        public List<ISoldier> Enemies;


        private void Awake()
        {

            Flag = FindObjectOfType<FlagComponent>();
            Army = FindObjectsOfType<Soldier>().ToList();
            AllBases = new Dictionary<Base, bool>();
            var allB = FindObjectsOfType<Base>().ToList();

            foreach (var @base in allB)
            {
                AllBases.Add(@base, false);
            }

            OtherTeam = MyTeam == Teams.RedTeam ? Teams.BlueTeam : Teams.RedTeam;
            Enemies = FindObjectsOfType<MonoBehaviour>().OfType<ISoldier>().Where(s => s.MyTeam == OtherTeam).ToList();
        }


        public void RequestNewPlan(GoapAgent agent)
        {
            agent.AbortPlan();
        }

        public void ResetTeamPlan()
        {
            StartCoroutine(ResetPlans());
        }

        private IEnumerator ResetPlans()
        {
            yield return null;

            foreach (var soldier in Army)
            {
                RequestNewPlan(soldier.GetComponent<GoapAgent>());
            }
        }

        private void Start()
        {
            StartCoroutine(ManageBases());
            //StartCoroutine(DoSomeHunting());
        }

        private IEnumerator DoSomeHunting()
        {
            throw new NotImplementedException();
        }

        private IEnumerator ManageBases()
        {
            while (true)
            {
                StartCoroutine(ResetPlans());

                yield return new WaitForSeconds(2f);
            }
        }
    }
}