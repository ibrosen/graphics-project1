using UnityEngine;
using System.Collections;

public class SunMove : MonoBehaviour
{

	public float radius = 1000;		// the radius of the circle that the sun rotates around (sun follows the circumference)
    public float speed = 10;		// speed at which the sun moves

    // Update is called once per frame
    void Start()
    {
		
        this.transform.localScale = new Vector3(100, 100, 100);

    }


    void Update()
    {
		// Moves the sun in a circlular motion so it rises and sets
        this.transform.position = new Vector3(Mathf.Cos((Time.fixedTime/5)*speed), Mathf.Sin((Time.fixedTime/5)*speed), 0.0f) * radius;

    }
}
