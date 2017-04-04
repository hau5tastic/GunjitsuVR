using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreArea : MonoBehaviour {

    [SerializeField]
    GameObject childPrefab;
    [SerializeField]
    int numSubDivisions;
    [SerializeField]
    float animationSpeed;
    [SerializeField]
    float size = 10;

    int numObjects = 5;

    CircleAnimation[] childCircles;
    int index = 0;

	void Awake() {
        CreateChildren();      
    }

    void CreateChildren()
    {
        // create the meshes first
        Mesh[] circles = new Mesh[numObjects];       
        circles[0] = CreateCircleMesh(GJLevel.instance.killRange - (GJLevel.instance.perfect * size));
        circles[1] = CreateCircleMesh(GJLevel.instance.killRange + (GJLevel.instance.great * size));
        circles[2] = CreateCircleMesh(GJLevel.instance.killRange + (GJLevel.instance.good * size));
        circles[3] = CreateCircleMesh(GJLevel.instance.killRange + (GJLevel.instance.ok * size));
        circles[4] = CreateCircleMesh(GJLevel.instance.killRange + (GJLevel.instance.ok + 1.0f * size));
        //CreateCircleMesh(GJLevel.instance.spawnRange);
        //CreateCircleMesh(GJLevel.instance.killRange + (GJLevel.instance.ok+ 2.6f * size)); //arbitrary radius for last circle

        GameObject[] gameObjects = new GameObject[numObjects];
        childCircles = new CircleAnimation[numObjects];

        // create the child objects
        for (int i = 0; i < numObjects; ++i)
        {
            Vector3 position = transform.position;
            position.y = transform.position.y - 0.01f * i;
            gameObjects[i] = Instantiate(childPrefab, position, transform.rotation, transform);
            childCircles[i] = gameObjects[i].GetComponent<CircleAnimation>();
            childCircles[i].Init(circles[i], numObjects / numSubDivisions / animationSpeed);
        }

        childCircles[0].SetColor(Color.black);
        childCircles[1].SetColor(GJLevel.instance.perfectColor);
        childCircles[2].SetColor(GJLevel.instance.greatColor);
        childCircles[3].SetColor(GJLevel.instance.goodColor);
        childCircles[4].SetColor(GJLevel.instance.okColor);
    }

    Mesh CreateCircleMesh(float radius)
    {
        Mesh tmpMesh = new Mesh();

        List<Vector3> v = new List<Vector3>();
        List<int> t = new List<int>();
        float angleStep = 360.0f / (float)numSubDivisions;
        Quaternion q = Quaternion.Euler(0.0f, 0.0f, angleStep);

        // first tri
        v.Add(new Vector3(0.0f, 0.0f, 0.0f));
        v.Add(new Vector3(0.0f, radius, 0.0f));
        v.Add(q * v[1]);

        t.Add(0);
        t.Add(1);
        t.Add(2);

        // the rest of the tris
        for (int i = 0; i < numSubDivisions - 1; ++i)
        {
            t.Add(0);
            t.Add(v.Count - 1);
            t.Add(v.Count);
            v.Add(q * v[v.Count - 1]);
        }


        tmpMesh.vertices = v.ToArray();
        tmpMesh.triangles = t.ToArray();

        return tmpMesh;
    }

    void Update()
    {
        if (index >= numObjects)
            return;

        // iterate through the child object coroutines
        if (index == 0)
        {
            childCircles[index].StartCoroutine(childCircles[index].AddFace());
        }
        if (childCircles[index].doneLoading)
        {
            index++;
            if(index < numObjects)
                childCircles[index].StartCoroutine(childCircles[index].AddFace());
        }
    }
}
