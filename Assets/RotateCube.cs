using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        
    }
}

/*
   private bool rotatingClockwise = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (rotatingClockwise && (transform.eulerAngles.z > 270 || transform.eulerAngles.z <= 0))
	    {
            transform.Rotate(0, 0, -1);
	    }
	    else if (!rotatingClockwise && transform.eulerAngles.z > 0)
	    {
            transform.Rotate(0, 0, 1);
        
        }

        if (Input.anyKeyDown)
	    {
	        rotatingClockwise = !rotatingClockwise;
	    }
	}
*/