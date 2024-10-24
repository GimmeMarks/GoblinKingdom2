using UnityEngine;

public class goldScript : MonoBehaviour
{
    public int goldAmount;

    public void setGold(int goldFromEnemy)
    {
        goldAmount = goldFromEnemy;
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

}
