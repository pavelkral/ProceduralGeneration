using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SimpleCircle : MonoBehaviour
{

     // Use this for initialization
     void Start()
     {
	  //GetComponent<MeshFilter>().mesh = GenerateCircle(resolution);
	  //GetComponent<MeshFilter>().mesh = GenerateCircle(resolution);
	  MakeCircle(12);
     }

     // Update is called once per frame
     void Update()
     {

     }
     public void MakeCircle(int numOfPoints)
     {
	  float angleStep = 360.0f / (float)numOfPoints;
	  List<Vector3> vertexList = new List<Vector3>();
	  List<int> triangleList = new List<int>();
	  Quaternion quaternion = Quaternion.Euler(0.0f, 0.0f, angleStep);
	  // Make first triangle.
	  vertexList.Add(new Vector3(0.0f, 0.0f, 0.0f));  // 1. Circle center.
	  vertexList.Add(new Vector3(0.0f, 1.5f, 0.0f));  // 2. First vertex on circle outline (radius = 0.5f)
	  vertexList.Add(quaternion * vertexList[1]);     // 3. First vertex on circle outline rotated by angle)
							  // Add triangle indices.
	  triangleList.Add(0);
	  triangleList.Add(1);
	  triangleList.Add(2);

	  for (int i = 0; i < numOfPoints - 1; i++)
	  {
	       triangleList.Add(0);                      // Index of circle center.
	       triangleList.Add(vertexList.Count - 1);
	       triangleList.Add(vertexList.Count);
	       vertexList.Add(quaternion * vertexList[vertexList.Count - 1]);
	  }
	  //Mesh mesh = new Mesh();
	  Mesh mesh = GetComponent<MeshFilter>().mesh;
	  mesh.vertices = vertexList.ToArray();
	  mesh.triangles = triangleList.ToArray();
     }
     public void MakeUVCircle(int numOfPoints)
     {
	  // The more verts, the more 'round' the circle appears
	  // It's hard coded here but it would better if you could pass it in as an argument to this function
	  int numVerts = 41;

	  Mesh plane = new Mesh();
	  Vector3[] verts = new Vector3[numVerts];
	  Vector2[] uvs = new Vector2[numVerts];
	  int[] tris = new int[(numVerts * 3)];

	  // The first vert is in the center of the triangle
	  verts[0] = Vector3.zero;
	  uvs[0] = new Vector2(0.5f, 0.5f);

	  float angle = 360.0f / (float)(numVerts - 1);

	  for (int i = 1; i < numVerts; ++i)
	  {
	       verts[i] = Quaternion.AngleAxis(angle * (float)(i - 1), Vector3.back) * Vector3.up;

	       float normedHorizontal = (verts[i].x + 1.0f) * 0.5f;
	       float normedVertical = (verts[i].x + 1.0f) * 0.5f;
	       uvs[i] = new Vector2(normedHorizontal, normedVertical);
	  }

	  for (int i = 0; i + 2 < numVerts; ++i)
	  {
	       int index = i * 3;
	       tris[index + 0] = 0;
	       tris[index + 1] = i + 1;
	       tris[index + 2] = i + 2;
	  }

	  // The last triangle has to wrap around to the first vert so we do this last and outside the lop
	  var lastTriangleIndex = tris.Length - 3;
	  tris[lastTriangleIndex + 0] = 0;
	  tris[lastTriangleIndex + 1] = numVerts - 1;
	  tris[lastTriangleIndex + 2] = 1;

	  plane.vertices = verts;
	  plane.triangles = tris;
	  plane.uv = uvs;

	  plane.RecalculateNormals();



     }
}