using UnityEngine;
using System.Collections;

public class RespawnController : MonoBehaviour {
	private bool isDead;
	private double respawnTimer;
	public double respawnTime;
	public GameObject player;
	private GameObject currentPlayer;

	// Use this for initialization
	void Start () {
		respawnTimer = 0;
		respawn ();
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

	public void kill() {
		Destroy (currentPlayer.gameObject);
		isDead = true;
		respawnTimer = 0;
	}

	void respawn() {
		isDead = false;
		currentPlayer = (GameObject)Instantiate (player, transform.position, Quaternion.identity);
	}
}
