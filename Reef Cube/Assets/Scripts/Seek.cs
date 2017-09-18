using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIUtils
{
    public class Seek : MonoBehaviour
    {
        [SerializeField] GameObject m_character;
        [SerializeField] GameObject m_target;

        [SerializeField] float maxAcceleration; //max speed at which the character will run

        
        // Returns the desired steering output
        public SteeringOutput getSteering()
        {
            //Declaring data struture for output
            SteeringOutput steering = new SteeringOutput();

            //Get direction to the target
            steering.m_linear = m_target.GetComponent<Transform>().position - m_character.GetComponent<Transform>().position; 

            //Give full acceleration along this direction
            steering.m_linear.Normalize();
            steering.m_linear *= maxAcceleration;

            steering.m_angular = Quaternion.Euler(0,0,0);
            return steering;
        }
    }
}

