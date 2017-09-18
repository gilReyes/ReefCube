using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMatch : MonoBehaviour {
    GameObject m_character;

    [SerializeField]float m_maxAcceleration;
    [SerializeField] float m_timeToTarget;
	// Use this for initialization
	void Start () {
        m_character = this.gameObject;
	}
	
	public SteeringOutput GetSteering (Vector3 target) {
        SteeringOutput steering = new SteeringOutput();
        steering.m_linear = target - m_character.GetComponent<Rigidbody>().velocity;
        steering.m_linear /= m_timeToTarget;

        //if(steering.m_linear.magnitude > m_maxAcceleration)
        //{
        //    steering.m_linear.Normalize();
        //    steering.m_linear *= m_maxAcceleration;
        //}

        steering.m_angular = Quaternion.Euler(0,0,0);
        return steering;

    }
}
