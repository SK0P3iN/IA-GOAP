using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.GrupoCJJ.Scripts.AI.GOAP;
using Assets.EOTS;
using UnityEngine;

using Soldier = Assets.GrupoCJJ.Scripts.GameData.Soldiers.Soldier;
using Assets.GrupoCJJ.Scripts.GameData.Actions;
using Assets.General_Scripts;

namespace Assets.GrupoCJJ
{
    public class TeamManager : MonoBehaviour
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

        /*#############################
          ###   CONTROLA PONTE      ###
          #############################*/

        public bool IsBase1Nossa(Soldier aSoldier)
        {
            if ( FindObjectsOfType<Base>().
                 Where(b => (b.name.Equals("SE") && aSoldier.MyTeam == Teams.RedTeam && b.MyTeam==Teams.RedTeam)
                 || (b.name.Equals("NE") && aSoldier.MyTeam == Teams.BlueTeam && b.MyTeam == Teams.BlueTeam)).Count() > 0 )
            {
                return true;
            }
            else
                return false;
        }

        public bool Base1HasEnemys(Soldier aSoldier)
        {
            if (FindObjectsOfType<Base>().
                 Where(b => (b.name.Equals("SE") && aSoldier.MyTeam == Teams.RedTeam && b.MemberOfTeamBlue.Count > b.MemberOfTeamRed.Count)
                 || (b.name.Equals("NE") && aSoldier.MyTeam == Teams.BlueTeam && b.MemberOfTeamRed.Count > b.MemberOfTeamBlue.Count)).Count() > 0)
            {
                return true;
            }
            else
                return false;
        }
        public void DecidePonte(Soldier aSoldier)
        {
            if(Base1HasEnemys(aSoldier)) //base 1 tem algum enimigo?
            {
                if (IsBase1Nossa(aSoldier))//se a base ainda for nossa, esperam na ponte
                {
                    VoltaPonte(aSoldier);
                }

                else //se não ajuda a recuperar
                {
                    RecuperaBase1(aSoldier);
                }
            }
            else //se nao tem inimigos, ataca a base 4
            {
                AtacarBase4(aSoldier);
            }
        }

        public void RecuperaBase1(Soldier aSoldier)
        {
            aSoldier.GetComponent<CaptureFourthBase>().Cost = 1;
            aSoldier.GetComponent<CaptureBase>().Cost = -99;
            aSoldier.GetComponent<BlockBridge>().Cost = 1;

            RequestNewPlan(aSoldier.GetComponent<GoapAgent>());
        }
        public void VoltaPonte(Soldier aSoldier)
        {
            aSoldier.GetComponent<CaptureBase>().Cost = 1;
            aSoldier.GetComponent<CaptureFourthBase>().Cost = 1;
            aSoldier.GetComponent<BlockBridge>().Cost = -99;
            RequestNewPlan(aSoldier.GetComponent<GoapAgent>());
        }
        public void AtacarBase4(Soldier aSoldier)
        {
            aSoldier.GetComponent<CaptureFourthBase>().Cost = -99;
            aSoldier.GetComponent<CaptureBase>().Cost = 1;
            aSoldier.GetComponent<BlockBridge>().Cost = 1;
            RequestNewPlan(aSoldier.GetComponent<GoapAgent>());
        }


        /*#############################
          ###   CONTROLA BASE2/3    ###
          #############################*/

        public bool IsBase2Nossa(Soldier aSoldier)
        {
            if (FindObjectsOfType<Base>().
                 Where(b => (b.name.Equals("SW") && aSoldier.MyTeam == Teams.RedTeam && b.MyTeam != Teams.BlueTeam)
                 || (b.name.Equals("NW") && aSoldier.MyTeam == Teams.BlueTeam && b.MyTeam != Teams.RedTeam)).Count() > 0)
            {
                return true;
            }
            else
                return false;
        }
        public void DecideBase2(Soldier aSoldier)
        {
            if (IsBase2Nossa(aSoldier))
            {
                AtacarBase3(aSoldier);
            }
            else
            {
                AtacarBase2(aSoldier);
            }
        }
        private void AtacarBase3(Soldier soldier)
        {
                if (soldier.MyTransform.GetComponent<CaptureThirdBase>())
                {
                    soldier.GetComponent<CaptureSecondBase>().Cost = 1;
                    soldier.GetComponent<CaptureThirdBase>().Cost = -99;
                    RequestNewPlan(soldier.GetComponent<GoapAgent>());
                }
        }
        private void AtacarBase2(Soldier soldier)
        {
                if (soldier.MyTransform.GetComponent<CaptureThirdBase>())
                {
                    soldier.GetComponent<CaptureSecondBase>().Cost = -99;
                    soldier.GetComponent<CaptureThirdBase>().Cost = 1;
                    RequestNewPlan(soldier.GetComponent<GoapAgent>());
                }

        }
        private void Start()
        {
            StartCoroutine(ManageBases());
        }

        public void DoWaitFlag(Soldier aSoldier, bool wait)
        {
            if(wait)
            {
                print("TM: ESPERA PELA BANDEIRA");
                aSoldier.GetComponent<WaitForFlag>().Cost = -99;
                RequestNewPlan(aSoldier.GetComponent<GoapAgent>());
            }
            else
            {
                aSoldier.GetComponent<WaitForFlag>().Cost = 1;
                RequestNewPlan(aSoldier.GetComponent<GoapAgent>());
            }
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
