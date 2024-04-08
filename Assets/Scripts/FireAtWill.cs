using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAtWill : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Camera cam;
    private Vector3 hitPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                hitPoint = hitInfo.point;
                GameObject temp = Instantiate(projectile, cam.transform.position, Quaternion.identity);
                temp.AddComponent<Rigidbody>().AddForce((hitPoint - cam.transform.position) * projectileSpeed);
                temp.GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }
}
