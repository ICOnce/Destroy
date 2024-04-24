using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading;
using UnityEngine.UIElements;
using System.Runtime;
using System.Security;

public class Calc : MonoBehaviour
{
    Mesh mesh1;
    Mesh mesh2;

    private static PartTheSeas returning;
    private GameObject targeted;
    private bool done = true;
    private bool first = true;

    private List<Vector3> vertice1 = new List<Vector3>();
    private List<Vector3> vertice2 = new List<Vector3>();
    private List<Vector3> cuts = new List<Vector3>();

    private List<int> tris1 = new List<int>();
    private List<int> tris2 = new List<int>();

    private Vector3 pop;
    private Vector3 normal;
    private Vector3 sum;
    private float det;

    [SerializeField] private GameObject empty;
    private Vector3 oldScale;

    private Vector3 p1, p2, p3;
    private int length1;
    private int length2;

    private int depth;
    private int curDepth;

    private GameObject temp;
    private GameObject temp2;

    private float timer;
    private int impact;
    void Start()
    {
        timer = Time.realtimeSinceStartup;
        depth = 2;
        impact = 1;
        p1 = transform.position;
    }
    private void Update()
    {
        Debug.Log("First : " + first);
        Debug.Log(done);
        if (timer - Time.realtimeSinceStartup < -5)
        {
            //Destroy(gameObject);
        }

        if (done == true && first != true)
        {
            CreateShape();
            done = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        targeted = collision.gameObject;
        oldScale = collision.transform.localScale;
        p2 = collision.GetContact(0).point;
        pop = p2;
        p3 = p2 + new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10)).normalized;
        normal = Vector3.Cross(p2 - p1, p3 - p1);
        if(collision.gameObject.layer != 3 && done == true)
        {
            Debug.Log("Running");
            Vector3[] vertex = targeted.GetComponent<MeshFilter>().mesh.vertices;
            int[] tempTri = targeted.GetComponent<MeshFilter>().mesh.triangles;
            targeted.transform.TransformPoints(vertex);
            Thread thread = new (() =>
            {
                Debug.Log("Threading");
                Destruction(vertex, tempTri);
            });
            thread.Start();
            Debug.Log("thread : " + thread.IsAlive);
            done = false;
        }

