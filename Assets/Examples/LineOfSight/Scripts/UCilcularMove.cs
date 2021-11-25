using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orientation
{
     public class UCilcularMove : MonoBehaviour
     {
	  public float Speed = 2f;
	  public float Radius = 2f;
	  public float moveValue;
	  public GameObject otherObject;
	  public Vector3 pos;

	  void Start()
	  {
	       Debug.Log("Local position: " + transform.localPosition);
	       Debug.Log("Local rotation: " + transform.localRotation);
	       Debug.Log("Local rotation (E): " + transform.localEulerAngles);
	       Debug.Log("Local scale: " + transform.localScale);
	       Debug.Log("Global position: " + transform.position);
	       Debug.Log("Global rotation: " + transform.rotation);
	       Debug.Log("Global rotation (E): " + transform.eulerAngles);
	       Debug.Log(" Mossy scale: " + transform.lossyScale + "!!!(Read - only)!!!");

	  }

	  void Update()
	  {

	       moveValue += Time.deltaTime * Speed;
	       float x = Mathf.Sin(moveValue) * Radius;
	       float z = Mathf.Cos(moveValue) * Radius;

	       //direction = destination - source
	       Vector3 direction = otherObject.transform.position - transform.position;
	       //
	       Debug.DrawRay(transform.position, direction, Color.green); 
	       //calculate the ann. he inverse tangent method
	       float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg -1;
	       //Debug.Log("Angle: " + angle);
	       //define the rotation along a specific axis using the angle
	       Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward); 
	       //slerp from our current rotation to the new specific rotatiwi
	      // transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 50); 
	       //  transform.position = new Vector3(x, 0, z);
	       // transform.position.x = x;
	       //  otherObject.transform.position = transform.TransformPoint(pos);
	  }
     }
}