using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour {

	public float moveSpeed; // Set to 10
	public float rollSpeed; // Set to 50

	// Use this for initialization
	void Start()
	{

		// Set initial location of camera

		// Get terrain variables
//		float terrainHeight = DiamondSquareTerrain.mSize / 4.0f;
//		Vector3 terrainPosition = GameObject.Find("Terrain").transform.localPosition;
//
////		this.transform.localPosition = terrainPosition + new Vector3(0.0f, 2.0f, 0.0f);
//		this.transform.localPosition = Vector3.zero + new Vector3(0.0f, terrainHeight, 0.0f);

//		GameObject.Find("Terrain").

	}

	// Update is called once per frame
	void Update () {

		// MOUSE MOVEMENTS (pitch, yaw)

		// Move camera pitch up
		if (Input.GetAxis("Mouse Y") > 0) {
			this.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * rollSpeed, -1 * this.transform.right);

		// Move camera pitch down
		} else if (Input.GetAxis("Mouse Y") < 0) {
			this.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * rollSpeed, this.transform.right);
		
		}

		// Move camera yaw right
		if (Input.GetAxis("Mouse X") > 0) {
			this.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * rollSpeed, this.transform.up);

		// Move camera yaw left
		} else if (Input.GetAxis("Mouse X") < 0) {
			this.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * rollSpeed, -1 * this.transform.up);

		}


		// KEY MOVEMENTS (forwards, backwards, right, left, roll)

		// Move camera forwards
		if (Input.GetKey(KeyCode.W)) {
			this.transform.localPosition += this.transform.forward * Time.deltaTime * moveSpeed;

		// Move camera backwards
		} else if (Input.GetKey(KeyCode.S)) {
			this.transform.localPosition -= this.transform.forward * Time.deltaTime * moveSpeed;
		
		// Move camera right
		} else if (Input.GetKey(KeyCode.D)) {
			this.transform.localPosition += this.transform.right * Time.deltaTime * moveSpeed;

		// Move camera left
		} else if (Input.GetKey(KeyCode.A)) {
			this.transform.localPosition -= this.transform.right * Time.deltaTime * moveSpeed;
		
		// Roll camera clockwise
		} else if (Input.GetKey(KeyCode.E)) {
			this.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * rollSpeed, -1 * this.transform.forward);

		// Roll camera anticlockwise
		} else if (Input.GetKey(KeyCode.Q)) {
			this.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * rollSpeed, this.transform.forward);

		}

		/* TO DO:
			- function to start camera a few units above the ground/sea level in the
			  centre of the terrain with correct angle
			- stop camera from going through terrain
		*/

	}
}
