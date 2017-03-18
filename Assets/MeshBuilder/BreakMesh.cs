using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakMesh : MonoBehaviour {

    public float minDuration;
    public float maxDuration;
    public float time;
    public GameObject pulseLightPrefab;

    void Start() {
        SplitMesh();
    }

    void SplitMesh() {
        MeshFilter MF = GetComponent<MeshFilter>();
        MeshRenderer MR = GetComponent<MeshRenderer>();
        Mesh M = MF.mesh;
        Vector3[] verts = M.vertices;
        Vector3[] normals = M.normals;
        Vector2[] uvs = M.uv;

        // For the number of submeshes
        for (int submesh = 0; submesh < M.subMeshCount; submesh++) {
            int[] indices = M.GetTriangles(submesh);
            // For the number of triangles of each submesh
            for (int i = 0; i < indices.Length; i += 3) {
                Vector3[] newVerts = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUvs = new Vector2[3];
                for (int n = 0; n < 3; n++) {
                    int index = indices[i + n];
                    newVerts[n] = verts[index];
                    newUvs[n] = uvs[index];
                    newNormals[n] = normals[index];
                }
                Mesh mesh = new Mesh();
                mesh.vertices = newVerts;
                mesh.normals = newNormals;
                mesh.uv = newUvs;

                mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };

                GameObject GO = new GameObject("Triangle " + (i / 3));
                GO.transform.position = GO.transform.position + new Vector3(Random.Range(1, 100), Random.Range(1, 100), Random.Range(1, 100));
                // GO.transform.position = Camera.main.transform.position;
                // GO.transform.position = new Vector3(0, 50, 0);
                // GO.transform.position = new Vector3(0, -50, 0);
                GO.transform.rotation = Random.rotation;
                Fragment f = GO.AddComponent<Fragment>();
                f.maxDuration = maxDuration;
                f.endPosition = transform.position;
                f.endRotation = transform.rotation;
                f.gameObject.transform.localScale = transform.localScale;
                f.builtObject = gameObject;
                GO.AddComponent<MeshRenderer>().material = MR.materials[submesh];
                GO.AddComponent<MeshFilter>().mesh = mesh;

                // Destroy(GO, 5 + Random.Range(0.0f, 5.0f));
            }
        }


        // Time.timeScale = 0.2f;
        // yield return new WaitForSeconds(0.8f);
        // Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }

}
