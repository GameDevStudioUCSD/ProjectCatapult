using UnityEngine;
using System.Collections;

public class InvertGravity : MonoBehaviour
{

    public bool isDebugging=false;

    /***
         * OnCollisionEnter2D(Collision2D other)
         * Description: invert the gravity of the player if the player touches a  
         *              gameobject attached with this script
         * Args: Collider2D other - only reacts to player collision
         * Returns: none
         ***/

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody2D playerRB = other.gameObject.GetComponent<Rigidbody2D>();
            playerRB.gravityScale = (playerRB.gravityScale==1) ? -1 : 1;
         
            if(isDebugging)
              Debug.Log("Inverting gravity of player");
        }
    }

    /* can change to OnTriggerEnter2D(Collider2D Other) and turn isTrigger on
    *  to make the player go through object instead, but still achieves
    *  the same effect
    */

}
