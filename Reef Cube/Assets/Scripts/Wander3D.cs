using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander3D : MonoBehaviour {
    [SerializeField] float m_wanderOffset;
    [SerializeField] float m_wanderRadiusXZ;
    [SerializeField] float m_wanderRadiusY;

    [SerializeField] float m_wanderRate;
    [SerializeField] float m_turnSpeed;
    Vector3 m_wanderVector;
    ObstacleAvoidance avoidance;
    [SerializeField] float m_maxAcceleration;
    Vector3 m_target;
    SteeringOutput steering;
    SteeringOutput avoidanceOutput;
    [SerializeField] float epsilon;
    float timer;
    // Use this for initialization
    void Start () {
        m_wanderVector = new Vector3(0, 0, 0);
        steering = new SteeringOutput();
        avoidance = this.GetComponent<ObstacleAvoidance>();
    }
	
	// Update is called once per frame
	void Update () {
        SteeringOutput output = this.GetSteering();
        //avoidanceOutput = avoidance.GetSteering();
        if (avoidanceOutput.m_linear.magnitude > epsilon)
        {
            //(avoidanceOutput.m_linear);
            this.transform.position += (avoidanceOutput.m_linear) * m_maxAcceleration * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-avoidanceOutput.m_linear), Time.deltaTime * m_turnSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, output.m_angular, Time.deltaTime * m_turnSpeed);
            //Move(-transform.forward);
            this.transform.position += (-transform.forward) * m_maxAcceleration * Time.deltaTime;
        }

        
        if (transform.GetComponent<Rigidbody>().velocity.magnitude > m_maxAcceleration)
        {
            transform.GetComponent<Rigidbody>().velocity = transform.GetComponent<Rigidbody>().velocity.normalized * m_maxAcceleration;
        }

    }

    void Move(Vector3 move)
    {
        //Clip max speed
        if (move.magnitude > m_maxAcceleration)
        {
            move.Normalize();
            move *= m_maxAcceleration;
        }
        this.GetComponent<Rigidbody>().AddForce(move * Time.deltaTime, ForceMode.VelocityChange);
    }

    SteeringOutput GetSteering()
    {
        //Update wander direction
        m_wanderVector.x += RandomBinomial() * m_wanderRate;
        m_wanderVector.y += RandomBinomial() * m_wanderRate;
        m_wanderVector.z += RandomBinomial() * m_wanderRate;
        m_wanderVector.Normalize();
        //Calculate transformed target direction and scale it
        m_target = this.transform.rotation * m_wanderVector;
        m_target.x *= m_wanderRadiusXZ;
        m_target.y *= m_wanderRadiusY;
        m_target.z *= m_wanderRadiusXZ;


        m_target += this.transform.position + Quaternion.ToEulerAngles(this.transform.rotation) * m_wanderOffset;
        
        steering.m_angular = Quaternion.LookRotation(m_target);

        return steering;
    }

    float RandomBinomial()
    {
        return Random.Range(0.0f, 1.0f) - Random.Range(0.0f, 1.0f);
    }
}
