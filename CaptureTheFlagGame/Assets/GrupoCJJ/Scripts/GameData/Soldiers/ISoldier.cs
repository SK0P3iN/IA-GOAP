using Assets.EOTS;
using UnityEngine;

namespace Assets.General_Scripts
{
    public interface ISoldier
    {
        Transform MyTransform { get; set; }
        Teams MyTeam { get; set; }
        bool Invulnerable { get; set; }
        bool HasFlag { get; set; }


        void Died();

    }
}