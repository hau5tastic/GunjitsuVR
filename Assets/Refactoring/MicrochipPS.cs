using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrochipPS : MonoBehaviour {

    public GameObject microParticle;
    public float interval;
    public float amount;
    public float lifetime;

    void Start() {
        StartCoroutine(Emit());
    }

    IEnumerator Emit() {
        while (true) {
            for (int i = 0; i < amount; ++i) {
                Destroy(Instantiate(microParticle, transform.position, Quaternion.identity), lifetime);
            }
            yield return new WaitForSeconds(interval);
        }

    }
}
