//using UnityEngine;

//public class NPCCatchTrigger : MonoBehaviour {
//    bool triggered = false;

//    void OnTriggerEnter(Collider other) {
//        if (triggered) {
//            return;
//        }

//        if (other.CompareTag("Player")) {
//            triggered = true;

//            Time.timeScale = 0.4f;

//            BossFightManager.Instance
//                .TriggerCraneSequence();

//            Time.timeScale = 1f;
//        }
//    }
//}