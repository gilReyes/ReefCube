using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour, IState<SteeringOutput> {
    [SerializeField] Vector3 m_wanderOffset;
    [SerializeField] float m_wanderRadiusXZ;
    [SerializeField] float m_wanderRadiusY;
    [SerializeField] float m_wanderRate;
    [SerializeField] float m_turnSpeed;


    Vector3 m_wanderVector;
    Vector3 m_target;

    SteeringOutput wanderOutput;
    Transition[] m_transitions;

    public string stateName = "Wander";
    // Use this for initialization
    void Start () {
        wanderOutput = new SteeringOutput();
        m_transitions = new Transition[3];
        InitializeTransitions();
	}

    void InitializeTransitions()
    {
        m_transitions[0] = new Transition("Distance", "Flock");
        m_transitions[1] = new Transition("Separation", "Separation");
        m_transitions[2] = new Transition("Collision", "Avoidance");
    }

    public SteeringOutput GetAction()
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


        m_target += this.transform.position + this.transform.rotation * m_wanderOffset;

        wanderOutput.m_linear = transform.forward;
        wanderOutput.m_angular = Quaternion.LookRotation(m_target);

        return wanderOutput;
    }

    float RandomBinomial()
    {
        return Random.Range(0.0f, 1.0f) - Random.Range(0.0f, 1.0f);
    }

    public string GetName()
    {
        return this.stateName;
    }

    public Transition[] GetTransitions()
    {
        return m_transitions;
    }
}
