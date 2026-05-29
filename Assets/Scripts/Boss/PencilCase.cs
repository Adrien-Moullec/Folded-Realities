

using UnityEngine;
using UnityEngine.Pool;

public class PencilCase : MonoBehaviour {
    [SerializeField] PencilCaseObject pencilCaseObject;
    public ObjectPool<PencilCaseObject> pencilPool;
    public Vector3 direction;
    public float gap = 5;

    Vector3 prefLoc;
    float time = 0;

    void Awake() {
        prefLoc = pencilCaseObject.gameObject.transform.position;
        pencilCaseObject.gameObject.SetActive(false);

        pencilPool = new ObjectPool<PencilCaseObject>(
            createFunc: () => Instantiate(pencilCaseObject, transform),
            actionOnGet: damageObj => OnSpawn(damageObj),
            actionOnRelease: damageObj => Despawn(damageObj),
            actionOnDestroy: damageObj => Destroy(damageObj.gameObject),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 100);
    }
    void Update() {
        time += Time.deltaTime;
        if (time > gap) { pencilPool.Get().OnSpawn(this); time = 0; }
    }
    public void OnSpawn(PencilCaseObject pencilCaseObject) {
        pencilCaseObject.gameObject.SetActive(true);
        pencilCaseObject.transform.position = prefLoc;
        pencilCaseObject.OnSpawn(this);
    }
    public void Despawn(PencilCaseObject pencilCaseObject) {
        pencilCaseObject.gameObject.SetActive(false);
    }
}
