using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace AbilitySystem {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerAbilityController : AbilityController {

        [Space]
        [Header("Abilities")]
        [SerializeField] List<PlayerAbilitySetSO> abilitySetSO;
        [HideInInspector] List<PlayerAbilitySet> abilitySetList = new();
        [HideInInspector] internal PlayerAbilitySet currentAbilitySet;
        private CharacterController characterController;

        protected override void Awake() {
            base.Awake();
            characterController = GetComponent<CharacterController>();

            foreach (var n in abilitySetSO) {
                if (n == null)
                    continue;
                PlayerAbilitySet ab = new PlayerAbilitySet(n);
                abilitySetList.Add(ab);
            }
            currentAbilitySet = abilitySetList[0];
        }


        public void SetAbility(string name) {
            if (abilitySetList.Any(x => x.abilitySetName == name))
                currentAbilitySet = abilitySetList.First(x => x.abilitySetName == name);
            else
                Debug.LogWarning("No ability to set of name " + name);
        }

        #region Input Functions

        public override void InputMove(Vector3 moveInput, bool dash) =>
            currentAbilitySet?.movement.Activate(entityBody, moveInput, dash);
        public override void InputPrimaryAttack() {
            currentAbilitySet?.light.Activate(entityBody);
        }
        public override void InputPrimaryAbility() =>
            throw new NotImplementedException();
        #endregion

        #region Movement Functions

        public override void OnMoveEntity(Vector3 direction) {
            characterController.Move(direction);
        }
        public override void OnRotateEntity(Vector3 movement) {
            throw new NotImplementedException();
        }
        #endregion
    }
}