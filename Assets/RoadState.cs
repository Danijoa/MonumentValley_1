using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadState : MonoBehaviour
{
    public static int roadNumber = 0;

    public int roadNum;

    private void Awake()
    {
        roadNum = roadNumber++;
        //Debug.Log("roadNum : " + roadNum);
    }
}
