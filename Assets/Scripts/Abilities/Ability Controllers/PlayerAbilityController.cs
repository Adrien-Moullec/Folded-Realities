using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace AbilitySystem {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerAbilityController : AbilityController {

        [Space]
        [Header("Transition Animation")]
        [SerializeField] Animation SmokesAndMirrorsAnimation;
        [SerializeField] AbilityAnimation transitionAnimationClip;
        [SerializeField] float transitionTime = 0.5f;
        private bool isTransitioning = false;

        [Space]
        [Header("Abilities")]
        [SerializeField] List<PlayerSetSummary> playerSetsList;
        [HideInInspector] public PlayerSetSummary currentAbilitySet;
        private CharacterController characterController;

        #region OnStart
        protected override void Awake() {
            base.Awake();
            SmokesAndMirrorsAnimation.gameObject.SetActive(false);
            characterController = GetComponent<CharacterController>();
            transitionAnimationClip.Setup(SmokesAndMirrorsAnimation, WrapMode.Once);
            foreach (var i in playerSetsList) {
                if (i.abilitySetSO == null)
                    continue;
                i.playerAbilitySet = new PlayerAbilitySet(i.abilitySetSO);
                i.entityBody.iAbility = this;
                i.entityBody.modelPrefab.SetActive(false);
                i.playerAbilitySet.movement.AbilityData = playerSetsList[0].playerAbilitySet.movement.AbilityData;
                i.entityBody.iHealth = this;
            }
            currentAbilitySet = playerSetsList[0];
            currentAbilitySet.entityBody.modelPrefab.SetActive(true);
        }
        public override void SetupAnimations() {
            foreach (var n in playerSetsList)
                n.abilitySetSO?.SetupAnimations(n.entityBody.animationComponent);
        }
        public override void Die() {
            base.Die();
        }
        #endregion

        #region Transitions
        public override void OnEvent(string eventMessage) => InputTransitionName(eventMessage);
        public bool UnlockSet(string name) {
            if (playerSetsList.Any(x => x.abilitySetSO.abilitySetName == name)) {
                playerSetsList.First(x => x.abilitySetSO.abilitySetName == name).isUnlocked = true;
                return true;
            }
            return false;
        }
        public override void InputTransitionName(string name) {
            if (isTransitioning) return;

            if (!playerSetsList.Any(x => x.abilitySetSO.abilitySetName == name)) {
                Debug.LogWarning("No ability set of that name.");
                return;
            }

            PlayerSetSummary checkAbilitySet = playerSetsList.First(x => x.abilitySetSO.abilitySetName == name);
            if (checkAbilitySet == currentAbilitySet || !checkAbilitySet.isUnlocked)
                return;

            StartCoroutine(RunAnimationsWithEvents(
                new TimelineEvent[] {
                    new TimelineEvent(currentAbilitySet.entityBody.animationComponent, currentAbilitySet.abilitySetSO.transitionAnimation, 0, transitionTime/2),
                    new TimelineEvent(checkAbilitySet.entityBody.animationComponent, checkAbilitySet.abilitySetSO.transitionAnimation, transitionTime/2, transitionTime, true),
                    new TimelineEvent(SmokesAndMirrorsAnimation, transitionAnimationClip, 0, transitionTime)
                },
                new DeltaEvent[] {
                    new DeltaEvent(() => {
                        isTransitioning = true;
                        SmokesAndMirrorsAnimation.gameObject.SetActive(true);
                    }, 0),
                    new DeltaEvent(() => {
                        currentAbilitySet.entityBody.modelPrefab.SetActive(false);
                        Debug.Log(currentAbilitySet.entityBody.modelPrefab.name);
                        checkAbilitySet.entityBody.modelPrefab.SetActive(true);
                        currentAbilitySet = checkAbilitySet;
                    }, 0.5f),
                    new DeltaEvent(() => {
                        isTransitioning = false;
                        SmokesAndMirrorsAnimation.gameObject.SetActive(false);
                    }, 1)
                }
            ));
        }
        #endregion

        #region Movement Functions
        public override void OnMoveEntity(Vector3 direction, float turnSpeed) {
            characterController.Move(direction);
            direction.y = 0;
            if (direction != Vector3.zero) currentAbilitySet.entityBody.bodyHolder.transform.forward = direction;
        }
        public override void OnRotateEntity(Vector3 direction) {
            direction.y = 0;
            if (direction != Vector3.zero) currentAbilitySet.entityBody.bodyHolder.transform.forward = direction;
        }
        #endregion

        #region Input Functions
        public override void InputMove(Vector3 moveInput, bool isRunning) {
            base.InputMove(moveInput, isRunning);
            currentAbilitySet.
            playerAbilitySet?.
            movement.Activate(
                currentAbilitySet.entityBody,
                moveInput,
                isRunning);
        }
        public override void InputPrimaryAttack() {
            base.InputPrimaryAttack();
            currentAbilitySet.playerAbilitySet?.light.Activate(currentAbilitySet.entityBody);
        }

        public override void InputPrimaryAbility() {
            base.InputPrimaryAbility();
            currentAbilitySet.playerAbilitySet?.primary.Activate(currentAbilitySet.entityBody);
        }
        #endregion

        #region Data
        public override EntityBody GetEntityBody() => currentAbilitySet.entityBody;

        [Serializable]
        public class PlayerSetSummary {
            public bool isUnlocked = false;
            public PlayerAbilitySetSO abilitySetSO;
            public EntityBody entityBody;
            [HideInInspector] public PlayerAbilitySet playerAbilitySet;
        }
        #endregion
    }
}