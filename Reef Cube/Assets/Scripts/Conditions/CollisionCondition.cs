using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCondition : MonoBehaviour {
    RaycastHit m_collisionDetector;
    RaycastHit m_collisionDetector1;
    RaycastHit m_collisionDetector2;

    public bool Test(Transform character , float lookAhead)
    {
        if (Physics.Raycast(character.position, character.forward , out m_collisionDetector, lookAhead))
        {
            if (m_collisionDetector.collider.tag != "Boid")
            {
                return true;
            }
        }
        Quaternion spreadAngle = Quaternion.AngleAxis(30, new Vector3(0, 1, 0));
        Vector3 rightVector = spreadAngle * character.forward;
        if (Physics.Raycast(character.position, rightVector, out m_collisionDetector1, lookAhead))
        {
            if (m_collisionDetector1.collider.tag != "Boid")
            {
                return true;
            }
        }
        Quaternion spreadAngle2 = Quaternion.AngleAxis(150, new Vector3(0, 1, 0));
        Vector3 leftVector = spreadAngle2 * character.forward;
        if (Physics.Raycast(character.position, leftVector, out m_collisionDetector2, lookAhead))
        {
            if (m_collisionDetector2.collider.tag != "Boid")
            {
                return true;
            }
        }
        return false;
    }
}
