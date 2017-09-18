
using UnityEngine;

interface ITransition<T>
{
    bool IsTriggered(Transform character, float threshold, float lookAhead, Vector3 positionB, float distance, float fieldOfView);
    string GetTargetState();
}
