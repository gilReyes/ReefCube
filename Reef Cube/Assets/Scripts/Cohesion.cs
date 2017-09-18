using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cohesion : MonoBehaviour {
    Arrive arrive;
    GameObject m_character;
    // Use this for initialization
    public Cohesion(GameObject character, float maxAcceleration, float targetRadius, float slowRadius, float timeToTarget)
    {

        arrive = new Arrive(character, maxAcceleration, targetRadius, slowRadius, timeToTarget);
        m_character = character;
    }
	
	// Hand off the center to arrive behavior
	public SteeringOutput GetSteering (float maxSpeed) {

        Vector3 theCenter = CenterOfMass();
        return arrive.GetSteering(theCenter, maxSpeed);

	}
    //Calculate the center of mass of the flock
    public Vector3 CenterOfMass()
    {
        Vector3 center = new Vector3(0, 0, 0);
        float count = 0;
        GameObject[] boids = GameObject.FindGameObjectsWithTag("Boid");
        foreach (GameObject boid in boids)
        {
            if (!boid.name.Equals(m_character.name))
            {
                center += boid.transform.position;
                count++;
            }
        }
        return center / count;
    }
}
