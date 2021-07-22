using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    private GameObject cameraObject;

    private bool once;
    private bool cameraMove;
    private bool setPosition;
    private bool loadButton;

    private GameObject buttonObject;
    private MakeButton nextStageButton;

    private Vector3 dir = Vector3.zero;

    private void Start()
	{
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        once = true;
        cameraMove = false;
        setPosition = true;
        loadButton = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && once)
        {
            cameraMove = true;
            once = false;
        }
    }

	private void Update()
	{
        //if (Input.GetKeyDown(KeyCode.A))
        //    cameraMove = true;
        
        if (cameraMove)
        {
            if (setPosition)
            {
                buttonObject = GameObject.FindGameObjectWithTag("NextButton");
                nextStageButton = buttonObject.GetComponent<MakeButton>();

                setPosition = false;
                dir = new Vector3(cameraObject.transform.position.x, cameraObject.transform.position.y + 11.5f, cameraObject.transform.position.z);
            }
            cameraObject.transform.position = Vector3.MoveTowards(cameraObject.transform.position, dir, Time.deltaTime* 3f);
            
            // Coroutine으로 어케 안되나..
            Vector3 temp = cameraObject.transform.position - dir;
            if (temp.magnitude < 3f && loadButton)
            {
                loadButton = false;
                nextStageButton.ButtonAppear();
            }
        }
    }
}
