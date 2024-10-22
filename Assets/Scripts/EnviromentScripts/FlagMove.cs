using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMove : MonoBehaviour
{
    public float amount = 3;

    public void MoveFlagUp()
    {
        // Move the flag up by the specified amount
        transform.position += new Vector3(0, amount, 0);
    }
}
