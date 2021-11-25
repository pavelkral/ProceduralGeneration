
using UnityEngine;
using System.Collections;

namespace ProceduralGeometry
{

     [ExecuteInEditMode]
     [RequireComponent(typeof(MeshFilter))]
     public class MeshGizmoHelper : MonoBehaviour
     {

          Mesh mesh;
          [SerializeField] float size = 0.025f;
          void Start()
          {
               mesh = GetComponent<MeshFilter>().sharedMesh;
          }

          void OnDrawGizmos()
          {
               if (mesh == null) return;

               Gizmos.matrix = transform.localToWorldMatrix;
               Gizmos.color = Color.red;

               var vertices = mesh.vertices;
               var triangles = mesh.triangles;

               for (int i = 0, n = triangles.Length; i < n; i += 3)
               {
                    var a = vertices[triangles[i]];
                    var b = vertices[triangles[i + 1]];
                    var c = vertices[triangles[i + 2]];
                    Gizmos.DrawLine(a, b);
                    Gizmos.DrawLine(b, c);
                    Gizmos.DrawLine(c, a);
               }

               var uvs = mesh.uv;
               for (int i = 0, n = vertices.Length; i < n; i++)
               {
                    var v = vertices[i];
                    var uv = uvs[i];
                    Gizmos.color = new Color(uv.x, uv.y, 0f);
                    Gizmos.DrawSphere(v, size);
               }
          }

     }

}