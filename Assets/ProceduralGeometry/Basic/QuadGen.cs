using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadGen : MonoBehaviour
{
     public float width = 1;
     public float height = 1;

     public void Start()
     {
	  //MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
	  //	meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
	  MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

	  Mesh mesh = new Mesh();

	  Vector3[] vertices = new Vector3[4]
	  {
			new Vector3(0, 0, 0),
			new Vector3(width, 0, 0),
			new Vector3(0,0, height),
			new Vector3(width, 0,  height)
	  };
	  

	  int[] tris = new int[6]
	  {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
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
			new Vector2(0, 0),
			new Vector2(1, 0),
			new Vector2(0, 1),
			new Vector2(1, 1)
	  };
	  mesh.vertices = vertices;
	  mesh.triangles = tris;
	  mesh.uv = uv;
	  mesh.normals = normals;
	  meshFilter.mesh = mesh;
     }
}