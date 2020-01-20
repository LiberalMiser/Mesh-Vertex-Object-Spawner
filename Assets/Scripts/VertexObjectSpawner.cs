using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexObjectSpawner : MonoBehaviour {

    public GameObject[] objects;
    public Mesh mesh;
    public GameObject spawnHolder;
    public Transform offsetter;
    public float objectSpawnSpeed = 0.1f;
    public Vector3 inverseRotation;
    public bool coroutined = true;
    public KeyCode togglePhysicsKey = KeyCode.Space;
    public KeyCode toggleCoroutinedPhysicssKey = KeyCode.T;
    public bool correctRotation = true;
    public bool toggleWithCoroutine = false;
    public bool toggleWithCoroutineReverse = false;

    private Quaternion objectRotation;
    private WaitForSeconds wait;
    private List<GameObject> spawned = new List<GameObject>();

    private void Start() {
        if (!spawnHolder) {
            spawnHolder = new GameObject("Spawn Holder");
        }
        wait = new WaitForSeconds(objectSpawnSpeed);
        if (coroutined) {
            StartCoroutine(Spawn());
        }
        else {
            Spawn01();
        }
    }

    private void Update() {
        if (Input.GetKeyDown(togglePhysicsKey)) {
            if (toggleWithCoroutine) {
                StartCoroutine(TogglePhysics01(!spawned[spawned.Count - 1].GetComponent<Rigidbody>().isKinematic, toggleWithCoroutineReverse));
            }
            else {
                TogglePhysics(!spawned[spawned.Count - 1].GetComponent<Rigidbody>().isKinematic);
            }
        }
        if (Input.GetKeyDown(toggleCoroutinedPhysicssKey)) {
            toggleWithCoroutineReverse = !toggleWithCoroutineReverse;
            toggleWithCoroutine = !toggleWithCoroutine;
        }
    }

    private IEnumerator Spawn () {
        objectRotation = offsetter.rotation;
        for (int i = 0; i < mesh.vertexCount; i++) {
            GameObject spawn = Instantiate(objects[Random.Range(0, objects.Length)], mesh.vertices[i] + offsetter.position, Quaternion.Euler(inverseRotation), spawnHolder.transform);
            spawned.Add(spawn);
            yield return objectSpawnSpeed;
        }
        if (correctRotation) {
            offsetter.rotation = Quaternion.identity;
        }
    }

    private void Spawn01() {
        objectRotation = offsetter.rotation;
        for (int i = 0; i < mesh.vertexCount; i++) {
            GameObject spawn = Instantiate(objects[Random.Range(0, objects.Length)], mesh.vertices[i] + offsetter.position, Quaternion.Euler(inverseRotation), spawnHolder.transform);
            spawned.Add(spawn);
        }
        if (correctRotation) {
            offsetter.rotation = Quaternion.identity;
        }
    }

    private void TogglePhysics (bool kinematic) {
        for (int i = 0; i < spawned.Count; i++) {
            spawned[i].GetComponent<Rigidbody>().isKinematic = kinematic;
        }
    }

    private IEnumerator TogglePhysics01 (bool kinematic, bool reverse) {
        if (reverse) {
            for (int i = spawned.Count - 1; i > 0; i--) {
                spawned[i].GetComponent<Rigidbody>().isKinematic = kinematic;
                yield return objectSpawnSpeed;
            }
        }
        else {
            for (int i = 0; i < spawned.Count; i++) {
                spawned[i].GetComponent<Rigidbody>().isKinematic = kinematic;
                yield return objectSpawnSpeed;
            }
        }
    }

}
