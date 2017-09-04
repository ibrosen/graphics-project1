using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour {
	
	public float moveSpeed = 1000; // Default is 1,000
	public float rollSpeed = 100; // Default is 100


	// Update is called once per frame
	void Update () {

		// KEY MOVEMENTS (forwards, backwards, right, left, roll)

		// Move camera forwards
		if (Input.GetKey(KeyCode.W)) {
			this.transform.localPosition += this.transform.forward * Time.deltaTime * moveSpeed;
			RestrictToBoundaries();

		// Move camera backwards
		} else if (Input.GetKey(KeyCode.S)) {
			this.transform.localPosition -= this.transform.forward * Time.deltaTime * moveSpeed;
			RestrictToBoundaries();

		// Move camera right
		} else if (Input.GetKey(KeyCode.D)) {
			this.transform.localPosition += this.transform.right * Time.deltaTime * moveSpeed;
			RestrictToBoundaries();

		// Move camera left
		} else if (Input.GetKey(KeyCode.A)) {
			this.transform.localPosition -= this.transform.right * Time.deltaTime * moveSpeed;
			RestrictToBoundaries();
		
		// Roll camera clockwise
		} else if (Input.GetKey(KeyCode.E)) {
			this.transform.RotateAround(transform.position, transform.forward, -1 * Time.deltaTime * rollSpeed);

		// Roll camera anticlockwise
		} else if (Input.GetKey(KeyCode.Q)) {
			this.transform.RotateAround(transform.position, transform.forward, Time.deltaTime * rollSpeed);

		}

		// MOUSE MOVEMENTS (pitch, yaw)

		// Move camera pitch up
		if (Input.GetAxis("Mouse Y") > 0) {
			this.transform.RotateAround(transform.position, transform.right, -1 * Time.deltaTime * rollSpeed);

		// Move camera pitch down
		} else if (Input.GetAxis("Mouse Y") < 0) {
			this.transform.RotateAround(transform.position, transform.right, Time.deltaTime * rollSpeed);
		
		}

		// Move camera yaw right
		if (Input.GetAxis("Mouse X") > 0) {
			this.transform.RotateAround(transform.position, transform.up, Time.deltaTime * rollSpeed);

		// Move camera yaw left
		} else if (Input.GetAxis("Mouse X") < 0) {
			this.transform.RotateAround(transform.position, transform.up, -1 * Time.deltaTime * rollSpeed);

		}
	
		// Prevent spinning and movement after collision
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

	}


	void RestrictToBoundaries() {

		// Get boundary of generated Terrain
		Vector3 pos;
		DiamondSquareTerrain terrain = GameObject.Find("Terrain").GetComponent<DiamondSquareTerrain>();
		float boundary = terrain.mSize / 2.0f;

		// Restrict movement to boundaries
		pos = this.transform.position;
		pos.x = Mathf.Clamp(pos.x, -boundary, boundary);
		pos.y = Mathf.Clamp(pos.y, -boundary, boundary);
		pos.z = Mathf.Clamp(pos.z, -boundary, boundary);
		this.transform.position = pos;

	}

}
