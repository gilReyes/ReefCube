using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : MonoBehaviour {
    GameObject m_character;

    float m_MaxAcceleration;
    float m_TargetRadius;
    float m_SlowRadius;
    float m_TimeToTarget;
	
    public Arrive(GameObject character, float maxAcceleration, float targetRadius, float slowRadius, float timeToTarget)
    {
        m_character = character;
        m_MaxAcceleration = maxAcceleration;
        m_TargetRadius = targetRadius;
        m_SlowRadius = slowRadius;
        m_TimeToTarget = timeToTarget;
    }
	
	// Update is called once per frame
	public SteeringOutput GetSteering (Vector3 target, float maxSpeed) {
        SteeringOutput steering = new SteeringOutput();
        float targetSpeed;
        Vector3 direction = target - m_character.transform.position;
        float distance = direction.magnitude;

        if (distance < m_TargetRadius)
        {
            steering.m_linear = new Vector3(0, 0, 0);
            return steering;
        }

        if (distance > m_SlowRadius)
        {
            targetSpeed = maxSpeed;
        }
        else
        {
            targetSpeed = maxSpeed * distance / m_SlowRadius;
        }
        Vector3 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        steering.m_linear = targetVelocity - m_character.GetComponent<Rigidbody>().velocity;
        steering.m_linear /= m_TimeToTarget;

        if(steering.m_linear.magnitude > m_MaxAcceleration)
        {
            steering.m_linear.Normalize();
            steering.m_linear *= m_MaxAcceleration;
        }
        steering.m_angular = Quaternion.LookRotation(steering.m_linear);
        return steering;

	}
}
