using AbilitySystem;
using UnityEngine;

namespace AbilitySystem {
    public class CooldownData : AbilityData {
        [HideInInspector] internal float cooldownDelta;
        [HideInInspector] internal int currentCharges;
        [HideInInspector] internal bool isRecharging = false;
        [HideInInspector] internal bool isUsing = false;
        public CooldownData() { }
        public CooldownData(int charges, float cooldown) {
            currentCharges = charges;
            cooldownDelta = cooldown;
        }
    }
}