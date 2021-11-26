using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTransformMove : MonoBehaviour
{

     public float moveSpeed = 10f;
     public float turnSpeed = 50f;
     public float jumpHeight = 5f;

     public ULineOfSight linofsight = null;
     public LayerMask ground;

     void Update()
     {

	  if (linofsight)
	  {
	         linofsight.SetOrigin(transform.position);
	         linofsight.SetAimDirection(transform.forward);

	  }
	
	  if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100,ground))
	  {
	       transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
	  }

	  if (Input.GetKey(KeyCode.W))
	       transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

	  if (Input.GetKey(KeyCode.S))
	       transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);

	  if (Input.GetKey(KeyCode.A))
	       transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
	       //transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);

	  if (Input.GetKey(KeyCode.D))
	       transform.Translate(-Vector3.left * moveSpeed * Time.deltaTime);
	       //transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);

	//  if (Input.GetKey(KeyCode.Space))
	     //  GetComponent<Rigidbody>().velocity = Vector3.up * jumpHeight;
     }
}