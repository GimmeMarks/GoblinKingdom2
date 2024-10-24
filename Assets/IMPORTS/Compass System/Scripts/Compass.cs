using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {
    public Transform playerTransform;
    Vector3 dir;

    private void Update()
    {
        dir.z = playerTransform.eulerAngles.y / 2;
        transform.localEulerAngles = dir;
    }
}
