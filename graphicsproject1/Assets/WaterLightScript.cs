using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLightScript : MonoBehaviour {

	public PointLight pointLight;       //Light source, here will mimick the sun

	Color[] mColours;

	// Update is called once per frame
	void Update () {

		// Get renderer component (in order to pass params to shader)
		MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

		// Pass updated light positions to shader
		renderer.material.SetColor("_PointLightColor", this.pointLight.color);
		renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());

	}

	void Start () {
		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;

		// create new colors array where the colors will be created.
		Color[] colors = new Color[vertices.Length];

		for (int i = 0; i < vertices.Length; i++)
			colors[i] = new Color(64.0f/255, 164.0f/255, 223.0f/255);

		// assign the array of colors to the Mesh.
		mesh.colors = colors;

	}
}
