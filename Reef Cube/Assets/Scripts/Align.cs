using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : MonoBehaviour {
    float m_turnSpeed;
    GameObject m_character;

	public Align(GameObject character,float turnSpeed)
    {
        m_turnSpeed = turnSpeed;
        m_character = character;
    }
	
	public SteeringOutput GetSteering()
    {

        SteeringOutput steering = new SteeringOutput();
        steering.m_linear = this.AvgVelocity();
        steering.m_angular = Quaternion.Slerp(m_character.transform.rotation, Quaternion.LookRotation(steering.m_linear), Time.deltaTime * m_turnSpeed);
        return steering;
    }

    Vector3 AvgVelocity()
    {
        Vector3 velocity = new Vector3(0, 0, 0);
        float count = 0;
        GameObject[] boids = GameObject.FindGameObjectsWithTag("Boid");
        foreach (GameObject boid in boids)
        {
            if (!boid.name.Equals(m_character.gameObject.name))
            {
                velocity += boid.GetComponent<Rigidbody>().velocity;
                count++;
            }
        }
        return velocity / count;
    }
}
