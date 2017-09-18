using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : ITransition<SteeringOutput> {
    string m_targetState;
    public string m_conditionType;
    DistanceCondition m_distanceCondition;
    CollisionCondition m_collisionCondition;
    SeparationCondition m_separationCondition;
    public Transition(string conditionType, string targetState)
    {
        m_conditionType = conditionType;
        m_targetState = targetState;
        switch (m_conditionType)
        {
            case "Distance":
                m_distanceCondition = new DistanceCondition();
                break;
            case "Collision":
                m_collisionCondition = new CollisionCondition();
                break;
            case "Separation":
                m_separationCondition = new SeparationCondition();
                break;
            case "Wander":
                Console.WriteLine("Default case");
                break;
        }
    }
    public string GetTargetState()
    {
        return m_targetState;
    }

    public bool IsTriggered(Transform character, float threshold,float lookAhead, Vector3 positionB, float distance,float fieldOfView)
    {
        switch (m_conditionType)
        {
            case "Distance":
                return m_distanceCondition.Test(character.position,positionB,distance);
            case "Collision":
                return m_collisionCondition.Test(character,lookAhead);
            case "Separation":
                return m_separationCondition.Test(character,threshold,fieldOfView);
            default:
                return false;
        }
    }
}
