using System.Collections.Generic;
using Assets.General_Scripts;
using UnityEngine;

namespace Assets.EOTS
{
    public class Base : MonoBehaviour
    {
        public List<ISoldier> MemberOfTeamBlue;
        public List<ISoldier> MemberOfTeamRed;

        public Material _myMat;
        public Teams MyTeam;
        public Texture Noise;

        private GameManager _gm;
        private static readonly int BumpScale = Shader.PropertyToID("_BumpScale");

        private void Awake()
        {
            MemberOfTeamBlue = new List<ISoldier>();
            MemberOfTeamRed = new List<ISoldier>();
            _gm = FindObjectOfType<GameManager>();
            _myMat = GetComponent<MeshRenderer>().materials[0];
            _myMat.EnableKeyword("_NORMALMAP");
        }

        private void OnTriggerEnter(Collider other)
        {
            //print("Entered: " + other.gameObject.name);

            if (other.gameObject.TryGetComponent<ISoldier>(out var soldier) == false) return;
            
            switch (soldier.MyTeam)
            {
                case Teams.BlueTeam:
                    MemberOfTeamBlue.Add(soldier);
                    break;
                case Teams.RedTeam:
                    MemberOfTeamRed.Add(soldier);
                    break;
                default:
                    Debug.LogError("Unknown team");
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<ISoldier>(out var soldier) == false) return;

            switch (soldier.MyTeam)
            {
                case Teams.BlueTeam:
                    MemberOfTeamBlue.Remove(soldier);
                    break;
                case Teams.RedTeam:
                    MemberOfTeamRed.Remove(soldier);
                    break;
                default:
                    Debug.LogError("Unknown team");
                    break;
            }
        }


        private void FixedUpdate()
        {
            if (_myMat.color.r < 0.1 && _myMat.color.g < 0.1)
            {
                MyTeam = Teams.BlueTeam;
                _myMat.SetFloat(BumpScale, 5);

            }
            else if (_myMat.color.b < 0.1 && _myMat.color.g < 0.1)
            {
                MyTeam = Teams.RedTeam;
                _myMat.SetFloat(BumpScale, 5);
            }
            else
            {
                _myMat.SetFloat(BumpScale, 0);
            }

            if (MemberOfTeamBlue.Count > MemberOfTeamRed.Count)
                _myMat.color = Color.Lerp(_myMat.color, Color.blue, Time.deltaTime * MemberOfTeamBlue.Count * .2f);
            else if (MemberOfTeamBlue.Count < MemberOfTeamRed.Count)
                _myMat.color = Color.Lerp(_myMat.color, Color.red, Time.deltaTime * MemberOfTeamRed.Count * .2f);

            
        }
    }
}