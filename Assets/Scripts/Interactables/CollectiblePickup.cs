using System;
using System.Collections;

using UnityEngine;

[RequireComponent(typeof(Outline))]
public class CollectiblePickup : MonoBehaviour {

    [Header("Pickup Options")]
    [SerializeField] AudioClip pickupSound;

    // Time before collectible disappears
    [SerializeField] float deactivateTime;

    [SerializeField] float pickupFloatSpeed = 2f;
    [SerializeField] float pickupRotationSpeed = 360f;

    [Header("Collectible Idle")]
    public float hoverHeight = 0.25f;
    public float hoverSpeed = 2f;
    public float rotationSpeed = 90f;

    [Header("Player Pref")]
    [SerializeField] PlayerPrefIDGenerator playerPrefIDGenerator;

    public bool isSpecial = false;

    // Stores original hover position
    private Vector3 startPos;

    void Awake() {

        startPos = transform.position;
    }

    void Update() {

        // Floating hover animation
        float newY = startPos.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        transform.position = new Vector3(startPos.x, newY, startPos.z);

        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {

        // Only player can collect
        if (!other.CompareTag("Player")) {
            return;
        }

        //playerPrefIDGenerator.SetPlayerPrefIdActive(false);

        CollectiblesManager manager = other.GetComponent<CollectiblesManager>();

        if (manager == null) {
            return;
        }

        // Handles special vs normal collectibles
        if (isSpecial) {
            manager.CollectSpecial(gameObject);
        } else {
            manager.CollectNormal(gameObject);
        }

        StartCoroutine(PlayPickupEffect());
    }

    IEnumerator PlayPickupEffect() {

        AudioClip sound = pickupSound == null ? CollectiblesManager.Instance.pickupSound : pickupSound;

        if (sound != null)
            AudioSource.PlayClipAtPoint(sound, transform.position);

        // Disables collider after pickup
        Collider col = GetComponent<Collider>();

        if (col != null) {
            col.enabled = false;
        }

        float timer = 0f;

        Material mat = GetComponent<Renderer>().material;

        Outline outline = GetComponent<Outline>();

        float delta = 0;

        // Fade and float effect
        while (timer <= deactivateTime) {

            delta = 1 - (timer / deactivateTime);

            transform.position += Vector3.up * pickupFloatSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up * pickupRotationSpeed * Time.deltaTime);

            timer += Time.deltaTime;

            mat.SetFloat("_Alpha", delta);

            outline.OutlineColor = new(
                outline.OutlineColor.r,
                outline.OutlineColor.g,
                outline.OutlineColor.b,
                delta
            );

            yield return null;
        }

        gameObject.SetActive(false);
    }
}