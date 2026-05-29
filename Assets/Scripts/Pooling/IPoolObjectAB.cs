namespace AbilitySystem {
    /// <summary>
    /// Pool object interface for projectile abilities in ability system.
    /// </summary>
    public interface IPoolObjectAS {
        public void GetIPoolObj(EntityBody body);
        public void ReleaseIPoolObj(EntityBody body);
        public void OnDestroyIPoolObj(EntityBody body);
    }
}