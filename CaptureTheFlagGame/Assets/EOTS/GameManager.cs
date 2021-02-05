using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.General_Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.EOTS
{
    public class GameManager : MonoBehaviour
    {
        public List<IGoap> BlueTeamSoldiers;
        public List<IGoap> RedTeamSoldiers;



        public int TeamBlueScore;
        public int TeamRedScore;

        private int _teamBlueControlledBases;
        private int _teamRedControlledBases;

        public Text TeamBlueScoreText;
        public Text TeamRedScoreText;
        public Text TeamBlueBasesText;
        public Text TeamRedBasesText;

        public Text MinTimer;
        public Text SecTimer;
        
        public int TotalGameTime = 600;


        private List<Base> _bases;

        public int TeamRedControlledBases
        {
            get => _teamRedControlledBases;
            set
            {
                _teamRedControlledBases = value;
                TeamRedBasesText.text = _teamRedControlledBases.ToString();
            }
        }

        public int TeamBlueControlledBases
        {
            get => _teamBlueControlledBases;
            set
            {
                _teamBlueControlledBases = value;
                TeamBlueBasesText.text = _teamBlueControlledBases.ToString();
            }
        }

        public void Start()
        {
            _bases = FindObjectsOfType<Base>().ToList();

            StartCoroutine(BaseScores());
            StartCoroutine(Clock());
            StartCoroutine(CheckNumberOfBases());

        }

        private IEnumerator CheckNumberOfBases()
        {

            while (true)
            {
                TeamBlueControlledBases = _bases.Count(b => b.MyTeam == Teams.BlueTeam);
                TeamRedControlledBases = _bases.Count(b => b.MyTeam == Teams.RedTeam);
                
                yield return new WaitForFixedUpdate();
            }


        }


        private IEnumerator Clock()
        {
            var currentGameTime = TotalGameTime;
            while (currentGameTime > 0)
            {
                var mins = currentGameTime / 60;
                var secs = currentGameTime % 60;

                MinTimer.text = mins.ToString("00");
                SecTimer.text = secs.ToString("00");

                currentGameTime--;
                yield return new WaitForSeconds(1f);
            }

            Endgame();
        }

        private IEnumerator BaseScores()
        {
            while (true)
            {
                TeamBlueScore += GetBaseScore(TeamBlueControlledBases);
                TeamRedScore += GetBaseScore(TeamRedControlledBases);

                TeamBlueScoreText.text = TeamBlueScore.ToString("0000");
                TeamRedScoreText.text = TeamRedScore.ToString("0000");

                if (TeamBlueScore >= 1600 || TeamRedScore >= 1600)
                {
                    Endgame();
                    Debug.Break();
                }

                yield return new WaitForSeconds(1f);
            }
        }

        private void Endgame()
        {
            StopAllCoroutines();
            // todo show final score screen
        }

        private int GetBaseScore(int numberOfBases)
        {
            switch (numberOfBases)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                case 2:
                    return 2;
                case 3:
                    return 5;
                case 4:
                    return 10;
                default:
                    Debug.LogError("Incorrect number of bases");
                    return -1;
            }
        }


        public void ScoreFlag(Teams myTeam)
        {
            if (myTeam == Teams.RedTeam)
            {
                switch (TeamRedControlledBases)
                {
                    case 0:
                        break;
                    case 1:
                        TeamRedScore += 75;
                        break;
                    case 2:
                        TeamRedScore += 85;
                        break;
                    case 3:
                        TeamRedScore += 100;
                        break;
                    case 4:
                        TeamRedScore += 500;
                        break;
                }
            }
            else
            {
                switch (TeamBlueControlledBases)
                {
                    case 0:
                        break;
                    case 1:
                        TeamBlueScore += 75;
                        break;
                    case 2:
                        TeamBlueScore += 85;
                        break;
                    case 3:
                        TeamBlueScore += 100;
                        break;
                    case 4:
                        TeamBlueScore += 500;
                        break;
                }
            }
                
        }
    }
}