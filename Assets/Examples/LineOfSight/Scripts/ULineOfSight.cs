
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
//[ExecuteInEditMode]

#if UNITY_EDITOR
[CustomEditor(typeof(ULineOfSight)), CanEditMultipleObjects]
class ULineOfSight_editor : Editor
{
     public override void OnInspectorGUI()
     {
	  ULineOfSight ufov = (ULineOfSight)target;
	  if (GUILayout.Button("Make Fow"))
	       ufov.EditorGenerateFov();
	  if (GUILayout.Button("Clear"))
	       ufov.hideFov();

	  DrawDefaultInspector();
     }
}
#endif

public class ULineOfSight : MonoBehaviour
{

     // [SerializeField] public Transform enemy;
     [Range(0, 180)]
     [SerializeField] private float halfAngle;
     [SerializeField] private float vievDistance;
     [SerializeField] private LayerMask layerMask;
     [SerializeField] public Mesh mesh ;
     [SerializeField] private Vector3 origin;
     [SerializeField] private Vector3 aimDirection;
     [SerializeField] private float startingAngle;
     [Range(10, 180)]
     [SerializeField] private int rayCount = 10;

     public MeshFilter mfilter;
     public List<GameObject> visibleTargets = new List<GameObject>();
     public LayerMask targetMask;
     public bool isMarked;

     public IEnumerator check;
     public LayerMask castMask;

     private void Start()
     {

	  mesh = new Mesh();
	  
	  mfilter.mesh = mesh;
	  castMask = LayerMask.GetMask("Obstacles", "Units", "Explosive", "Default");
	  origin = Vector3.zero;
	  startingAngle = GetAngleFromVectorFloat(transform.forward) + halfAngle;
	  check = FindTargetsWithDelay(0.2f);

	  StartCoroutine("FindTargetsWithDelay", .2f);
     }
     private void LateUpdate()
     {
	  SetAimDirection(transform.TransformDirection(Vector3.forward));
	  generateLineOfsight();
     }
     public void showFov()
     {

	 // generateFov();
	  generateLineOfsight();
     }
     public void EditorGenerateFov()
     {
	  mesh = new Mesh();
	  mfilter.mesh = mesh;
	  SetAimDirection(transform.TransformDirection(Vector3.forward));
	  SetOrigin(transform.position);
	  FindVisibleTargets();
	  //generateFov();
	  generateLineOfsight(); 
     }

     public void checkTargets()
     {

	  StartCoroutine(check);

     }
     public void hideFov()
     {
	  mesh.Clear();
	  mfilter.mesh = mesh;
     }

     public void SetFovAngle(float fov)
     {
	  this.halfAngle = fov / 2;
     }

     public void SetViewDistance(float viewDistance)
     {
	  this.vievDistance = viewDistance;
     }
     public void SetOrigin(Vector3 origin)
     {
	  this.origin = origin;
     }

     public void SetAimDirection(Vector3 aimDirection)
     {
	  this.aimDirection = aimDirection;
	  this.startingAngle = GetAngleFromVectorFloat(this.aimDirection) + halfAngle;
     }

     IEnumerator FindTargetsWithDelay(float delay)
     {
	  while (true)
	  {
	       yield return new WaitForSeconds(delay);
	       FindVisibleTargets();
	  }
     }

