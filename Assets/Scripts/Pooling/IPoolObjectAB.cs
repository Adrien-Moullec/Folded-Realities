namespace AbilitySystem {
    public interface IPoolObjectAS {
        public void GetIPoolObj(EntityBody body);
        public void ReleaseIPoolObj(EntityBody body);
        public void OnDestroyIPoolObj(EntityBody body);
    }
}