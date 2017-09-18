using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : MonoBehaviour, IState<SteeringOutput> {
    [SerializeField] float m_avoidDistance;
    [SerializeField] public float m_lookAhead;

    RaycastHit m_collisionDetector;
    RaycastHit m_collisionDetector1;
    RaycastHit m_collisionDetector2;

    public string stateName = "Avoidance";

    SteeringOutput avoidanceOutput;
    Transition[] m_transitions;
    // Use this for initialization
    void Start () {
        avoidanceOutput = new SteeringOutput();
        m_transitions = new Transition[1];
        InitializeTransitions();
    }

    void InitializeTransitions()
    {
        m_transitions[0] = new Transition("Distance", "Flock");
    }

    public SteeringOutput GetAction()
    {
        if (Physics.Raycast(transform.position, -transform.forward, out m_collisionDetector, m_lookAhead))
        {
            if (m_collisionDetector.collider.tag != "Boid")
            {
                avoidanceOutput.m_linear = m_collisionDetector.transform.position + m_collisionDetector.normal * m_avoidDistance;
                avoidanceOutput.m_angular = Quaternion.LookRotation(avoidanceOutput.m_linear);
                return avoidanceOutput;
            }
        }
        Quaternion spreadAngle = Quaternion.AngleAxis(30, new Vector3(0, 1, 0));
        Vector3 rightVector = spreadAngle * -transform.forward;
        if (Physics.Raycast(transform.position, rightVector, out m_collisionDetector1, m_lookAhead))
        {
            if (m_collisionDetector1.collider.tag != "Boid")
            {
                avoidanceOutput.m_linear = m_collisionDetector1.transform.position + m_collisionDetector1.normal * m_avoidDistance;
                avoidanceOutput.m_angular = Quaternion.LookRotation(avoidanceOutput.m_linear);
                return avoidanceOutput;
            }
        }
        Quaternion spreadAngle2 = Quaternion.AngleAxis(150, new Vector3(0, 1, 0));
        Vector3 leftVector = spreadAngle2 * -transform.forward;
        if (Physics.Raycast(transform.position, leftVector, out m_collisionDetector2, m_lookAhead))
        {
            if (m_collisionDetector2.collider.tag != "Boid")
            {
                avoidanceOutput.m_linear = m_collisionDetector2.transform.position + m_collisionDetector2.normal * m_avoidDistance;
                avoidanceOutput.m_angular = Quaternion.LookRotation(avoidanceOutput.m_linear);
                return avoidanceOutput;
            }
        }
        return avoidanceOutput;
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
