using UnityEngine;
using System.Collections;

public class Die : MonoBehaviour {
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
			Debug.Log (respawnTimer);
			if (respawnTimer >= respawnTime) {
				respawn ();
			}
		}
	}

	public void kill() {
		Destroy (currentPlayer.gameObject);
		Debug.Log ("killed");
		isDead = true;
	}

	void respawn() {
		isDead = false;
		currentPlayer = (GameObject)Instantiate (player, transform.position, Quaternion.identity);
	}
}
