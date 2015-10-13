using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	private Vector3 direction;
	private float delta;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += delta * direction;
	}

	public void setDirection(Vector3 directionPar) {
		direction = directionPar;
	}

	public void setDelta(float deltaPar) {
		delta = deltaPar;
	}
}