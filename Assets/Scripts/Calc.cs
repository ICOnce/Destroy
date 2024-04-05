using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Calc : MonoBehaviour
{
    Mesh mesh1;
    Mesh mesh2;

    private List<Vector3> vertice1 = new List<Vector3>();
    private List<Vector3> vertice2 = new List<Vector3>();

    private List<int> tris1 = new List<int>();
    private List<int> tris2 = new List<int>();
    private List<GameObject> targets = new List<GameObject>();

    private Vector3 pop;
    private Vector3 normal;
    private float det;

    [SerializeField] private GameObject empty;

    private Vector3 p1, p2, p3;

    void Start()
    {
        p1 = transform.position;
        pop = p1;
    }
    private void OnCollisionEnter(Collision collision)
    {
        p2 = collision.GetContact(0).point;
        p3 = p2 + new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10)).normalized;
        normal = Vector3.Cross(p2 - p1, p3 - p1);
        Debug.Log(normal);
        if(collision.gameObject.layer != 3)
        {
            Destruction(collision.gameObject);
        }

        Destroy(gameObject);
    }

    private void Destruction(GameObject targeted)
    {
        mesh1 = new Mesh();
        mesh2 = new Mesh();
        vertice1.Clear();
        vertice2.Clear();
        tris1.Clear();
        tris2.Clear();
        int trias1 = 0;
        int trias2 = 0;
        for (int i = 0; i < targeted.GetComponent<MeshFilter>().mesh.triangles.Length; i+= 3)
        {
            Vector3 v1 = targeted.GetComponent<MeshFilter>().mesh.vertices[targeted.GetComponent<MeshFilter>().mesh.triangles[i + 0]];
            v1 += targeted.transform.position;
            Vector3 v2 = targeted.GetComponent<MeshFilter>().mesh.vertices[targeted.GetComponent<MeshFilter>().mesh.triangles[i + 1]];
            v2 += targeted.transform.position;
            Vector3 v3 = targeted.GetComponent<MeshFilter>().mesh.vertices[targeted.GetComponent<MeshFilter>().mesh.triangles[i + 2]];
            v3 += targeted.transform.position;
            if (!(SideDecider(v1) > 0 && SideDecider(v2) > 0 && SideDecider(v3) > 0 || SideDecider(v1) < 0 && SideDecider(v2) < 0 && SideDecider(v3) < 0))
            {
                //3: OOU
                if (SideDecider(v1) > 0 && SideDecider(v2) > 0 && SideDecider(v3) < 0)
                {
                    Debug.Log("OOU");
                    vertice1.Add(v1);
                    vertice1.Add(v2);
                    vertice1.Add(Intersect(v3, v1));
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;

                    vertice1.Add(v2);
                    vertice1.Add(Intersect(v3, v2));
                    vertice1.Add(Intersect(v3, v1));
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;

                    vertice2.Add(Intersect(v3, v1));
                    vertice2.Add(Intersect(v3, v2));
                    vertice2.Add(v3);
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;
                }

                //4: OUO
                else if (SideDecider(v1) > 0 && SideDecider(v2) < 0 && SideDecider(v3) > 0)
                {
                    Debug.Log("OUO");
                    vertice1.Add(Intersect(v1, v2));
                    vertice1.Add(v3);
                    vertice1.Add(v1);
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;

                    vertice2.Add(v2);
                    vertice2.Add(Intersect(v2, v3));
                    vertice2.Add(Intersect(v2, v1));
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;

                    vertice1.Add(v3);
                    vertice1.Add(Intersect(v2, v1));
                    vertice1.Add(Intersect(v2, v3));
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;
                }

                //5: UOO
                else if (SideDecider(v1) < 0 && SideDecider(v2) > 0 && SideDecider(v3) > 0)
                {
                    Debug.Log("UOO");
                    vertice2.Add(v1);
                    vertice2.Add(Intersect(v1, v2));
                    vertice2.Add(Intersect(v1, v3));
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;

                    vertice1.Add(Intersect(v3, v1));
                    vertice1.Add(v2);
                    vertice1.Add(v3);
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;

                    vertice1.Add(v2);
                    vertice1.Add(Intersect(v1, v3));
                    vertice1.Add(Intersect(v1, v2));
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;
                }

                //6: OUU
                else if (SideDecider(v1) > 0 && SideDecider(v2) < 0 && SideDecider(v3) < 0)
                {
                    Debug.Log("OUU");
                    vertice1.Add(v1);
                    vertice1.Add(Intersect(v2, v1));
                    vertice1.Add(Intersect(v3, v1));
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;

                    
                    vertice2.Add(Intersect(v3, v1));
                    vertice2.Add(v2);
                    vertice2.Add(v3);
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;

                    
                    vertice2.Add(Intersect(v1, v2));
                    vertice2.Add(v2);
                    vertice2.Add(Intersect(v1, v3));
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;
                }

                //7: UOU
                else if (SideDecider(v1) < 0 && SideDecider(v2) > 0 && SideDecider(v3) < 0)
                {
                    Debug.Log("UOU");
                    vertice2.Add(v3);
                    vertice2.Add(v1);
                    vertice2.Add(Intersect(v3, v2));
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;


                    vertice1.Add(Intersect(v2, v1));
                    vertice1.Add(v2);
                    vertice1.Add(Intersect(v2, v3));
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;


                    vertice2.Add(Intersect(v2, v3));
                    vertice2.Add(v1);
                    vertice2.Add(Intersect(v2, v1));
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;
                }

                //8: UUO
                else if (SideDecider(v1) < 0 && SideDecider(v2) < 0 && SideDecider(v3) > 0)
                {
                    Debug.Log("UUO");
                    vertice2.Add(v1);
                    vertice2.Add(v2);
                    vertice2.Add(Intersect(v3, v1));
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;


                    vertice2.Add(Intersect(v3, v2));
                    vertice2.Add(Intersect(v3, v1));
                    vertice2.Add(v2);
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;


                    vertice1.Add(Intersect(v3, v1));
                    vertice1.Add(Intersect(v3, v2));
                    vertice1.Add(v3);
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;
                }

                //9: OOP
                else if (SideDecider(v1) > 0 && SideDecider(v2) > 0 && SideDecider(v3) == 0) 
                {
                    Debug.Log("OOP");
                    vertice1.Add(v1);
                    vertice1.Add(v2);
                    vertice1.Add(v3);
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;
                }

                //10: OPO
                else if (SideDecider(v1) > 0 && SideDecider(v2) == 0 && SideDecider(v3) > 0)
                {
                    Debug.Log("OPO");
                    vertice1.Add(v1);
                    vertice1.Add(v2);
                    vertice1.Add(v3);
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;
                }

                //11: POO
                else if (SideDecider(v1) == 0 && SideDecider(v2) > 0 && SideDecider(v3) > 0)
                {
                    Debug.Log("POO");
                    vertice1.Add(v1);
                    vertice1.Add(v2);
                    vertice1.Add(v3);
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;
                }

                //12: PPO
                else if (SideDecider(v1) == 0 && SideDecider(v2) == 0 && SideDecider(v3) > 0)
                {
                    Debug.Log("PPO");
                    vertice1.Add(v1);
                    vertice1.Add(v2);
                    vertice1.Add(v3);
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;
                }

                //13: POP
                else if (SideDecider(v1) == 0 && SideDecider(v2) > 0 && SideDecider(v3) == 0)
                {
                    Debug.Log("POP");
                    vertice1.Add(v1);
                    vertice1.Add(v2);
                    vertice1.Add(v3);
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;
                }

                //14: OPP
                else if (SideDecider(v1) > 0 && SideDecider(v2) == 0 && SideDecider(v3) == 0)
                {
                    Debug.Log("OPP");
                    vertice1.Add(v1);
                    vertice1.Add(v2);
                    vertice1.Add(v3);
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;
                }

                //15: UUP
                else if (SideDecider(v1) < 0 && SideDecider(v2) < 0 && SideDecider(v3) == 0)
                {
                    Debug.Log("UUP");
                    vertice2.Add(v1);
                    vertice2.Add(v2);
                    vertice2.Add(v3);
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;
                }

                //16: UPU
                else if (SideDecider(v1) < 0 &&  SideDecider(v2) == 0 && SideDecider(v3) < 0)
                {
                    Debug.Log("UPU");
                    vertice2.Add(v1);
                    vertice2.Add(v2);
                    vertice2.Add(v3);
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;
                }

                //17: PUU
                else if (SideDecider(v1) == 0 && SideDecider(v2) < 0 && SideDecider(v3) < 0)
                {
                    Debug.Log("PUU");
                    vertice2.Add(v1);
                    vertice2.Add(v2);
                    vertice2.Add(v3);
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;
                }

                //18: PPU
                else if (SideDecider(v1) == 0 && SideDecider(v2) == 0 && SideDecider(v3) < 0)
                {
                    Debug.Log("PPU");
                    vertice2.Add(v1);
                    vertice2.Add(v2);
                    vertice2.Add(v3);
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;
                }

                //19: PUP
                else if (SideDecider(v1) == 0 && SideDecider(v2) < 0 && SideDecider(v3) == 0)
                {
                    Debug.Log("PUP");
                    vertice2.Add(v1);
                    vertice2.Add(v2);
                    vertice2.Add(v3);
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;
                }

                //20: UPP
                else if (SideDecider(v1) < 0 && SideDecider(v2) == 0 && SideDecider(v3) == 0)
                {
                    Debug.Log("UPP");
                    vertice2.Add(v1);
                    vertice2.Add(v2);
                    vertice2.Add(v3);
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;
                }

                //21: OUP
                else if (SideDecider(v1) > 0 && SideDecider(v2) < 0 && SideDecider(v3) == 0)
                {
                    Debug.Log("OUP");
                    vertice1.Add(v3);
                    vertice1.Add(v1);
                    vertice1.Add(Intersect(v1, v2));
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;

                    vertice2.Add(v2);
                    vertice2.Add(v3);
                    vertice2.Add(Intersect(v2, v1));
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;
                }

                //22: UOP
                else if (SideDecider(v1) < 0 && SideDecider(v2) > 0 && SideDecider(v3) == 0)
                {
                    Debug.Log("UOP");
                    vertice2.Add(v3);
                    vertice2.Add(v1);
                    vertice2.Add(Intersect(v1, v2));
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;

                    vertice1.Add(v2);
                    vertice1.Add(v3);
                    vertice1.Add(Intersect(v2, v1));
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;
                }

                //23: OPU
                else if (SideDecider(v1) > 0 && SideDecider(v2) == 0 && SideDecider(v3) < 0)
                {
                    Debug.Log("OPU");
                    vertice1.Add(v1);
                    vertice1.Add(v2);
                    vertice1.Add(Intersect(v1, v3));
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;

                    vertice2.Add(v2);
                    vertice2.Add(v3);
                    vertice2.Add(Intersect(v3, v1));
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;
                }

                //24: UPO
                else if (SideDecider(v1) < 0 && SideDecider(v2) == 0 && SideDecider(v3) > 0)
                {
                    Debug.Log("UPO");
                    vertice2.Add(v1);
                    vertice2.Add(v2);
                    vertice2.Add(Intersect(v1, v3));
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;

                    vertice1.Add(v2);
                    vertice1.Add(v3);
                    vertice1.Add(Intersect(v3, v1));
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;
                }

                //25: POU
                else if (SideDecider(v1) == 0 && SideDecider(v2) > 0 && SideDecider(v3) < 0)
                {
                    Debug.Log("POU");
                    vertice1.Add(v3);
                    vertice1.Add(v1);
                    vertice1.Add(Intersect(v1, v2));
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;

                    vertice2.Add(v2);
                    vertice2.Add(v3);
                    vertice2.Add(Intersect(v2, v1));
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;
                }

                //26: PUO
                else if (SideDecider(v1) == 0 && SideDecider(v2) < 0 && SideDecider(v3) > 0)
                {
                    Debug.Log("PUO");
                    vertice2.Add(v3);
                    vertice2.Add(v1);
                    vertice2.Add(Intersect(v1, v2));
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;

                    vertice1.Add(v2);
                    vertice1.Add(v3);
                    vertice1.Add(Intersect(v2, v1));
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;
                }

                //27: PPP
                else if (SideDecider(v1) == 0 && SideDecider(v2) == 0 && SideDecider(v3) == 0)
                {
                    Debug.Log("PPP");
                    vertice1.Add(v1);
                    vertice1.Add(v2);
                    vertice1.Add(v3);
                    tris1.Add(trias1 + 0);
                    tris1.Add(trias1 + 1);
                    tris1.Add(trias1 + 2);
                    trias1 += 3;

                    vertice2.Add(v1);
                    vertice2.Add(v2);
                    vertice2.Add(v3);
                    tris2.Add(trias2 + 0);
                    tris2.Add(trias2 + 1);
                    tris2.Add(trias2 + 2);
                    trias2 += 3;
                }
            }

            //1: OOO
            else if (SideDecider(v1) > 0 && SideDecider(v2) > 0 && SideDecider(v3) > 0)
            {
                Debug.Log("OOO");
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
                Debug.Log("UUU");
                vertice2.Add(v1);
                vertice2.Add(v2);
                vertice2.Add(v3);
                tris2.Add(trias2 + 0);
                tris2.Add(trias2 + 1);
                tris2.Add(trias2 + 2);
                trias2 += 3;
            }
        }
        CreateShape(targeted);

    }

    public float SideDecider(Vector3 vertex)
    {
        det = -normal.x * pop.x - normal.y * pop.y - normal.z * pop.z;
        return normal.x * vertex.x + normal.y * vertex.y + normal.z * vertex.z + det;
    }

    private Vector3 Intersect (Vector3 vert1, Vector3 vert2)
    {

        float x0 = vert1.x;
        float y0 = vert1.y;
        float z0 = vert1.z;

        Vector3 lineR = vert2 - vert1 ;
        det = -normal.x * pop.x - normal.y * pop.y - normal.z * pop.z;
        float t = -(normal.x * x0 + normal.y * y0 + normal.z * z0+det)/(normal.x * lineR.x + normal.y * lineR.y + normal.z * lineR.z);
        Vector3 intersect = new Vector3(
            x0 + lineR.x * t,
            y0 + lineR.y * t,
            z0 + lineR.z * t);
        return intersect;
    }

    void CreateShape(GameObject targeted)
    {
        Vector3 subtract = targeted.transform.position;
        if (vertice1.Count != 0)
        {
            for (int i = 0 ; i < vertice1.Count; i++)
            {
                vertice1[i] -= subtract;
            }
            mesh1.vertices = vertice1.ToArray();
            mesh1.triangles = tris1.ToArray();
            mesh1.RecalculateNormals();
            mesh1.RecalculateBounds();
            GameObject temp = Instantiate(empty, targeted.transform.position, Quaternion.identity);
            temp.GetComponent<MeshFilter>().mesh = mesh1;
            temp.AddComponent<MeshCollider>().convex = true;
            temp.GetComponent<Rigidbody>().mass = targeted.GetComponent<Rigidbody>().mass / 2;
        }

        if (vertice2.Count != 0)
        {
            /*for (global::System.Int32   = 0; i < length; i++)
            {
                
            }*/
            for (int i = 0; i <  vertice2.Count; i++)
            {
                vertice2[i] -= subtract;
            }
            mesh2.vertices = vertice2.ToArray();
            mesh2.triangles = tris2.ToArray();
            mesh2.RecalculateNormals();
            mesh2.RecalculateBounds();
            GameObject temp2 = Instantiate(empty, targeted.transform.position, Quaternion.identity);
            temp2.GetComponent<MeshFilter>().mesh = mesh2;
            temp2.AddComponent<MeshCollider>().convex = true;
            temp2.GetComponent<Rigidbody>().mass = targeted.GetComponent<Rigidbody>().mass/2;
        }
        Destroy(targeted);
    }
}
