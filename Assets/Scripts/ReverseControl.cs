using UnityEngine;
using System.Collections;

public class ReverseControl : MonoBehaviour
{

    /***
     * onTriggerEnter2D(Collider2D other)
     * Description: reverse the control if the player passes through the gameobject 
     *              attached with this script
     * Args: Collider2D other - only reacts to player trigger
     * Returns: none
     ***/

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            FlingObject player = other.gameObject.GetComponent<FlingObject>();
            player.reverseControl();
 
        }
    }
    /* can change to OnCollision2D(Collider2D Other) and turn isTrigger off
    *  to make the effect appear on collision instead, but still achieves
    *  the same effect
    */
}


