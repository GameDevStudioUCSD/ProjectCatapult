using UnityEngine;
using System.Collections;

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
