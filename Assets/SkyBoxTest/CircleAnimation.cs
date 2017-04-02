using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAnimation : MonoBehaviour {

    Mesh animatedMesh;
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    //List<Color> colors = new List<Color>();

    int index = 0;
    int numFaces;

    Vector3[] verts;
    int[] tris;

    float animSpeed;
    Color color;
    public bool doneLoading = false;

    public void Init(Mesh mesh, float _animSpeed)
    {
        animSpeed = _animSpeed;
        numFaces = mesh.triangles.Length;
        verts = mesh.vertices;
        tris = mesh.triangles;

        animatedMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = animatedMesh;
    }

    public void SetColor(Color _color)
    {
        color = _color;
        color.a = 0.5f;
        GetComponent<MeshRenderer>().material.color = color;//SetColor("_Color", color);
    }

    public IEnumerator AddFace()
    {
        while (index < numFaces)
        {
            animatedMesh.Clear();

            vertices.Add(verts[tris[index]]);
            vertices.Add(verts[tris[index + 1]]);
            vertices.Add(verts[tris[index + 2]]);

            triangles.Add(index);
            triangles.Add(index + 1);
            triangles.Add(index + 2);

            //colors.Add(color);
            //colors.Add(color);
            //colors.Add(color);

            animatedMesh.vertices = vertices.ToArray();
            animatedMesh.triangles = triangles.ToArray();
            //animatedMesh.colors = colors.ToArray();

            index += 3;

            yield return new WaitForSeconds(animSpeed);
        }

        doneLoading = true;

        StopCoroutine(AddFace());
    }
}
