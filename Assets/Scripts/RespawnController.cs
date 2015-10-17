using UnityEngine;
using System.Collections;

/**
 * Handles DrEric's death and respawning. Should be on the Respawner object.
 * There should only be one Respawner in the level.
 */
public class RespawnController : MonoBehaviour {
	private bool isDead;
	private double respawnTimer;
	public double respawnTime;
	public GameObject player;
	private GameObject currentPlayer = null;

	// Use this for initialization
	void Start () {
		respawnTimer = 0;
		respawn(); //initial creation of DrEric
	}
	
	// Update is called once per frame
	void Update () {
		if (isDead) {
			respawnTimer += Time.deltaTime;
			if (respawnTimer >= respawnTime) {
				respawn ();
			}
		}
	}
	/*
	 * Destroys DrEric and begins a countdown to respawning.
	 */
	public void kill() {
		if (currentPlayer != null) { //prevents attempting to delete null
			Destroy (currentPlayer.gameObject);
			isDead = true;
			respawnTimer = 0;
			Debug.Log ("DrEric has died");
		}
	}

	/*
	 * Spawns a new DrEric for the player to control. Only one DrEric can exist at once.
	 */
	public void respawn() {
		if (currentPlayer == null) { //DrEric must not already exist
			isDead = false;
			currentPlayer = (GameObject)Instantiate (player, transform.position, Quaternion.identity);
			Debug.Log ("DrEric has spawned");
		}
	}
}
