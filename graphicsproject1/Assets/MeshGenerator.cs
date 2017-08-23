using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

	Vector3[] mVertices;
	Vector2[] mUVs;
	int[] mTriangles;

	// Use this for initialization
	void Start () {

		// Create a single plane
		mVertices = new Vector3[4];
		mUVs = new Vector2[4];
		mTriangles = new int[6]; // Need two triangles with 3 vertices each

		Mesh mesh = new Mesh ();
		GetComponent<MeshFilter>().mesh = mesh;

		mVertices [0] = new Vector3 (-1.0f, 0.0f, 1.0f);
		mVertices [1] = new Vector3 (1.0f, 0.0f, 1.0f);
		mVertices [2] = new Vector3 (-1.0f, 0.0f, -1.0f);
		mVertices [3] = new Vector3 (1.0f, 0.0f, -1.0f);

		mUVs [0] = new Vector2 (0.0f, 0.0f);
		mUVs [1] = new Vector2 (1.0f, 0.0f);
		mUVs [2] = new Vector2 (0.0f, 1.0f);
		mUVs [3] = new Vector2 (1.0f, 1.0f);

		mTriangles [0] = 0;
		mTriangles [1] = 1;
		mTriangles [2] = 3;

		mTriangles [3] = 0;
		mTriangles [4] = 3;
		mTriangles [5] = 2;

		// Assign vertices, UVs and triangles to our mesh
		mesh.vertices = mVertices;
		mesh.uv = mUVs;
		mesh.triangles = mTriangles;

		// Recalculate bounds and normals to update them to the ones we made
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();

	}
}
