using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCondition : MonoBehaviour {

    public bool Test(Vector3 positionA, Vector3 positionB, float distance)
    {
            Vector3 separation = positionA - positionB;
            if (separation.magnitude > distance)
            {
                return true;
            }
            else
            {
                return false;
            }
    }
}
