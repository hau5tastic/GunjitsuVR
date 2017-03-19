using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMaker : MonoBehaviour {

    public GameObject boxPrefab;
    public int width;
    public int height;
    public float spacing;

	void Awake() {
        float s = boxPrefab.GetComponent<Renderer>().bounds.size.x + spacing;
        transform.position = new Vector3(transform.position.x, s, transform.position.z);

		for (int h = 0; h < height; ++h) {
            for (int w = 0; w < width; ++w) {
                Vector3 offset = new Vector3(w * s, h * s, 0);
                GameObject g = Instantiate(boxPrefab, transform.position + offset, Quaternion.identity);
            }
        }



	}
	
}
