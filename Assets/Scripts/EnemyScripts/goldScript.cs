using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldScript : MonoBehaviour
{
    public int goldAmount;

    public void setGold(int goldFromEnemy)
    {
        goldAmount = goldFromEnemy;
    }
}
