using System;

using UnityEngine;


namespace AbilitySystem {
    /// <summary>
    /// Ability template
    /// </summary>
    public abstract class AbilitySO : ScriptableObject {

        /// <summary>
        /// Optional callable gizmo function to show attack ranges or custom ability displays.
        /// </summary>
        /// <param name="entityBody"> Entity data for the gizmo to base abilities off </param>
        public virtual void GizmoEvent(EntityBody entityBody) { }

        /// <summary>
        /// Ability cost for unused AI purposes
        /// </summary>
        /// <returns> the cost of using an ability </returns>
        public virtual float AbilityCost() { return 0; }
    }
    /// <summary>
    /// Base summary class that holds data for a particular instance of an ability, to allow scriptable objects to have stored data
    /// </summary>
    [Serializable]
    public abstract class AbilitySummary {
        /// Base variable for storing ability data, allows any type of data to be stored based on the ability.
        public AbilityData AbilityData;
        /// <summary>
        /// OnDrawGizmos option for ability gizmo to be used in character controller
        /// </summary>
        public abstract void OnDrawGizmos(EntityBody entityBody);
    }
}