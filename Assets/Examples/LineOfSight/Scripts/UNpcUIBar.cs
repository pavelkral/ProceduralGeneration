using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UNpcUIBar : MonoBehaviour
{
     public Camera main_camera;
     // Start is called before the first frame update
     void Start()
     {
          main_camera = Camera.main;
     }

     // Update is called once per frame
     void LateUpdate()
     {
          // transform.LookAt(transform.position + main_camera.transform.rotation * Vector3.back, main_camera.transform.rotation * Vector3.down);
          transform.LookAt(main_camera.transform);
          transform.Rotate(0,180,0);
     }
}
