using System.Collections.Generic;
using UnityEngine;

public class Calc : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    private List<float> vertice1 = new List<float>();
    private List<float> vertice2 = new List<float>();

    private float sideDecider;
    private float det;

    Vector3 pop;

    [SerializeField] private GameObject target;

    void Start()
    {
        mesh = new Mesh();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) Destruction();
    }

    private void Destruction()
    {
        Vector3 normal = new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
        for (int i = 0; i < target.GetComponent<Mesh>().vertices.Length; i++)
        {
            Vector3 v = target.GetComponent<Mesh>().vertices[i];
            det = -normal.x*pop.x - normal.y*pop.y - normal.z*pop.z;
            sideDecider = normal.x * v.x + normal.y * v.y + normal.z * v.z + det;

            if (sideDecider > 0)
            {
                vertice1.Add(sideDecider);
            }
            else
            {
                vertice2.Add(sideDecider);  
            }
        }
        target.GetComponent<Mesh>().vertices[1] = new Vector3(1,1,1);
    }
}
