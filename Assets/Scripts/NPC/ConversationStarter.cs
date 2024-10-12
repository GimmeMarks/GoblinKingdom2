using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationStarter : MonoBehaviour
{

    [SerializeField] private NPCConversation myConversation;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                ConversationManager.Instance.StartConversation(myConversation);
                ConversationManager.Instance.SetInt("Gold", PlayerController.goldCount);
                ConversationManager.Instance.SetBool("Bought1", PlayerController.Bought1);
                ConversationManager.Instance.SetBool("Bought2", PlayerController.Bought2);
                ConversationManager.Instance.SetBool("Bought3", PlayerController.Bought3);
                ConversationManager.Instance.SetBool("Bought4", PlayerController.Bought4);
                ConversationManager.Instance.SetBool("Bought5", PlayerController.Bought5);
                ConversationManager.Instance.SetBool("Bought6", PlayerController.Bought6);
                ConversationManager.Instance.SetBool("Bought7", PlayerController.Bought7);
                ConversationManager.Instance.SetBool("Bought8", PlayerController.Bought8);
                ConversationManager.Instance.SetBool("LaserBought", PlayerController.LaserBought);
                ConversationManager.Instance.SetBool("IceBought", PlayerController.IceBought);
                ConversationManager.Instance.SetBool("ExplosiveBought", PlayerController.ExplosiveBought);

            }

        }
    }


}
