using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquareTerrain : MonoBehaviour
{
	public GameObject maincamera;
	public int mDivisions;				// Must be powers of 2. Default & max is 128.
	public float mSize;					// 100 to 10,000. Default is 1,000.
	public float iterRate;				// Between 0 and 1. Default is 0.5.
	public PointLight pointLight;       //Light source, here will mimick the sun

	Vector3[] mVerts;
	Color[] mColours;
	int mVertCount;


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

		// Get the centre vector of the terrain (0, y, 0)
		int centreVertIndex = mVertCount / 2;
		Vector3 centreVert = mVerts[centreVertIndex];

		// Set camera position to slightly above the ground the in centre of the terrain
		maincamera.transform.localPosition = new Vector3(0.0f, centreVert.y + 10.0f, 0.0f);

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
		//setting the colours of the vertices;
		for(int i = 0; i < mVertCount; i++)
		{
			//based on the height of the vertex, give it a certain colour
			if (mVerts[i].y > 0)
				mColours[i] = Color.black;
			else
				mColours[i] = Color.white;
		}


		mesh.vertices = mVerts;
		mesh.uv = UVs;
		mesh.triangles = tris;
		//mesh.colors = mColours;

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
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
