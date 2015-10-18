using UnityEngine;
using System.Collections;

/**
 * Put this script on anything that kills DrEric on contact.
 * It must have a 2D Collider that is a trigger.
 */
public class Deadly : MonoBehaviour {
	private RespawnController respawner;

	// Use this for initialization
	void Start () {
		respawner = GameObject.Find("Respawner").GetComponent<RespawnController>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			respawner.kill();
		}
	}
}
