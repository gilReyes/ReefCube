using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrioritySteering : MonoBehaviour {
    [SerializeField] float epsilon;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public SteeringOutput GetSteering (List<SteeringOutput> group) {
		foreach(SteeringOutput steering in group)
        {
            if(steering.m_linear.magnitude > epsilon)
            {
                return steering;
            }
        }
        return group[group.Count-1];
	}
}
