using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMaker : MonoBehaviour {

    public GameObject boxPrefab;
    public int width;
    public int height;
    public float spacing;
    public float heightOffset;
    public bool isBrickWall;
    int area;

	void Awake() {
        area = width * height;
        CreateWall();
	}

    void CreateWall() {
        float bw = boxPrefab.GetComponent<Renderer>().bounds.size.x;
        float bh = boxPrefab.GetComponent<Renderer>().bounds.size.y;
        transform.position = new Vector3(transform.position.x, bh/2 + heightOffset, transform.position.z);

        // int spawned = 0;
        for (int h = 0; h < height; ++h) {
            for (int w = 0; w < width; ++w) {
                // spawned++;
                // if (spawned >= area) return;
                Vector3 offset = new Vector3(w * bw, h * bh, 0);
                if (h % 2 != 0 && isBrickWall) {
                    offset.x += bw / 2f + spacing;

                } else {
                    offset.x += spacing;
                }

                GameObject g = Instantiate(boxPrefab, transform, false);
                g.transform.localPosition = (offset);

            }
        }
    }
	
}
