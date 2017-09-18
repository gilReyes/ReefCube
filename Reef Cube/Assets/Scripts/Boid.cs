using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
    public float speed = 0.001f;
    float rotationSpeed = 1.0f;
    Vector3 flockDirection;
    Vector3 flockPosition;
    float neighbourThreshold = 3.0f;
    float collisionThreshold = 1.0f;
    
	// Use this for initialization
	void Start () {
        speed = Random.Range(0.5f, 1);
	}
	
	// Update is called once per frame
	void Update () {
        if(Vector3.Distance(transform.position,Vector3.zero) >= GlobalFlock.enviromentSize)
        {
            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            speed = Random.Range(0.5f, 1);
        }
        else
        {
            if (Random.Range(0, 5) < 1)
            {
                Evaluate();
            }

        }

        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    private void Evaluate()
    {

        GameObject[] flockType = new GameObject[10];
        GlobalFlock[] flocks = GameObject.FindObjectsOfType<GlobalFlock>();
        foreach(GlobalFlock flock in flocks)
        {
            if(flock.name == this.transform.tag)
            {
                flockType = flock.flock;
            }
        }

        Vector3 centerOfMass = Vector3.zero;
        Vector3 avoidVector = Vector3.zero;
        float flockSpeed = 0.1f;

        Vector3 goalPosition = GlobalFlock.goalPos;
        float distance;
        float groupSize = 0;
        foreach(GameObject boid in flockType)
        {
            if(boid != this.gameObject)
            {
                distance = Vector3.Distance(boid.transform.position, this.transform.position);
                if(distance <= neighbourThreshold)
                {
                    centerOfMass += boid.transform.position;
                    groupSize++;

                    if(distance < collisionThreshold)
                    {
                        avoidVector += this.transform.position - boid.transform.position;
                    }

                    flockSpeed += boid.GetComponent<Boid>().speed;
                }
            }

            if(groupSize > 0)
            {
                centerOfMass = centerOfMass / groupSize + (goalPosition - this.transform.position);
                speed = flockSpeed / groupSize;

                Vector3 direction = (centerOfMass + avoidVector) - transform.position;
                if(direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
