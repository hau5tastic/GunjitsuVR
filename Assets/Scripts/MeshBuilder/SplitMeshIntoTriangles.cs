using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitMeshIntoTriangles : MonoBehaviour {
 
   public virtual void SplitMesh() {
        Debug.Log("SplitMesh() Called");
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
            for (int i = 0; i < indices.Length; i += 6) {
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
                GO.transform.position = transform.position;
                GO.transform.rotation = transform.rotation;
                GO.transform.localScale = transform.localScale;
                GO.AddComponent<MeshRenderer>().material = MR.materials[submesh];
                GO.AddComponent<MeshFilter>().mesh = mesh;
                GO.AddComponent<BoxCollider>();
                GO.AddComponent<Rigidbody>().AddExplosionForce(100, transform.position, 30);
                GO.layer = LayerMask.NameToLayer("MeshBuild");

                Destroy(GO, 5 + Random.Range(0.0f, 2.0f));
            }
        }
        MR.enabled = false;

        // Time.timeScale = 0.2f;
        // yield return new WaitForSeconds(0.8f);
        // Time.timeScale = 1.0f;
        Destroy(gameObject);
    }
}
