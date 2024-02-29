using System.Collections.Generic;
using UnityEngine;

public class Calc : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    private List<Vector3> vertice1 = new List<Vector3>();
    private List<Vector3> vertice2 = new List<Vector3>();

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
        if (Input.GetKeyDown(KeyCode.Space)) Destruction();
    }

    private void Destruction()
    {
        //Vector3 normal = new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
        Vector3 normal = new Vector3(0, 1, 0);
        Debug.Log(target.GetComponent<MeshFilter>().mesh.vertices.Length);
        for (int i = 0; i < target.GetComponent<MeshFilter>().mesh.vertices.Length; i++)
        {
            Vector3 v = target.GetComponent<MeshFilter>().mesh.vertices[i];
            det = -normal.x*pop.x - normal.y*pop.y - normal.z*pop.z;
            Debug.Log(v);
            sideDecider = normal.x * v.x + normal.y * v.y + normal.z * v.z + det;
            if (sideDecider == 0)
            {
                vertice1.Add(v);
                vertice2.Add(v);
            }

            if (sideDecider > 0)
            {
                vertice1.Add(v);
            }
            if (sideDecider < 0)
            {
                vertice2.Add(v);
            }
        }
    }
}
