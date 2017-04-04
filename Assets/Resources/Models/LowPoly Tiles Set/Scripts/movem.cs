using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movem : MonoBehaviour {

//	private float Dist = 20f;

	void Update () {
		Vector3 temp = Input.mousePosition;
		temp.z = 20f;
		this.transform.position = Camera.main.ScreenToWorldPoint(temp);
	}
}
