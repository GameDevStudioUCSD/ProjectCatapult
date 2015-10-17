using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

    public GameObject asteroid;
    public int time;
    private int currTime;

	// Use this for initialization
	void Start() {
        currTime = time;
	}
	
	// Update is called once per frame
	void Update() {
        currTime -= 1;

        if (currTime == 0) {
            Vector3 pos = new Vector3(
                Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2),
                Random.Range(transform.position.y - transform.localScale.y / 2, transform.position.y + transform.localScale.y / 2)
            );
            Instantiate(asteroid, pos, Quaternion.identity);

            currTime = time;
        }
	}
}