     void FindVisibleTargets()
     {
	  visibleTargets.Clear();
	  Collider[] targetsInViewRadius = Physics.OverlapSphere(origin, vievDistance, targetMask);

	  for (int i = 0; i < targetsInViewRadius.Length; i++)
	  {
	       GameObject target = targetsInViewRadius[i].gameObject;

	       Vector3 dirToTarget = (target.transform.position - origin).normalized;

	       if (Vector3.Angle(aimDirection, dirToTarget) < halfAngle)
	       {
		    Vector3 castPosition = transform.position + Vector3.up;

		    float dstToTarget = Vector3.Distance(origin, target.transform.position);

		    if (!Physics.Raycast(castPosition, dirToTarget, dstToTarget, castMask))
		    {
			 if (targetsInViewRadius[i].transform.CompareTag("NPC"))
			 {

			      //  Debug.Log("enemy in fov");

			      Debug.DrawRay(castPosition, dirToTarget * dstToTarget);
			      visibleTargets.Add(target);
			 }
			 // Debug.Log(target);
		    }
	       }
	  }
     }
     private void generateLineOfsight()
     {

	
	  float innerRadius = 1f;
	  float outerRadius = vievDistance;
	  float startAngle = (startingAngle - halfAngle * 2 )*Mathf.Deg2Rad;
	  float endAngle = (halfAngle * 2) * Mathf.Deg2Rad;
	  //int heightSegments = 1;
	  int segments = Mathf.Max(3, this.rayCount);
	  int heightsegments = Mathf.Max(1, 1);


	  List<Vector3> vertices = new List<Vector3>();
	  List<Vector3> normals = new List<Vector3>();
	  List<Vector2> uvs = new List<Vector2>();
	  List<int> indices = new List<int>();


	  float heihtradiusstep = ((outerRadius - innerRadius) / heightsegments);

	  float radius = innerRadius;

	  for (int j = 0; j <= heightsegments; j++)
	  {
	       for (int i = 0; i <= segments; i++)
	       {
		    float segment = startAngle + 1f * i / segments * endAngle;

		    //Vector3 vertex = new Vector3(radius * Mathf.Cos(segment),0f,radius * Mathf.Sin(segment));
		    Vector3 vertex = transform.InverseTransformPoint(origin + new Vector3(radius * Mathf.Cos(segment), 0f, radius * Mathf.Sin(segment)));
		    vertex.y = 0;
		    vertices.Add(vertex);


		    normals.Add(new Vector3(0f, 1f, 0f));
		    uvs.Add(new Vector2((vertex.x / outerRadius + 1) / 2, (vertex.z / outerRadius + 1) / 2));
	       }
	       radius += heihtradiusstep;
	  }

	  for (int j = 0; j < heightsegments; j++)
	  {
	       int thetaSegmentLevel = j * (segments + 1);

	       for (int i = 0; i < segments; i++)
	       {
		    int segment = i + thetaSegmentLevel;
		    int a = segment;
		    int b = segment + segments + 1;
		    int c = segment + segments + 2;
		    int d = segment + 1;
		    indices.Add(d); indices.Add(b); indices.Add(a);
		    indices.Add(d); indices.Add(c); indices.Add(b);
	       }
	  }

	
	  mesh.SetVertices(vertices);
	  mesh.SetNormals(normals);
	  mesh.SetUVs(0, uvs);
	  mesh.SetIndices(indices, MeshTopology.Triangles, 0);
	  mesh.RecalculateBounds();
	 
	  // fovCollider.sharedMesh = mesh;
     }
     private void generateFov()
     {

	  mesh.Clear();
	  int rayCount = this.rayCount;
	  float angle = startingAngle;
	  float angleIncrease = halfAngle * 2 / rayCount;

	  Vector3[] vertices = new Vector3[rayCount + 1 + 1];
	  Vector2[] uv = new Vector2[vertices.Length];
	  int[] triangles = new int[rayCount * 3];

	  vertices[0] = Vector3.zero;
	  uv[0] = new Vector2(0.5f, 0.5f);

	  int vertexIndex = 1;
	  int triangleIndex = 0;

	  for (int i = 0; i <= rayCount; i++)
	  {
	       Vector3 vertex;

	       if (!Physics.Raycast(origin, GetVectorFromAngle(angle), out RaycastHit raycastHit, vievDistance, layerMask))
	       {
		    vertex = origin + GetVectorFromAngle(angle) * vievDistance;
	       }
	       else
	       {
		    if (raycastHit.collider.CompareTag("NPC"))
		    {
			 // Debug.Log("hit");
		    }

		    vertex = raycastHit.point;
	       }
	       vertices[vertexIndex] = transform.InverseTransformPoint(vertex);
	       vertices[vertexIndex].y = 0;
	       //vertex;

	       uv[i + 1] = new Vector2(0.5f + (vertices[vertexIndex].x) / (2 * vievDistance), 0.5f + (vertices[vertexIndex].z) / (2 * vievDistance));
	 

	       if (i > 0)
	       {
		    triangles[triangleIndex + 0] = 0;
		    triangles[triangleIndex + 1] = vertexIndex - 1;
		    triangles[triangleIndex + 2] = vertexIndex;

		    triangleIndex += 3;
	       }

	       vertexIndex++;
	       angle -= angleIncrease;
	  }
	  // Debug.Log("Human = " + String.Join("",new List<Vector2>(uv).ConvertAll(i => i.ToString()).ToArray()));
	  mesh.vertices = vertices;
	  mesh.uv = uv;
	  mesh.triangles = triangles;
	  mesh.RecalculateNormals();
	  // fovCollider.sharedMesh = mesh;
     }

     public static Vector3 GetVectorFromAngle(float angle)
     {
	  // angle = 0 -> 360
	  float angleRad = angle * (Mathf.PI / 180f);
	  return new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));
     }

     public static float GetAngleFromVectorFloat(Vector3 dir)
     {
	  dir = dir.normalized;
	  float n = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
	  if (n < 0) n += 360;

	  return n;
     }
     private void OnDrawGizmos()
     {
	  Gizmos.color = Color.yellow;
	  // Gizmos.DrawWireSphere(transform.position, vievDistance);
	  Vector3 fovLineLeft = Quaternion.AngleAxis(halfAngle, transform.up) * this.aimDirection * vievDistance;
	  Vector3 fovLine2 = Quaternion.AngleAxis(-halfAngle, transform.up) * this.aimDirection * vievDistance;
	  //  Debug.Log("start  " + GetAngleFromVectorFloat(fovLineLeft));
	  Gizmos.DrawRay(origin, fovLineLeft);
	  Gizmos.DrawRay(origin, fovLine2);
	  Gizmos.color = Color.red;
	  //  Gizmos.DrawRay(origin, (enemy.position - origin).normalized * vievDistance);
	  Gizmos.color = Color.blue;
	  Gizmos.DrawRay(origin, this.aimDirection * vievDistance);


     }



}

