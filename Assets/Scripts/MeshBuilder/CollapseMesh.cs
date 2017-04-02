using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseMesh : SplitMeshIntoTriangles {

    Vector3 pointOfImpact;

    public void SetPointOfImpact(Vector3 _pointOfImpact)
    {
        pointOfImpact = _pointOfImpact + Vector3.forward * 2.0f;
    }

    public override void SplitMesh()
    {
        //Debug.Log("SplitMesh() Called");
        //MeshFilter MF = GetComponent<MeshFilter>();

        SkinnedMeshRenderer MR = GetComponentInChildren<SkinnedMeshRenderer>();
        Mesh M = MR.sharedMesh;
        Vector3[] verts = M.vertices;
        Vector3[] normals = M.normals;
        Vector2[] uvs = M.uv;

        // For the number of submeshes
        for (int submesh = 0; submesh < M.subMeshCount; submesh++)
        {
            int[] indices = M.GetTriangles(submesh);
            // For the number of triangles of each submesh
            for (int i = 0; i < indices.Length; i += 3)
            {
                Vector3[] newVerts = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUvs = new Vector2[3];
                for (int n = 0; n < 3; n++)
                {
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
                GO.AddComponent<Rigidbody>().AddExplosionForce(10, pointOfImpact, 30);
                GO.layer = LayerMask.NameToLayer("MeshFragment");

                Destroy(GO, Random.Range(0.1f, 1.0f));
            }
        }
        MR.enabled = false;

        // Time.timeScale = 0.2f;
        // yield return new WaitForSeconds(0.8f);
        // Time.timeScale = 1.0f;
        Destroy(gameObject);
    }
}
