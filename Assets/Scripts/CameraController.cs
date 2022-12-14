using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Follow;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(Follow.position.x, Follow.position.y, -10.0f);
    }
}
