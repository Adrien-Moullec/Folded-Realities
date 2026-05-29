

namespace AbilitySystem {
    /// <summary>
    /// A scriptable object ability type that requires constant update checks from the entity, such as cooldown or continuous buffs.
    /// </summary>
    /// <param name="entityBody"> Required entity body information that is passed through each function </param>
    /// <param name="data"> Stored ability data from being used in the scriptable object </param>
    public abstract class FrameAbilitySO : AbilitySO {

        /// <summary>
        /// Start function for setup purposes.
        /// </summary>
        public abstract void Startup(EntityBody entityBody, AbilityData data);
        /// <summary>
        /// A mandatory function for each ability SO to determine what type of ability data each ability uses.
        /// </summary>
        public abstract AbilityData AbilityDataSetup(EntityBody entityBody);
        /// <summary>
        /// Code that plays when an input is activated.
        /// </summary>
        public abstract bool Execute(EntityBody entityBody, AbilityData data);
        /// <summary>
        /// Code that plays when ability is not being pressed.
        /// </summary>
        public abstract bool PassEvent(EntityBody entityBody, AbilityData data);
        /// <summary>
        /// Code that plays out every frame regardless of input.
        /// </summary>
        public abstract void FrameEvent(EntityBody entityBody, AbilityData data);
    }

    /// <summary>
    /// A holder for a scriptable object so ability data can be held and accessed.
    /// </summary>
    public abstract class FrameAbilitySummary : AbilitySummary {
        /// <summary>
        /// Transfers to abilitySO Startup()
        /// </summary>
        public abstract void StartUp(EntityBody entityBody);
        /// <summary>
        /// Transfers to abilitySO Execute() or PassEvent()
        /// </summary>
        public abstract void Activate(EntityBody entityBody, bool AbilityPressed);
        /// <summary>
        /// Transfers to abilitySO FrameEvent()
        /// </summary>
        public abstract void FrameEvent(EntityBody entityBody);
    }
}