//using System;
//using System.Collections;


//using UnityEngine;

//namespace AbilitySystem {
//    [Serializable]
//    public class PoolObject_OLD : MonoBehaviour {
//        [SerializeField] Action setupAction;
//        [SerializeField] Action updateAction;
//        private float time = 0;
//        Pass through functions from projectile ability rather than custom ones here
//         !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//        public void Setup() {
//            setupAction();
//        }

//        public IEnumerator OnStart() {
//            yield return null;
//        }

//        public void UpdateObject() {
//            updateAction();
//        }

//        public IEnumerator OnEnd() {
//            yield return null;
//        }
//    }
//}