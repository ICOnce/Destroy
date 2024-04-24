using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Specialized;

public class PartTheSeas
{

    public Vector3[] vert1;
    public Vector3[] vert2;
    public int[] trian1;
    public int[] trian2;


    public PartTheSeas(Func<Vector3[]> vert1, Func<Vector3[]> vert2, Func<int[]> tri1, Func<int[]> tri2)
    {

        this.vert1 = vert1.ConvertTo<Vector3[]>();
        
        this.vert2 = vert2.ConvertTo<Vector3[]>();
        this.trian1 = tri1.ConvertTo<int[]>();
        this.trian2 = tri2.ConvertTo<int[]>();
    }
}
