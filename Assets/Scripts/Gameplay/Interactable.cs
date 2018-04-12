using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        //Check if the colliding object is controlled by player
        PlayerController playerController = collision.GetComponent<PlayerController>();

        //If the colliding object is controlled by player
        if (playerController != null)
        {
            playerController.interactableObject = this;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        //Check if the colliding object is controlled by player
        PlayerController playerController = collision.GetComponent<PlayerController>();

        //If the colliding object is controlled by player
        if (playerController != null)
        {
            playerController.interactableObject = null;
        }
    }

    virtual public void Interact()
    {
    }
}
