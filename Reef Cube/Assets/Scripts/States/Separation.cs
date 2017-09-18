using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separation : MonoBehaviour, IState<SteeringOutput> {

    GameObject m_character; //Data of the character
    List<GameObject> targets; //Potential targets
    [SerializeField] public float m_threshold; //Threshold to take action
    [SerializeField] float m_decayCoefficient; //Coefficient for inverse square law force
    [SerializeField] float m_maxAcceleration; //Character maximum acceleration
    [SerializeField] public float m_fieldOfView;

    public string stateName = "Separation";
    Transition[] m_transitions;

    private void Start()
    {
        m_character = this.gameObject;
        m_transitions = new Transition[2];
        InitializeTransitions();
    }

    void InitializeTransitions()
    {
        m_transitions[0] = new Transition("Separation", "Wander");
        m_transitions[1] = new Transition("Collsion", "Avoidance");
    }

    public SteeringOutput GetAction()
    {
        SteeringOutput steering = new SteeringOutput();
        GetNearbyTargets(); //Get targets in a cone in front of the Object
        foreach (GameObject target in targets) //Loop through each target
        {
            Vector3 direction = m_character.transform.position - target.transform.position; //Check if target is close
            float distance = direction.magnitude;
            if (distance < m_threshold)
            {
                float strength = Math.Min(m_threshold/ distance * distance, m_maxAcceleration); //Calculate the strength of repulsion
                direction.Normalize();
                steering.m_linear += strength * direction; //Add acceleration
            }
        }
        steering.m_angular = Quaternion.LookRotation(steering.m_linear);
        //We have done through all targets, return the result
        return steering;
    }

    public void GetNearbyTargets()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_threshold);
        targets = new List<GameObject>();
        foreach (Collider collider in hitColliders)
        {
            Vector3 direction = collider.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle < m_fieldOfView * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction.normalized, out hit, m_threshold))
                {
                    if (hit.transform.tag == "Boid")
                    {
                        Debug.DrawRay(transform.position, direction, Color.red);
                        targets.Add(hit.collider.gameObject);
                    }
                }
            }
        }
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
