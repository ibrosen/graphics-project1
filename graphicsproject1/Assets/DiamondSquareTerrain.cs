using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquareTerrain : MonoBehaviour
{
	
	public int mDivisions;				// Must be powers of 2. Default & max is 128.
	public float mSize;					// 100 to 10,000. Default is 1,000.
	public float iterRate;				// Between 0 and 1. Default is 0.5.
	public PointLight pointLight;       //Light source, here will mimick the sun

	Vector3[] mVerts;
	Color[] mColours;
	int mVertCount;
	float max, min;

	// Use this for initialization
	void Start()
	{
		
		GenerateTerrain();

		SetCameraPosition();

	}


	void Update()
	{

		// Get renderer component (in order to pass params to shader)
		MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

		// Pass updated light positions to shader
		renderer.material.SetColor("_PointLightColor", this.pointLight.color);
		renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
	}


	public void SetCameraPosition()
	{

		// Set camera position to slightly above the ground the in centre of the terrain
		GameObject.Find("Camera").transform.localPosition = new Vector3(-mSize/2.0f, max, -mSize/2.0f);

	}


	public void GenerateTerrain()
	{

		float mHeight = mSize / 4.0f; // Sets the height of the mesh. Proportional to mesh size.

		//CHANGE MVERTS TO BE 2D ARRAY
		mVertCount = (mDivisions + 1) * (mDivisions + 1);
		mVerts = new Vector3[mVertCount];
		mColours = new Color[mVertCount];
		Vector2[] UVs = new Vector2[mVertCount];
		//mdiv*mdiv is the number of faces, 2 triangles to a face, 3 ints for a triangle
		int[] tris = new int[2 * mDivisions * mDivisions * 3];

		float halfSize = mSize * 0.5f;
		float divisionSize = mSize / mDivisions;

		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;

		MeshCollider collide = new MeshCollider();
		collide = GetComponent<MeshCollider>();

		int triOffset = 0;

		for (int i = 0; i <= mDivisions; i++)
		{
			for (int j = 0; j <= mDivisions; j++)
			{
				mVerts[i * (mDivisions + 1) + j] = new Vector3(-halfSize + j * divisionSize, 0.0f, halfSize - i * divisionSize);
				UVs[i * (mDivisions + 1) + j] = new Vector2((float)(i / mDivisions), (float)(j / mDivisions));

				if (i < mDivisions && j < mDivisions)
				{
					int topLeft = i * (mDivisions + 1) + j;
					int botLeft = (i + 1) * (mDivisions + 1) + j;

					tris[triOffset] = topLeft;
					tris[triOffset + 1] = topLeft + 1;
					tris[triOffset + 2] = botLeft + 1;

					tris[triOffset + 3] = topLeft;
					tris[triOffset + 4] = botLeft + 1;
					tris[triOffset + 5] = botLeft;

					triOffset += 6;

				}
			}
		}

		mVerts[0].y = Random.Range(-mHeight, mHeight);
		mVerts[mDivisions].y = Random.Range(-mHeight, mHeight);
		mVerts[mVerts.Length - 1].y = Random.Range(-mHeight, mHeight);
		mVerts[mVerts.Length - 1 - mDivisions].y = Random.Range(-mHeight, mHeight);



		int iterations = (int)Mathf.Log(mDivisions, 2);
		int numSquares = 1;
		int squareSize = mDivisions;
		for (int i = 0; i < iterations; i++)
		{
			int row = 0;
			for (int j = 0; j < numSquares; j++)
			{
				int col = 0;
				for (int k = 0; k < numSquares; k++)
				{
					DiamondSquare(row, col, squareSize, mHeight);
					col += squareSize;
				}
				row += squareSize;
			}
			numSquares *= 2;
			squareSize /= 2;
			//can play around with this
			mHeight *= iterRate;
		}

		// CREATE COLOURS

		// Find max and min height of the terrain
		max = mVerts[0].y;
		min = mVerts[0].y;

		foreach (Vector3 v in mVerts)
		{
			if (v.y < min)
				min = v.y;
			
			if (v.y > max)
				max = v.y;
		}

		float heightRange = max - min;

		// Initalise terrain type percentages (relative to height)

		float snow = max - 0.15f * heightRange;
		float dirt = max - 0.3f * heightRange;
		float forest = max - 0.4f * heightRange;
		float green = max - 0.5f * heightRange;
		float sand = max - 0.6f * heightRange;
		Color forestGreen = new Color(0.0f / 255, 92.0f / 255, 9.0f / 255);
		Color lightGreen = new Color(1.0f/255, 166.0f/255, 17.0f/255);
		Color sandYellow = new Color(0.761f, 0.698f, 0.502f, 1);
		Color waterBlue = new Color(64.0f/255, 164.0f/255, 223.0f/255);

		//setting the colours of the vertices;
		for (int i = 0; i < mVertCount; i++)
		{

			//based on the height of the vertex, give it a certain colour
			if (mVerts[i].y >= snow)
				mColours[i] = Color.white;
			
			else if (mVerts[i].y >= dirt)
				mColours[i] = new Color(90.0f / 255, 77.0f / 255, 65.0f / 255);

			else if (mVerts[i].y >= green)
				mColours[i] = Color.Lerp(forestGreen,lightGreen, (forest-mVerts[i].y)/(forest-green));

			else if (mVerts[i].y >= sand)
				mColours[i] = sandYellow;

			else
				mColours[i] = waterBlue;
		}

		mesh.vertices = mVerts;
		mesh.uv = UVs;
		mesh.triangles = tris;
		mesh.colors = mColours;

		collide.sharedMesh = mesh;

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		// Create ocean layer
		GameObject.Find("Water").transform.localPosition = new Vector3(0.0f, sand, 0.0f);

	}


	void DiamondSquare(int row, int col, int size, float offset)
	{
		int halfSize = (int)(size * 0.5f);
		int topLeft = row * (mDivisions + 1) + col;
		int botLeft = (row + size) * (mDivisions + 1) + col;

		int midpoint = (int)(row + halfSize) * (mDivisions+1) + (int)(col + halfSize);
		mVerts[midpoint].y = (mVerts[topLeft].y + mVerts[botLeft].y + mVerts[topLeft + size].y + mVerts[botLeft + size].y)/4 + Random.Range(-offset, offset);

		mVerts[topLeft + halfSize].y = (mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[midpoint].y) / 3 + Random.Range(-offset, offset);
		mVerts[midpoint - halfSize].y = (mVerts[topLeft].y + mVerts[botLeft].y + mVerts[midpoint].y) / 3 + Random.Range(-offset, offset);
		mVerts[midpoint + halfSize].y = (mVerts[topLeft + size].y + mVerts[botLeft + size].y + mVerts[midpoint].y) / 3 + Random.Range(-offset, offset);
		mVerts[botLeft + halfSize].y = (mVerts[botLeft].y + mVerts[botLeft + size].y + mVerts[midpoint].y) / 3 + Random.Range(-offset, offset);
	}

}
