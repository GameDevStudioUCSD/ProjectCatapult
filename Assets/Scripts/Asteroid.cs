using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    public float speed;

    private Vector3 direction;

	// Use this for initialization
	void Start() {
        transform.localScale = new Vector3(Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f), 1.0f);
        direction = Random.insideUnitSphere;
        direction.z = 0.0f;
	}
	
	// Update is called once per frame
	void Update() {
        transform.position += direction * speed;
	}
}
