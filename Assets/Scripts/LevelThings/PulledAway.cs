using UnityEngine;

public class PulledAway : MonoBehaviour {
    [Header("Movement")]
    public Transform targetPoint;
    public float speed = 3f;
    public bool isBeingPulled = false;

    [Header("Struggle Rotation")]
    public float wobbleAmount = 15f;  
    public float wobbleSpeed = 8f;     

    [Header("Sound")]
    public AudioSource audioSource;    
    public AudioClip pullSound;       

    [Header("Dialogue")]
    public GameObject helpText;        

    private Vector3 startRotation;

    void Start() {
        startRotation = transform.eulerAngles;

        if (helpText != null)
            helpText.SetActive(false);
    }

    void Update() {
        if (isBeingPulled && targetPoint != null) {
          
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPoint.position,
                speed * Time.deltaTime
            );

            float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;
            transform.rotation = Quaternion.Euler(
                startRotation.x,
                startRotation.y,
                startRotation.z + wobble
            );
        }
    }

    public void StartPull() {
        isBeingPulled = true;

        if (audioSource != null && pullSound != null) {
            audioSource.PlayOneShot(pullSound);
        }

        
        if (helpText != null) {
            helpText.SetActive(true);
        }
    }
}