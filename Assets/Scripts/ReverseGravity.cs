using UnityEngine;
using System.Collections;

public class ReverseGravity : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    /***
         * onTriggerEnter2D(Collider2D other)
         * Description: reverse the control if the player passes through gameobject 
         *             attached with this script
         * Args: Collider2D other - only reacts to player
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
}
