using ProceduralGeometry;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// =============================================================================
public class MenuShapeCreator
{

     [MenuItem("ProceduralGeometry/Create/Circle")]
     // -------------------------------------------------------------------------
     static void CreateCircle()
     {

	  GameObject circleObj = new GameObject("Circle");
	  MeshFilter circleMesh = (MeshFilter)circleObj.AddComponent<MeshFilter>();
	  circleObj.AddComponent<MeshRenderer>();
	  circleMesh.sharedMesh = MeshBuilder.CreateCircle(20.0f);

	  Material material = Resources.Load<Material>("Default");
	  circleMesh.GetComponent<Renderer>().material = material;

	 // AssetDatabase.CreateAsset(circleMesh.sharedMesh, "Assets/ProceduralGeometry/Meshes/circle.asset");
     }

     [MenuItem("ProceduralGeometry/Create/Quad")]
     // -------------------------------------------------------------------------
     static void CreateQuad()
     {

	  GameObject circleObj = new GameObject("Quad");
	  MeshFilter circleMesh = (MeshFilter)circleObj.AddComponent<MeshFilter>();
	  circleObj.AddComponent<MeshRenderer>();
	  circleMesh.sharedMesh = MeshBuilder.CreateQuad(2,2);

	  Material material = Resources.Load<Material>("Default");
	  circleMesh.GetComponent<Renderer>().material = material;

	  // AssetDatabase.CreateAsset(circleMesh.sharedMesh, "Assets/ProceduralGeometry/Meshes/circle.asset");
     }

}
