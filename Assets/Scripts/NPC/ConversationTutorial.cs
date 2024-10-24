using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationTutorial : MonoBehaviour
{

    [SerializeField] private NPCConversation myConversation;
    private PlayerController PlayerController;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
                ConversationManager.Instance.StartConversation(myConversation);
                ConversationManager.Instance.SetBool("Bought1", PlayerController.Bought1);
                ConversationManager.Instance.SetBool("Bought2", PlayerController.Bought2);
                ConversationManager.Instance.SetBool("Bought3", PlayerController.Bought3);
                ConversationManager.Instance.SetBool("Bought4", PlayerController.Bought4);
                ConversationManager.Instance.SetBool("Bought5", PlayerController.Bought5);

        }

    }
    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting collider is the player
        if (other.CompareTag("Player")) // Assuming the player has the tag "Player"
        {
            ConversationManager.Instance.EndConversation(); // Call the method to end the conversation
        }
    }



}


