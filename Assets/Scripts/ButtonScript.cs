using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private Button resetCube;
    [SerializeField] private GameObject cube;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;
    private float sizeScale;
    // Start is called before the first frame update
    void Update()
    {
        sizeScale = (int)slider.value;
        sliderText.text = "Scale of spawned cube (current: " + sizeScale + ")" ;
    }
    public void SpawnCube()
    {
        GameObject temp = Instantiate(cube, new Vector3(0, sizeScale/2, 0), Quaternion.identity);
        temp.transform.localScale *= sizeScale;
        temp.GetComponent<Rigidbody>().mass = sizeScale;
    }

    public void Reset()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("partedMesh");
        foreach (GameObject target in targets)
        {
            Destroy(target);
        }
    }
}
