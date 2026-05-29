using System;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Endless backdrop of an item, created for a neverending bookshelf. Allows for a freefalling gamemode that ends when we want.
/// </summary>
public class EndlessBackdrop : MonoBehaviour {

    [Tooltip("The speed that the backdrop travels up.")]
    [SerializeField] float BackdropSpeed = 5;
    [Tooltip("Gameobject of the backdrop lower shelf.")]
    [SerializeField] GameObject lowShelf;
    [Tooltip("Gameobject of the backdrop upper shelf.")]
    [SerializeField] GameObject upShelf;
    [Tooltip("Sets the number of reoccuring items in a pool.")]
    [SerializeField, Range(2, 6)] int backdropNumber = 4;
    [Tooltip("Contains a shelf list to be updated framely.")]
    List<BackdropInfo> backdropInfos = new();

    [Tooltip("Initial y position of low shelf.")]
    float lowShelfFloat;
    [Tooltip("Initial y position of upper shelf.")]
    float upShelfFloat;
    [Tooltip("Initial y difference between 2 shelfs.")]
    float yDiff;
    [Tooltip("Updated y value over time to move the shelfs by.")]
    float currentYChangeValue = 0;

    /// <summary>
    /// Setup shelf values and create reoccuring shelfs.
    /// </summary>
    private void Awake() {
        lowShelfFloat = lowShelf.gameObject.transform.position.y;
        upShelfFloat = upShelf.gameObject.transform.position.y;
        yDiff = upShelfFloat - lowShelfFloat;

        backdropInfos.Add(new BackdropInfo(Instantiate(lowShelf, transform), YPos(yDiff, 0)));
        backdropInfos.Add(new BackdropInfo(Instantiate(lowShelf, transform), YPos(yDiff, 1)));
        for (int i = 2; i < backdropNumber; i++) {
            backdropInfos.Add(new BackdropInfo(Instantiate(lowShelf, transform), YPos(yDiff, i)));
        }

        /// Disable original shelfs
        lowShelf.SetActive(false);
        upShelf.SetActive(false);

        foreach (var n in backdropInfos)
            n.gameObject.transform.position = new Vector3(n.gameObject.transform.position.x, n.startYPos, n.gameObject.transform.position.x);

    }

    /// <summary>
    /// Update y position of each shelf based on time and original position.
    /// </summary>
    void Update() {
        currentYChangeValue += Time.deltaTime * BackdropSpeed;
        currentYChangeValue %= yDiff;

        foreach (var n in backdropInfos)
            n.gameObject.transform.position = new Vector3(n.gameObject.transform.position.x, n.startYPos + currentYChangeValue, n.gameObject.transform.position.x);
    }

    /// <summary>
    /// Calculate y position based on list ID
    /// </summary>
    float YPos(float space, int index) => (index - 1) * space + 0.5f * space; //(I - 1)x + 1/2x

    /// <summary>
    /// Contains structure for each backdrop item
    /// </summary>
    [Serializable]
    private class BackdropInfo {
        public GameObject gameObject;
        [HideInInspector] public float startYPos;
        public BackdropInfo(GameObject gameObject, float startYPos) {
            this.gameObject = gameObject;
            this.startYPos = startYPos;
        }
    }
}
