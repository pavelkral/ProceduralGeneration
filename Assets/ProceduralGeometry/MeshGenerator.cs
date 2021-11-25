using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeometry
{
    

     public class MeshGenerator : MonoBehaviour
     {
	  private Mesh mesh;
	  MeshFilter meshFilter;

	  void Start()
	  {
	       mesh = new Mesh();
	       meshFilter = gameObject.GetComponent<MeshFilter>();
	       mesh = MeshBuilder.CreateRing(1f, 5f, 5,1,0f,1.57f);
	       mesh.RecalculateNormals();
	       meshFilter.mesh = mesh;
	  }


	  void Update()
	  {
	       generate();
	  }

	  [ContextMenu("Generate Ring")]
	  public void generate()
	  {
	       mesh = new Mesh();
	       meshFilter = gameObject.GetComponent<MeshFilter>();
	       mesh = MeshBuilder.CreateRing(1f, 5f, 5, 1, 0f, 1.57f);
	       mesh.RecalculateNormals();
	       meshFilter.mesh = mesh;
	  }
     }
}