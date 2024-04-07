using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAtWill : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject temp = Instantiate(projectile, cam.transform.position, Quaternion.identity);
            temp.AddComponent<Rigidbody>().AddForce(cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20) - cam.transform.position) * projectileSpeed);
            temp.GetComponent<Rigidbody>().useGravity = false;
        }
    }
}
