

namespace AbilitySystem {
    public abstract class FrameAbilitySO : AbilitySO {
        public abstract void Startup(EntityBody entityBody, AbilityData data);
        public abstract AbilityData AbilityDataSetup(EntityBody entityBody);
        public abstract bool Execute(EntityBody entityBody, AbilityData data);
        public abstract bool PassEvent(EntityBody entityBody, AbilityData data);
        public abstract void FrameEvent(EntityBody entityBody, AbilityData data);
    }
    public abstract class FrameAbilitySummary : AbilitySummary {
        public abstract void StartUp(EntityBody entityBody);
        public abstract void Activate(EntityBody entityBody, bool AbilityPressed);
        public abstract void FrameEvent(EntityBody entityBody);
    }
}