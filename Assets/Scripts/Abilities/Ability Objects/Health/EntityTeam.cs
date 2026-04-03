namespace AbilitySystem {
    [System.Flags]
    public enum EntityTeam {
        None = 0,
        Enemy = 1 << 0,
        Player = 1 << 1,
        Neutral = 1 << 2,
    }
}