        //Destroy(gameObject);
    }

    private void Destruction(Vector3[] vertex, int[] trian)
    {
        vertice1.Clear();
        vertice2.Clear();
        tris1.Clear();
        tris2.Clear();
        cuts.Clear();
        int trias1 = 0;
        int trias2 = 0;
        for (int i = 0; i < trian.Length; i+= 3)
        {
            Vector3 v1 = vertex[trian[i + 0]];
            Vector3 v2 = vertex[trian[i + 1]];
            Vector3 v3 = vertex[trian[i + 2]];
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

                    cuts.Add(Intersect(v3, v1));
                    cuts.Add(Intersect(v3, v2));
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

                    cuts.Add(Intersect(v2, v3));
                    cuts.Add(Intersect(v2, v1));
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

                    cuts.Add(Intersect(v1, v3));
                    cuts.Add(Intersect(v1, v2));
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

                    cuts.Add(Intersect(v2, v1));
                    cuts.Add(Intersect(v3, v1));
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

                    cuts.Add(Intersect(v2, v3));
                    cuts.Add(Intersect(v2, v1));
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

                    cuts.Add(Intersect(v3, v1));
                    cuts.Add(Intersect(v3, v2));
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

                    cuts.Add(v3);
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

                    cuts.Add(v2);
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

                    cuts.Add(v1);
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

                    cuts.Add(v1);
                    cuts.Add(v2);
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

                    cuts.Add(v1);
                    cuts.Add(v3);
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

                    cuts.Add(v2);
                    cuts.Add(v3);
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

                    cuts.Add(v3);
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

                    cuts.Add(v2);
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

                    cuts.Add(v1);
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

                    cuts.Add(v1);
                    cuts.Add(v2);
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

                    cuts.Add(v1);
                    cuts.Add(v3);
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

                    cuts.Add(v2);
                    cuts.Add(v3);
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

                    cuts.Add(v3);
                    cuts.Add(Intersect(v2, v1));
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

                    cuts.Add(v3);
                    cuts.Add(Intersect(v2, v1));
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

                    cuts.Add(v2);
                    cuts.Add(Intersect(v3, v1));
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

                    cuts.Add(v2);
                    cuts.Add(Intersect(v3, v1));
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

                    cuts.Add(v1);
                    cuts.Add(Intersect(v2, v1));
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

                    cuts.Add(v1);
                    cuts.Add(Intersect(v2, v1));
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

                    cuts.Add(v1);
                    cuts.Add(v2);
                    cuts.Add(v3);
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
        if (cuts.Count != 0)
        {
            foreach (Vector3 _vertex in cuts)
            {
                sum += _vertex;
            }
            sum /= cuts.Count;
            vertice1.AddRange(cuts);
            vertice2.AddRange(cuts);
            vertice1.Add(sum);
            vertice2.Add(sum);

            length1 = tris1.Count;
            length2 = tris2.Count;
            for (int i = 0; i < cuts.Count; i++)
            {
                for (int j = 0; j < length1; j += 3)
                {
                    for (int k = 0; k < cuts.Count; k++)
                    {
                        if (cuts[i] == vertice1[tris1[j]] && cuts[k] == vertice1[tris1[j + 1]])
                        {
                            if (!Exists(tris1[j], vertice1.Count - 1, tris1[j + 1], 0))
                            {
                                tris1.Add(tris1[j]);
                                tris1.Add(vertice1.Count - 1);
                                tris1.Add(tris1[j + 1]);
                            }
                        }

                        else if (cuts[i] == vertice1[tris1[j]] && cuts[k] == vertice1[tris1[j + 2]])
                        {
                            if (!Exists(tris1[j], tris1[j + 2], vertice1.Count - 1, 0))
                            {
                                tris1.Add(tris1[j]);
                                tris1.Add(tris1[j + 2]);
                                tris1.Add(vertice1.Count - 1);

                            }
                        }

                        else if (cuts[i] == vertice1[tris1[j + 1]] && cuts[k] == vertice1[tris1[j]])
                        {
                            if (!Exists(tris1[j + 1], vertice1.Count - 1, tris1[j], 0))
                            {
                                tris1.Add(tris1[j + 1]);
                                tris1.Add(tris1[j]);
                                tris1.Add(vertice1.Count - 1);
                            }
                        }

                        else if (cuts[i] == vertice1[tris1[j + 1]] && cuts[k] == vertice1[tris1[j + 2]])
                        {
                            if (!Exists(tris1[j + 1], tris1[j + 2], vertice1.Count - 1, 0))
                            {
                                tris1.Add(tris1[j + 1]);
                                tris1.Add(vertice1.Count - 1);
                                tris1.Add(tris1[j + 2]);
                            }
                        }

                        else if (cuts[i] == vertice1[tris1[j + 2]] && cuts[k] == vertice1[tris1[j]])
                        {
                            if (!Exists(tris1[j + 2], vertice1.Count - 1, tris1[j], 0))
                            {
                                tris1.Add(tris1[j + 2]);
                                tris1.Add(vertice1.Count - 1);
                                tris1.Add(tris1[j]);
                            }
                        }

                        else if (cuts[i] == vertice1[tris1[j + 2]] && cuts[k] == vertice1[tris1[j + 1]])
                        {
                            if (!Exists(tris1[j + 2], tris1[j + 1], vertice1.Count - 1, 0))
                            {
                                tris1.Add(tris1[j + 2]);
                                tris1.Add(tris1[j + 1]);
                                tris1.Add(vertice1.Count - 1);
                            }
                        }
                    }
                }

                for (int j = 0; j < length2; j += 3)
                {
                    for (int k = 0; k < cuts.Count; k++)
                    {
                        if (cuts[i] == vertice2[tris2[j]] && cuts[k] == vertice2[tris2[j + 1]])
                        {
                            if (!Exists(tris2[j], vertice2.Count - 1, tris2[j + 1], 1))
                            {
                                tris2.Add(tris2[j]);
                                tris2.Add(vertice2.Count - 1);
                                tris2.Add(tris2[j + 1]);
                            }
                        }

                        else if (cuts[i] == vertice2[tris2[j]] && cuts[k] == vertice2[tris2[j + 2]])
                        {
                            if (!Exists(tris2[j], tris2[j + 2], vertice2.Count - 1, 1))
                            {
                                tris2.Add(tris2[j]);
                                tris2.Add(tris2[j + 2]);
                                tris2.Add(vertice2.Count - 1);

                            }
                        }

                        else if (cuts[i] == vertice2[tris2[j + 1]] && cuts[k] == vertice2[tris2[j]])
                        {
                            if (!Exists(tris2[j + 1], vertice2.Count - 1, tris2[j], 1))
                            {
                                tris2.Add(tris2[j + 1]);
                                tris2.Add(tris2[j]);
                                tris2.Add(vertice2.Count - 1);
                            }
                        }

                        else if (cuts[i] == vertice2[tris2[j + 1]] && cuts[k] == vertice2[tris2[j + 2]])
                        {
                            if (!Exists(tris2[j + 1], tris2[j + 2], vertice2.Count - 1, 1))
                            {
                                tris2.Add(tris2[j + 1]);
                                tris2.Add(vertice2.Count - 1);
                                tris2.Add(tris2[j + 2]);
                            }
                        }

                        else if (cuts[i] == vertice2[tris2[j + 2]] && cuts[k] == vertice2[tris2[j]])
                        {
                            if (!Exists(tris2[j + 2], vertice2.Count - 1, tris2[j], 1))
                            {
                                tris2.Add(tris2[j + 2]);
                                tris2.Add(vertice2.Count - 1);
                                tris2.Add(tris2[j]);
                            }
                        }

                        else if (cuts[i] == vertice2[tris2[j + 2]] && cuts[k] == vertice2[tris2[j + 1]])
                        {
                            if (!Exists(tris2[j + 2], tris2[j + 1], vertice2.Count - 1, 1))
                            {
                                tris2.Add(tris2[j + 2]);
                                tris2.Add(tris2[j + 1]);
                                tris2.Add(vertice2.Count - 1);
                            }
                        }
                    }
                }
            }
        }
        done = true;
        first = false;
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

    void CreateShape()
    {
        mesh1 = new Mesh();
        mesh2 = new Mesh();
        if (curDepth == 0)
        {
            if (vertice1.Count != 0)
            {
                targeted.transform.InverseTransformPoints(vertice1.ToArray());
                mesh1.vertices = vertice1.ToArray();
                mesh1.triangles = tris1.ToArray();
                mesh1.RecalculateNormals();
                mesh1.RecalculateBounds();
                temp = Instantiate(empty, targeted.transform.position, Quaternion.identity);
                temp.GetComponent<MeshFilter>().mesh = mesh1;
                temp.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
                temp.AddComponent<MeshCollider>().convex = true;
                temp.GetComponent<Rigidbody>().mass = targeted.GetComponent<Rigidbody>().mass / 2;
                temp.transform.rotation = targeted.transform.rotation;
                temp.transform.localScale = oldScale;
                temp.GetComponent<Rigidbody>().AddForceAtPosition((p2 - p1).normalized * impact, p1);
            }
        }
        else
        {
            if (vertice1.Count != 0)
            {
                targeted.transform.InverseTransformPoints(vertice1.ToArray());
                mesh1.vertices = vertice1.ToArray();
                mesh1.triangles = tris1.ToArray();
                mesh1.RecalculateNormals();
                mesh1.RecalculateBounds();
                GameObject temp3 = Instantiate(empty, targeted.transform.position, Quaternion.identity);
                temp3.GetComponent<MeshFilter>().mesh = mesh1;
                temp3.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
                temp3.AddComponent<MeshCollider>().convex = true;
                temp3.GetComponent<Rigidbody>().mass = targeted.GetComponent<Rigidbody>().mass / 2;
                temp3.transform.rotation = targeted.transform.rotation;
                temp3.transform.localScale = oldScale;
                temp3.GetComponent<Rigidbody>().AddForceAtPosition((p2 - p1).normalized * impact, p1);
            }
        }

        

        if (vertice2.Count != 0)
        {
            /*for (global::System.Int32   = 0; i < length; i++)
            {
                
            }*/
            targeted.transform.InverseTransformPoints(vertice2.ToArray());
            mesh2.vertices = vertice2.ToArray();
            mesh2.triangles = tris2.ToArray();
            mesh2.RecalculateNormals();
            mesh2.RecalculateBounds();
            temp2 = Instantiate(empty, targeted.transform.position, Quaternion.identity);
            temp2.GetComponent<MeshFilter>().mesh = mesh2;
            temp2.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            temp2.AddComponent<MeshCollider>().convex = true;
            temp2.GetComponent<Rigidbody>().mass = targeted.GetComponent<Rigidbody>().mass/2;
            temp2.transform.rotation = targeted.transform.rotation;
            temp2.transform.localScale = oldScale;
            temp2.GetComponent<Rigidbody>().AddForceAtPosition((p2 - p1).normalized * impact, p2);
        }
        curDepth++;
        Destroy(targeted);
        if (!(curDepth >= depth))
        {
            normal = Vector3.Cross(p3 - p1, normal + new Vector3(Random.Range(-10,10), Random.Range(-10, 10), Random.Range(-10, 10)));
            Destruction(temp2.GetComponent<MeshFilter>().mesh.vertices, temp2.GetComponent<MeshFilter>().mesh.triangles);
            Destruction(temp.GetComponent<MeshFilter>().mesh.vertices, temp2.GetComponent<MeshFilter>().mesh.triangles);
        }
    }

    bool Exists(int p1, int p2, int p3, int identifier)
    {
        List<int> compare = new List<int>();
        int compareStart;
        if (identifier == 0)
        {
            compare = tris1;
            compareStart = length1;
        }
        else
        {
            compare = tris2;
            compareStart = length2;
        }
        for (int i = compareStart; i < compare.Count; i+= 3)
        {
            if (compare[i] == p1 && compare[i + 1] == p2 && compare[i + 2] == p3)
            {
                return true;
            }
            else if (compare[i] == p1 && compare[i + 1] == p3 && compare[i + 2] == p2)
            {
                return true;
            }
            else if (compare[i] == p2 && compare[i + 1] == p1 && compare[i + 2] == p3)
            {
                return true;
            }
            else if (compare[i] == p2 && compare[i + 1] == p3 && compare[i + 2] == p1)
            {
                return true;
            }
            else if (compare[i] == p3 && compare[i + 1] == p2 && compare[i + 2] == p1)
            {
                return true;
            }
            else if (compare[i] == p3 && compare[i + 1] == p1 && compare[i + 2] == p2)
            {
                return true;
            }
        }
        return false;
    }
}