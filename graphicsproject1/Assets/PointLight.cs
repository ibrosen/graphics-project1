using UnityEngine;
using System.Collections;

public class PointLight : MonoBehaviour {

    public Color color;
    //public int radius;


    public Vector3 GetWorldPosition()
    {

        return this.transform.position;
    }
}
