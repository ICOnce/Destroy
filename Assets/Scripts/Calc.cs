using System.Collections.Generic;
using UnityEngine;

public class Calc : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    private List<Vector3> vertice1 = new List<Vector3>();
    private List<Vector3> vertice2 = new List<Vector3>();

    private List<int> tris1 = new List<int>();
    private List<int> tris2 = new List<int>();



    private Vector3 pop;
    private Vector3 normal;

    [SerializeField] private GameObject target;

    void Start()
    {
        mesh = new Mesh();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Debug.Log(Intersect(new Vector3(2, 10, 4), new Vector3(3, -1, 0)));
    }

    private void Destruction()
    {
        vertice1.Clear();
        vertice2.Clear();
        //Vector3 normal = new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
        normal = new Vector3(2, 4, 1);
        int trias1 = 0;
        int trias2 = 0;
        for (int i = 0; i < target.GetComponent<MeshFilter>().mesh.triangles.Length; i+= 3)
        {
            Vector3 v1 = target.GetComponent<MeshFilter>().mesh.vertices[target.GetComponent<MeshFilter>().mesh.triangles[i + 0]];
            Vector3 v2 = target.GetComponent<MeshFilter>().mesh.vertices[target.GetComponent<MeshFilter>().mesh.triangles[i + 1]];
            Vector3 v3 = target.GetComponent<MeshFilter>().mesh.vertices[target.GetComponent<MeshFilter>().mesh.triangles[i + 2]];
            if (!(SideDecider(v1) > 0 && SideDecider(v2) > 0 && SideDecider(v3) > 0 || SideDecider(v1) < 0 && SideDecider(v2) < 0 && SideDecider(v3) < 0))
            {
                //3: OOU
                if (SideDecider(v1) > 0 && SideDecider(v2) > 0 && SideDecider(v3) < 0)
                {
                    vertice1.Add(v1);
                    vertice1.Add(v2);
                    vertice1.Add(Intersect(v3, v1));
                    vertice1.Add(Intersect(v3, v2));

                    vertice2.Add(Intersect(v3, v1));
                    vertice2.Add(Intersect(v3, v2));
                    vertice2.Add(v3);
                }

                //4: OUO
                else if (SideDecider(v1) > 0 && SideDecider(v2) < 0 && SideDecider(v3) > 0)
                {

                }

                //5: UOO
                else if (SideDecider(v1) < 0 && SideDecider(v2) > 0 && SideDecider(v3) > 0)
                {

                }

                //6: OUU
                else if (SideDecider(v1) > 0 && SideDecider(v2) < 0 && SideDecider(v3) < 0)
                {

                }

                //7: UOU
                else if (SideDecider(v1) < 0 && SideDecider(v2) > 0 && SideDecider(v3) < 0)
                {

                }

                //8: UUO
                else if (SideDecider(v1) < 0 && SideDecider(v2) < 0 && SideDecider(v3) > 0)
                {

                }

                //9: OOP
                else if (SideDecider(v1) > 0 && SideDecider(v2) > 0 && SideDecider(v3) == 0) 
                {
                    
                }

                //10: OPO
                else if (SideDecider(v1) > 0 && SideDecider(v2) == 0 && SideDecider(v3) > 0)
                {

                }

                //11: POO
                else if (SideDecider(v1) == 0 && SideDecider(v2) > 0 && SideDecider(v3) > 0)
                {

                }

                //12: PPO
                else if (SideDecider(v1) == 0 && SideDecider(v2) == 0 && SideDecider(v3) > 0)
                {

                }

                //13: POP
                else if (SideDecider(v1) == 0 && SideDecider(v2) > 0 && SideDecider(v3) == 0)
                {

                }

                //14: OPP
                else if (SideDecider(v1) > 0 && SideDecider(v2) == 0 && SideDecider(v3) == 0)
                {

                }

                //15: UUP
                else if (SideDecider(v1) < 0 && SideDecider(v2) < 0 && SideDecider(v3) == 0)
                {

                }

                //16: UPU
                else if (SideDecider(v1) < 0 &&  SideDecider(v2) == 0 && SideDecider(v3) < 0)
                {

                }

                //17: PUU
                else if (SideDecider(v1) == 0 && SideDecider(v2) < 0 && SideDecider(v3) < 0)
                {

                }

                //18: PPU
                else if (SideDecider(v1) == 0 && SideDecider(v2) == 0 && SideDecider(v3) < 0)
                {

                }

                //19: PUP
                else if (SideDecider(v1) == 0 && SideDecider(v2) < 0 && SideDecider(v3) == 0)
                {

                }

                //20: UPP
                else if (SideDecider(v1) < 0 && SideDecider(v2) == 0 && SideDecider(v3) == 0)
                {

                }

                //21: OUP
                else if (SideDecider(v1) > 0 && SideDecider(v2) < 0 && SideDecider(v3) == 0)
                {

                }

                //22: UOP
                else if (SideDecider(v1) < 0 && SideDecider(v2) > 0 && SideDecider(v3) == 0)
                {

                }

                //23: OPU
                else if (SideDecider(v1) > 0 && SideDecider(v2) == 0 && SideDecider(v3) < 0)
                {

                }

                //24: UPO
                else if (SideDecider(v1) < 0 && SideDecider(v2) == 0 && SideDecider(v3) > 0)
                {

                }

                //25: POU
                else if (SideDecider(v1) == 0 && SideDecider(v2) > 0 && SideDecider(v3) < 0)
                {

                }

                //26: PUO
                else if (SideDecider(v1) == 0 && SideDecider(v2) < 0 && SideDecider(v3) > 0)
                {

                }

                //27: PPP
                else if (SideDecider(v1) == 0 && SideDecider(v2) == 0 && SideDecider(v3) == 0)
                {

                }
            }

            //1: OOO
            else if (SideDecider(v1) > 0 && SideDecider(v2) > 0 && SideDecider(v3) > 0)
            {
                vertice1.Add(v1);
                vertice1.Add(v2);
                vertice1.Add(v3);
                tris1.Add(trias1 + 0);
                tris1.Add(trias1 + 1);
                tris1.Add(trias1 + 2);
                trias1 += 3;
            }

            //2: UUU
            else if (SideDecider(v1) < 0 && SideDecider(v2) < 0 && SideDecider(v3) < 0) 
            {
                vertice2.Add(v1);
                vertice2.Add(v2);
                vertice2.Add(v3);
                tris2.Add(trias2 + 0);
                tris2.Add(trias2 + 1);
                tris2.Add(trias2 + 2);
                trias2 += 3;
            }
        }
    }

    public float SideDecider(Vector3 vertex)
    {
        //float det = -normal.x * pop.x - normal.y * pop.y - normal.z * pop.z;
        return normal.x * vertex.x + normal.y * vertex.y + normal.z * vertex.z /*+ det*/;
    }

    private Vector3 Intersect (Vector3 vert1, Vector3 vert2)
    {

        normal = new Vector3(2, 4, 1);

        float x0 = vert1.x;
        float y0 = vert1.y;
        float z0 = vert1.z;

        Vector3 lineR = vert2 - vert1 ;
        float t = -(normal.x * x0 + normal.y * y0 + normal.z * z0)/(normal.x * lineR.x + normal.y * lineR.y + normal.z * lineR.z);
        Vector3 intersect = new Vector3(
            x0 + lineR.x * t,
            y0 + lineR.y * t,
            z0 + lineR.z * t);
        return intersect;
    }
}
