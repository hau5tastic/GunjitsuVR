using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class FlipNormals : MonoBehaviour {

	void Start () {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        Vector3[] normals = mesh.normals;
        for(int i = 0; i < normals.Length; i++)
        {
            normals[i] = -1 * normals[i];
        }	
        
        for(int i = 0; i < mesh.subMeshCount; i++)
        {
            int[] tris = mesh.triangles;

            for(int j = 0; j < tris.Length; j += 3)
            {
                int tmp = tris[j];
                tris[j] = tris[j + 1];
                tris[j + 1] = tmp;
            }

            mesh.SetTriangles(tris, i);
        }	
	}
}
