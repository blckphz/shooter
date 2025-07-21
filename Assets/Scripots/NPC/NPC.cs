using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.F;
    [SerializeField] private npcInk inkHandler; // Reference to InkWithTMP script

    private bool playerInRange = false;
    private bool dialogueStarted = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            if (!dialogueStarted)
            {
                inkHandler.StartStory();   // Start the story first time
                dialogueStarted = true;
            }
            else
            {
                inkHandler.ContinueStory();  // Progress through story
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
