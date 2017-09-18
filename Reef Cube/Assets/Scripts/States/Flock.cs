using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour , IState<SteeringOutput> {
    [SerializeField] float m_MaxAcceletaion;
    [SerializeField] float m_TargetRadius;
    [SerializeField] float m_SlowRadius;
    [SerializeField] float m_TimeToTarget;
    [SerializeField] float m_MaxSpeed;
    [SerializeField] float m_TurnSpeed;

    public string stateName = "Flock";

    Cohesion cohesion;
    Align align;
    SteeringOutput flockingOutput;
    Transition[] m_transitions;

    // Use this for initialization
    void Start () {
        cohesion = new Cohesion(this.gameObject, m_MaxAcceletaion, m_TargetRadius, m_SlowRadius, m_TimeToTarget);
        align = new Align(this.gameObject,m_TurnSpeed);
        flockingOutput = new SteeringOutput();
        m_transitions = new Transition[2];
        InitializeTransitions();
    }

    void InitializeTransitions()
    {
        m_transitions[0] = new Transition("Distance", "Wander");
        m_transitions[1] = new Transition("Collision", "Avoidance");
    }

    public SteeringOutput GetAction()
    {
        flockingOutput.m_linear = cohesion.GetSteering(m_MaxSpeed).m_linear + align.GetSteering().m_linear;
        flockingOutput.m_angular = cohesion.GetSteering(m_MaxSpeed).m_angular * align.GetSteering().m_angular;
        return flockingOutput;
    }

    public Vector3 GetCenterOfMass()
    {
        return cohesion.CenterOfMass();
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
