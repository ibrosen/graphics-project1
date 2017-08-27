using UnityEngine;
using System.Collections;

public class SunMove : MonoBehaviour
{

    public float radius = 100000;
    public float speed = 1;
    // Update is called once per frame
    void Start()
    {
        this.transform.localScale = new Vector3(1000, 1000, 1000);
    }
    void Update()
    {
        this.transform.position = new Vector3(Mathf.Cos((Time.fixedTime/5)*speed), Mathf.Sin((Time.fixedTime/5)*speed), 0.0f) * radius; 
        //this.transform.localPosition += new Vector3(Mathf.Cos(Time.fixedTime/3), Mathf.Sin(Time.fixedTime/3), 0.0f) * radius;
        //this.transform.localRotation *= Quaternion.AngleAxis(spinSpeed * Time.deltaTime, Vector3.up);
    }
}
