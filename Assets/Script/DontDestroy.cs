using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public GameObject dataManager;

	private void Awake()
	{
        dataManager = GameObject.FindGameObjectWithTag("DataObject");
        DontDestroyOnLoad(dataManager);
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
