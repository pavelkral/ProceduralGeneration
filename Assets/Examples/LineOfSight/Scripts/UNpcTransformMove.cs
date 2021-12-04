using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Orientation
{
     public class UNpcTransformMove : MonoBehaviour
     {
	  public float Speed = 2f;
	  public float Radius = 2f;
	  public float moveValue;
	  public GameObject player;
	  public TMP_Text m_TextComponent;
	  public Vector3 pos;

	  void Start()
	  {
	       //Debug.Log("Local position: " + transform.localPosition);
	       //Debug.Log("Local rotation: " + transform.localRotation);
	       //Debug.Log("Local rotation (E): " + transform.localEulerAngles);
	       //Debug.Log("Local scale: " + transform.localScale);
	       //Debug.Log("Global position: " + transform.position);
	       //Debug.Log("Global rotation: " + transform.rotation);
	       //Debug.Log("Global rotation (E): " + transform.eulerAngles);
	       //Debug.Log(" Mossy scale: " + transform.lossyScale + "!!!(Read - only)!!!");

	  }

	  void Update()
	  {
	       Vector3 direction = player.transform.position - transform.position;
	       //
	       // Debug.DrawRay(transform.position, direction, Color.green);
	       // Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
	       Vector3 forward = transform.TransformDirection(Vector3.forward);
	       Vector3 toOther = player.transform.position - transform.position;
	       Debug.DrawRay(transform.position, toOther * 3, Color.cyan);
	       Debug.DrawRay(transform.position, forward * 3, Color.red);

	       var dot = Vector3.Dot(forward, toOther);
	       // Divide the dot by the product of the magnitudes of the vectors
	       dot = dot / (forward.magnitude * toOther.magnitude);
	       //Get the arc cosin of the angle, you now have your angle in radians 
	       var acos = Mathf.Acos(dot);
	       //Multiply by 180/Mathf.PI to convert to degrees
	       var angle = acos * 180 / Mathf.PI;
	       //Congrats, you made it really hard on yourself.
	       //   print(angle);

	       if (Vector3.Dot(forward, toOther) < 0)
	       {
		    // print("The other transform is behind me!");
		    // m_TextComponent.text = "behind  " + Vector3.Dot(forward, toOther).ToString();
		    m_TextComponent.text = "back ";// + angle;
		    // Debug.Log("The other transform is behind me!" + Vector3.Dot(forward, toOther));
	       }
	       else
	       {
		    // m_TextComponent.text = "in front  " + Vector3.Dot(forward.normalized, toOther.normalized).ToString() ;
		    m_TextComponent.text = "front  ";// + angle;
	       }


	       Vector3 youDir = transform.TransformDirection(Vector3.forward); ;

	       //The direction from you to the waypoint
	       Vector3 waypointDir = player.transform.position - transform.position;

	       //The cross product between these vectors
	       Vector3 crossProduct = Vector3.Cross(youDir, waypointDir);

	       //The dot product between the your up vector and the cross product
	       //This can be said to be a volume that can be negative
	       float dotProduct = Vector3.Dot(crossProduct, transform.up);

	       //Now we can decide if we should turn left or right
	       if (dotProduct > 0f)
	       {
		   // Debug.Log("Turn right");
		    m_TextComponent.text += " right  " + Mathf.RoundToInt(angle).ToString()+ "° " + waypointDir.magnitude + "m";
	       }
	       else
	       {
		    m_TextComponent.text += " left  "  +Mathf.RoundToInt(angle).ToString() + "° " + waypointDir.magnitude + "m"; ;
	       }


	       moveInCircle();


	  }
	  public void moveInCircle()
	  {
	       moveValue += Time.deltaTime * Speed;
	       float x = Mathf.Sin(moveValue) * Radius;
	       float z = Mathf.Cos(moveValue) * Radius;
	       //direction = destination - source
	       Vector3 direction = player.transform.position - transform.position;
	       //
	       Debug.DrawRay(transform.position, direction, Color.green);
	       //calculate the ann. he inverse tangent method
	       float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg - 1;
	       //Debug.Log("Angle: " + angle);
	       //define the rotation along a specific axis using the angle
	       Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
	       //slerp from our current rotation to the new specific rotatiwi
	       // transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 50); 
	       //  transform.position = new Vector3(x, 0, z);
	       transform.position += transform.forward * Time.deltaTime * Speed;
	       transform.Rotate(0, -0.3f, 0);

	       // transform.position.x = x;
	       //  otherObject.transform.position = transform.TransformPoint(pos);
	  }
	  private void OnDrawGizmos()
	  {
	       Gizmos.color = Color.blue;
	       // Gizmos.DrawWireSphere(transform.position, vievDistance);
	       //Vector3 fovLineLeft = Quaternion.AngleAxis(halfAngle, transform.up) * this.aimDirection * vievDistance;
	       //Vector3 fovLine2 = Quaternion.AngleAxis(-halfAngle, transform.up) * this.aimDirection * vievDistance;
	       //  Debug.Log("start  " + GetAngleFromVectorFloat(fovLineLeft));
	       Vector3 direction = player.transform.position - transform.position;
	       //
	       // Debug.DrawRay(transform.position, direction, Color.green);
	       //  Gizmos.DrawRay(transform.position, transform.forward * 2);
	       //   Gizmos.DrawRay(origin, fovLine2);
	       Gizmos.color = Color.red;



	  }
     }
}
