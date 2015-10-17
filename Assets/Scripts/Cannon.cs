using UnityEngine;
using System.Collections;

/**
 * Script for Cannon objects. Controls cannonball generation.
 */
public class Cannon : MonoBehaviour {
	public GameObject bullet; //the prefab for the cannonballs
	public int time; //the time between shots
	private int curTime; //running count of time until next shot
	public Vector3 direction; //Direction the cannon points and the cannonballs travel
	public float delta; //Speed of the cannonballs

	// Use this for initialization
	void Start () {
		curTime = time;
	}
	
	// Update is called once per frame
	void Update () {
		curTime -= 1;
		if (curTime == 0) {
			//creating bullet
			GameObject myBullet = (GameObject) GameObject.Instantiate(bullet, transform.position, Quaternion.identity);
			Bullet thisBullet = myBullet.GetComponent<Bullet>();

			//setting bullet properties
			thisBullet.setDirection(direction);
			thisBullet.setDelta(delta);

			//prepare for next shot
			curTime = time;
		}
	}
}
