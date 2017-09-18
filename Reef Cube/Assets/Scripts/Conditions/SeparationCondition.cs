using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationCondition : MonoBehaviour {

	public bool Test(Transform character,float threshold, float fieldOfView)
    {

        Collider[] hitColliders = Physics.OverlapSphere(character.position, threshold);
        foreach (Collider collider in hitColliders)
        {
            Vector3 direction = collider.transform.position - character.position;
            float angle = Vector3.Angle(direction, character.forward);
            RaycastHit hit;
            if (angle < fieldOfView * 0.5f)
            {
                if (Physics.Raycast(character.position, direction.normalized, out hit, threshold))
                {
                    if (hit.transform.tag == "Boid")
                    {
                        Debug.DrawRay(character.position, direction, Color.red);
                        return true;
                    }
                }
            }
             
        }
        return false;
    }
}
