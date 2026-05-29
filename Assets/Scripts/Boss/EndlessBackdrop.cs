using System;
using System.Collections.Generic;

using UnityEngine;

public class EndlessBackdrop : MonoBehaviour {

    [SerializeField] float BackdropSpeed = 5;
    [SerializeField] GameObject lowShelf;
    [SerializeField] GameObject upShelf;
    [SerializeField, Range(2, 6)] int backdropNumber = 4;

    List<BackdropInfo> backdropInfos = new();

    float lowShelfFloat;
    float upShelfFloat;
    float yDiff;
    float currentYChangeValue = 0;

    private void Awake() {
        lowShelfFloat = lowShelf.gameObject.transform.position.y;
        upShelfFloat = upShelf.gameObject.transform.position.y;
        yDiff = upShelfFloat - lowShelfFloat;


        backdropInfos.Add(new BackdropInfo(Instantiate(lowShelf, transform), YPos(yDiff, 0)));
        backdropInfos.Add(new BackdropInfo(Instantiate(lowShelf, transform), YPos(yDiff, 1)));
        for (int i = 2; i < backdropNumber; i++) {
            backdropInfos.Add(new BackdropInfo(Instantiate(lowShelf, transform), YPos(yDiff, i)));
        }
        lowShelf.SetActive(false);
        upShelf.SetActive(false);

        foreach (var n in backdropInfos)
            n.gameObject.transform.position = new Vector3(n.gameObject.transform.position.x, n.startYPos, n.gameObject.transform.position.x);

    }

    void Update() {
        currentYChangeValue += Time.deltaTime * BackdropSpeed;
        currentYChangeValue %= yDiff;

        foreach (var n in backdropInfos)
            n.gameObject.transform.position = new Vector3(n.gameObject.transform.position.x, n.startYPos + currentYChangeValue, n.gameObject.transform.position.x);
    }

    //(I - 1)x + 1/2x
    float YPos(float space, int index) {
        return (index - 1) * space + 0.5f * space;
    }

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
