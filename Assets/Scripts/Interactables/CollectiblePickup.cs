using System;
using System.Collections;

using UnityEngine;

[RequireComponent(typeof(Outline))]
public class CollectiblePickup : MonoBehaviour {
    [SerializeField] AudioClip pickupSound;
    [SerializeField] float deactivateTime;
    [SerializeField] float pickupFloatSpeed = 2f;
    [SerializeField] float pickupRotationSpeed = 360f;
    public bool isSpecial = false;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        CollectiblesManager manager = other.GetComponent<CollectiblesManager>();

        if (manager == null) {
            return;
        }

        if (isSpecial) {
            manager.CollectSpecial(gameObject);
        } else {
            manager.CollectNormal(gameObject);
        }

        StartCoroutine(PlayPickupEffect());
    }
    IEnumerator PlayPickupEffect() {
        AudioClip sound = pickupSound == null ? CollectiblesManager.Instance.pickupSound : pickupSound;

        if (sound != null) AudioSource.PlayClipAtPoint(sound, transform.position);


        Collider col = GetComponent<Collider>();
        if (col != null) {
            col.enabled = false;
        }

        CollectibleIdle idle = GetComponent<CollectibleIdle>();
        if (idle != null) {
            idle.enabled = false;
        }

        float timer = 0f;

        Material mat = GetComponent<Renderer>().material;
        Outline outline = GetComponent<Outline>();
        float delta = 0;

        while (timer <= deactivateTime) {
            delta = 1 - (timer / deactivateTime);

            transform.position += Vector3.up * pickupFloatSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up * pickupRotationSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            mat.SetFloat("_Alpha", delta);
            outline.OutlineColor = new(outline.OutlineColor.r, outline.OutlineColor.g, outline.OutlineColor.b, delta);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}