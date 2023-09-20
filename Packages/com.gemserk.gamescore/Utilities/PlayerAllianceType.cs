using System;
using System.Runtime.CompilerServices;

namespace Game.Utilities
{
    [Flags]
    public enum PlayerAllianceType
    {
        Nothing = 0,
        Enemies = 1 << 0,
        Allies = 1 << 1,
        Everything = -1
    }

    public static class PlayerAllianceExtensions
    {
        public static bool HasAllianceFlag(this PlayerAllianceType self, PlayerAllianceType flag)
        {
            return (self & flag) == flag;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckPlayerAlliance(this PlayerAllianceType playerAllianceType, int p0, int p1)
        {
            // get if p0 and p1 are allies or enemies from some config?
            // for now we assume all players are enemies

            if (playerAllianceType == PlayerAllianceType.Everything)
                return true;

            var allies = (p0 & p1) != 0;

            return (allies && playerAllianceType == PlayerAllianceType.Allies) || (
                !allies && playerAllianceType == PlayerAllianceType.Enemies);
            
            // var allies = p0 == p1;
            // var enemies = !allies;
            //
            // if (allies && playerAllianceType.HasAllianceFlag(PlayerAllianceType.Allies))
            //     return true;
            //
            // if (enemies && playerAllianceType.HasAllianceFlag(PlayerAllianceType.Enemies))
            //     return true;

            // return false;
        }

        public static int GetAlliedPlayers(int player)
        {
            // for now we are only ally of our same team...
            return 1 << player;
        }
    }
}