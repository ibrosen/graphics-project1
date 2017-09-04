using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquareTerrain : MonoBehaviour
{
	
	public int numDiv = 128;			// Must be powers of 2. Default & max is 128.
	public float mSize = 10000.0f;		// 100 to 10,000. Default is 10,000.
	public float iterRate = 0.5f;		// Between 0 and 1. Default is 0.5.
	public PointLight pointLight;       //Light source, here will mimick the sun

	Vector3[] vertices;
	Color[] colours;
	int vertexCount;
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
		GameObject.Find("Camera").transform.localPosition = new Vector3(-mSize/2.0f, max + 50.0f, -mSize/2.0f);

	}


	public void GenerateTerrain()
	{

		float mHeight = mSize / 4.0f;	// Sets the height of the mesh. Proportional to mesh size.

		
		vertexCount = (numDiv + 1) * (numDiv + 1);			// Sets the number of vertices relative to the number of divisions set
		vertices = new Vector3[vertexCount];
		colours = new Color[vertexCount];
		Vector2[] UVs = new Vector2[vertexCount];
		int[] tris = new int[2 * numDiv * numDiv * 3];		// numDiv * numDiv is the number of faces, 2 triangles to a face, 3 ints for a triangle

		float halfSize = mSize * 0.5f;
		float divSize = mSize / numDiv;


		// Get mesh filter and mesh collider
		
		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;

		MeshCollider collide = new MeshCollider();
		collide = GetComponent<MeshCollider>();

		int offset = 0;

		for (int i = 0; i <= numDiv; i++)
		{
			for (int j = 0; j <= numDiv; j++)
			{
				vertices[i * (numDiv + 1) + j] = new Vector3(-halfSize + j * divSize, 0.0f, halfSize - i * divSize);
				UVs[i * (numDiv + 1) + j] = new Vector2((float)(i / numDiv), (float)(j / numDiv));

				if (i < numDiv && j < numDiv)
				{
					int topLeft = i * (numDiv + 1) + j;
					int botLeft = (i + 1) * (numDiv + 1) + j;

					// Triangle 1
					tris[offset] = topLeft;
					tris[offset + 1] = topLeft + 1;
					tris[offset + 2] = botLeft + 1;

					// Triangle 2
					tris[offset + 3] = topLeft;
					tris[offset + 4] = botLeft + 1;
					tris[offset + 5] = botLeft;

					offset += 6;

				}
			}
		}

		vertices[0].y = Random.Range(-mHeight, mHeight);
		vertices[numDiv].y = Random.Range(-mHeight, mHeight);
		vertices[vertices.Length - 1].y = Random.Range(-mHeight, mHeight);
		vertices[vertices.Length - 1 - numDiv].y = Random.Range(-mHeight, mHeight);


		// Iterates through vertices, setting their heights using the diamond square algorithm
		int iterations = (int)Mathf.Log(numDiv, 2);
		int numSquares = 1;
		int squareSize = numDiv;
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
			mHeight *= iterRate;
		}


		// CREATE COLOURS

		// Find max and min height of the terrain
		max = vertices[0].y;
		min = vertices[0].y;

		foreach (Vector3 v in vertices)
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
		for (int i = 0; i < vertexCount; i++)
		{

			//based on the height of the vertex, give it a certain colour
			if (vertices[i].y >= snow)
				colours[i] = Color.white;
			
			else if (vertices[i].y >= dirt)
				colours[i] = new Color(90.0f / 255, 77.0f / 255, 65.0f / 255);

			else if (vertices[i].y >= green)
				colours[i] = Color.Lerp(forestGreen,lightGreen, (forest-vertices[i].y)/(forest-green));

			else if (vertices[i].y >= sand)
				colours[i] = sandYellow;

			else
				colours[i] = waterBlue;
		}

		mesh.vertices = vertices;
		mesh.uv = UVs;
		mesh.triangles = tris;
		mesh.colors = colours;

		collide.sharedMesh = mesh;

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		// Create ocean layer
		GameObject.Find("Water").transform.localPosition = new Vector3(0.0f, sand, 0.0f);

	}


	void DiamondSquare(int row, int col, int size, float offset)
	{

		int halfSize = (int)(size * 0.5f);
		int topLeft = row * (numDiv + 1) + col;
		int botLeft = (row + size) * (numDiv + 1) + col;

		int midpoint = (int)(row + halfSize) * (numDiv+1) + (int)(col + halfSize);
		vertices[midpoint].y = (vertices[topLeft].y + vertices[botLeft].y + vertices[topLeft + size].y + vertices[botLeft + size].y)/4 + Random.Range(-offset, offset);

		// Define heights of diamond or square midpoint as mean height of its vertices, plus a random number within the specified range
		vertices[topLeft + halfSize].y = (vertices[topLeft].y + vertices[topLeft + size].y + vertices[midpoint].y) / 3 + Random.Range(-offset, offset);
		vertices[midpoint - halfSize].y = (vertices[topLeft].y + vertices[botLeft].y + vertices[midpoint].y) / 3 + Random.Range(-offset, offset);
		vertices[midpoint + halfSize].y = (vertices[topLeft + size].y + vertices[botLeft + size].y + vertices[midpoint].y) / 3 + Random.Range(-offset, offset);
		vertices[botLeft + halfSize].y = (vertices[botLeft].y + vertices[botLeft + size].y + vertices[midpoint].y) / 3 + Random.Range(-offset, offset);

	}

}
