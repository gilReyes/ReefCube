using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour {

    [SerializeField] string initialState;
    [SerializeField] float m_maxSpeed;
    [SerializeField] float m_turnSpeed;
    [SerializeField] float tooFar;
    IState<SteeringOutput>[] m_states;
    IState<SteeringOutput> m_currentState;
    IState<SteeringOutput> m_targetState;
    string m_targetStateName;
    Transition triggeredTransition;
    // Use this for initialization
    void Start () {
        //Get all state scripts attached to object
        MonoBehaviour[] m_Objs = this.GetComponents<MonoBehaviour>();
        m_states = (from a in m_Objs where a.GetType().GetInterfaces().Any(k => k == typeof(IState<SteeringOutput>)) select (IState<SteeringOutput>)a).ToArray();
        //Set current state to initial state
        foreach (IState<SteeringOutput> state in m_states)
        {
            if (state.GetName().Equals(initialState))
            {
                m_currentState = state;
            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Move(m_currentState.GetAction());
        foreach (Transition transition in m_currentState.GetTransitions())
        {
            if (m_currentState.GetName().Equals("Wander"))
            {
                if (transition.IsTriggered(this.transform, this.GetComponent<Separation>().m_threshold, this.GetComponent<ObstacleAvoidance>().m_lookAhead, this.GetComponent<Flock>().GetCenterOfMass(), tooFar, this.GetComponent<Separation>().m_fieldOfView))
                {
                    triggeredTransition = transition;
                }
            }
            else if (m_currentState.GetName().Equals("Flock"))
            {
                if (transition.m_conditionType.Equals("Distance"))
                {
                    if (!transition.IsTriggered(this.transform, this.GetComponent<Separation>().m_threshold, this.GetComponent<ObstacleAvoidance>().m_lookAhead, this.GetComponent<Flock>().GetCenterOfMass(), tooFar, this.GetComponent<Separation>().m_fieldOfView))
                    {
                        triggeredTransition = transition;
                    }
                }
                else
                {
                    if (transition.IsTriggered(this.transform, this.GetComponent<Separation>().m_threshold, this.GetComponent<ObstacleAvoidance>().m_lookAhead, this.GetComponent<Flock>().GetCenterOfMass(), tooFar, this.GetComponent<Separation>().m_fieldOfView))
                    {
                        triggeredTransition = transition;
                    }
                }
            }
            else if (m_currentState.GetName().Equals("Separation"))
            {
                if (transition.m_conditionType.Equals("Separation"))
                {
                    if (!transition.IsTriggered(this.transform, this.GetComponent<Separation>().m_threshold, this.GetComponent<ObstacleAvoidance>().m_lookAhead, this.GetComponent<Flock>().GetCenterOfMass(), tooFar, this.GetComponent<Separation>().m_fieldOfView))
                    {
                        triggeredTransition = transition;
                    }
                }
                else
                {
                    if (transition.IsTriggered(this.transform, this.GetComponent<Separation>().m_threshold, this.GetComponent<ObstacleAvoidance>().m_lookAhead, this.GetComponent<Flock>().GetCenterOfMass(), tooFar, this.GetComponent<Separation>().m_fieldOfView))
                    {
                        triggeredTransition = transition;
                    }
                }
            }
            else if (m_currentState.GetName().Equals("Avoidance"))
            {
                if (!transition.IsTriggered(this.transform, this.GetComponent<Separation>().m_threshold, this.GetComponent<ObstacleAvoidance>().m_lookAhead, this.GetComponent<Flock>().GetCenterOfMass(), tooFar, this.GetComponent<Separation>().m_fieldOfView))
                {
                    triggeredTransition = transition;
                }
            }
        }
        if (triggeredTransition != null)
        {
            m_targetStateName = triggeredTransition.GetTargetState();
            foreach (IState<SteeringOutput> state in m_states)
            {
                if (state.GetName().Equals(m_targetStateName))
                {
                    m_targetState = state;
                }
            }
            Debug.Log(m_currentState.GetName() + " -> " + m_targetState.GetName());
            m_currentState = m_targetState;
        }
	}

    void Move(SteeringOutput target)
    {
        //Clip max speed
        if (target.m_linear.magnitude > m_maxSpeed)
        {
            target.m_linear.Normalize();
            target.m_linear *= m_maxSpeed;
        }
        //Apply velocity to the drone in the target direction
        this.GetComponent<Rigidbody>().velocity = target.m_linear * Time.deltaTime;
        //this.GetComponent<Rigidbody>().AddForce(target.m_linear, ForceMode.Acceleration);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.m_angular, Time.deltaTime * m_turnSpeed);
    }
}
