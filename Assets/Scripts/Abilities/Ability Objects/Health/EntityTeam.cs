using System;

namespace AbilitySystem {
    [Flags]
    public enum EntityTeam {
        None = 0,
        Enemy = 1 << 0,
        Player = 1 << 1,
        Neutral = 1 << 2
    }
    public static class EntityTeamFunctions {
        // HasFlag for enum
        public static bool HasCommonTeam(EntityTeam team1, EntityTeam team2) =>
            (team1 & team2) != EntityTeam.None;

        public static EntityTeam AddTeam(EntityTeam team, EntityTeam add) => team | add;
        public static EntityTeam RemoveTeam(EntityTeam team, EntityTeam remove) => team & ~remove;
    }
}