using UnityEngine;
using System.Collections;

public class Deadly : MonoBehaviour {
	public GameObject respawner;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			respawner.GetComponent<Die>().kill ();
		}
	}
}
