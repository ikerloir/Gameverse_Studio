using UnityEngine;
using System;

public class FollowCamera : MonoBehaviour
{
    // this object position (camera) should be the same as the car's positon

    [SerializeField] GameObject thingToFollow;
    private float cameraDistance = -10;
    void LateUpdate()
    {
        transform.position = thingToFollow.transform.position + new Vector3(0, 0, cameraDistance);
    }
}
