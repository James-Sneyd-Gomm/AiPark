using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
           gameObject.transform.position += new Vector3(0.0f,1.0f,0.0f);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            gameObject.transform.position -= new Vector3(0.0f, 1.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.transform.position += new Vector3(1.0f, 0.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.transform.position -= new Vector3(1.0f, 0.0f, 0.0f);
        }

    }







}
