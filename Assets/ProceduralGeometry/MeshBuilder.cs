using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProceduralGeometry
{

     public static class MeshBuilder
     {
	  static public Mesh CreateQuad(float width = 1, float height = 1)
	  {

	       Mesh mesh = new Mesh();

	       Vector3[] vertices = new Vector3[4]
	       {
			new Vector3(-width/2, 0, -height/2),
			new Vector3(-width/2, 0,height/2),
			new Vector3(width/2,0, height/2),
			new Vector3(width/2, 0, -height/2)
	       };


	       int[] tris = new int[6]
	       {

		  0, 1, 3,
		  2, 3, 1
	       };

	       Vector3[] normals = new Vector3[4]
	       {
			-Vector3.forward,
			-Vector3.forward,
			-Vector3.forward,
			-Vector3.forward
	       };

	       Vector2[] uv = new Vector2[4]
	       {
			new Vector2(-0.5f, -0.5f),
			new Vector2(-0.5f, 0.5f),
			new Vector2(0.5f, 0.5f),
			new Vector2(0.5f, -0.5f)
	       };
	       mesh.vertices = vertices;
	       mesh.triangles = tris;
	       mesh.uv = uv;
	       mesh.normals = normals;


	       mesh.RecalculateNormals();

	       return mesh;
	  }
	  static public Mesh CreateCircle(float radius, int segments = 21)
	  {

	       int numVerts = segments;
	       Mesh mesh = new Mesh();
	       Vector3[] verts = new Vector3[numVerts];
	       Vector2[] uvs = new Vector2[numVerts];
	       int[] tris = new int[(numVerts * 3)];

	       verts[0] = Vector3.zero;
	       uvs[0] = new Vector2(0.5f, 0.5f);

	       float angle = 360.0f / (float)(numVerts - 1);

	       for (int i = 1; i < numVerts; ++i)
	       {
		    verts[i] = Quaternion.AngleAxis(angle * (float)(i - 1), Vector3.up) * Vector3.back;

		    float normedHorizontal = (verts[i].x + 1.0f) * 0.5f;
		    float normedVertical = (verts[i].z + 1.0f) * 0.5f;
		    uvs[i] = new Vector2(normedHorizontal, normedVertical);
		    //uvs[i] = new Vector2(0.5f + (verts[i].x) / (2 * radius),0.5f + (verts[i].y) / (2 * radius));
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

	       mesh.vertices = verts;
	       mesh.triangles = tris;
	       mesh.uv = uvs;

	       mesh.RecalculateNormals();

	       return mesh;
	  }
	 

	  public static Mesh CreateRing(float innerRadius, float outerRadius, int heightSegments, int radiusSegments, float startAngle = 0f, float endAngle = Mathf.PI * 2f)
	  {
	       heightSegments = Mathf.Max(3, heightSegments);
	       radiusSegments = Mathf.Max(1, radiusSegments);

	       List<Vector3> vertices = new List<Vector3>();
	       List<Vector3> normals = new List<Vector3>();
	       List<Vector2> uvs = new List<Vector2>();
	       List<int> indices = new List<int>();


	       float radiusStep = ((outerRadius - innerRadius) / radiusSegments);
	       float radius = innerRadius;

	       for (int j = 0; j <= radiusSegments; j++)
	       {
		    for (int i = 0; i <= heightSegments; i++)
		    {
			 float segment = startAngle + 1f * i / heightSegments * endAngle;

			 Vector3 vertex = new Vector3(
			     radius * Mathf.Cos(segment),
			     0f,
			     radius * Mathf.Sin(segment)
			 );

			 vertices.Add(vertex);
			 normals.Add(new Vector3(0f, 1f, 0f));
			 uvs.Add(new Vector2((vertex.x / outerRadius + 1) / 2, (vertex.z / outerRadius + 1) / 2));
		    }
		    radius += radiusStep;
	       }

	       for (int j = 0; j < radiusSegments; j++)
	       {
		    int thetaSegmentLevel = j * (heightSegments + 1);

		    for (int i = 0; i < heightSegments; i++)
		    {
			 int segment = i + thetaSegmentLevel;
			 int a = segment;
			 int b = segment + heightSegments + 1;
			 int c = segment + heightSegments + 2;
			 int d = segment + 1;
			 indices.Add(d); indices.Add(b); indices.Add(a);
			 indices.Add(d); indices.Add(c); indices.Add(b);
		    }
	       }

	       var mesh = new Mesh();
	       mesh.SetVertices(vertices);
	       mesh.SetNormals(normals);
	       mesh.SetUVs(0, uvs);
	       mesh.SetIndices(indices, MeshTopology.Triangles, 0);
	       mesh.RecalculateBounds();
	       return mesh;
	  }

	  

     }
}


