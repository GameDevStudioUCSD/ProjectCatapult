using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {
	public GameObject bullet;
	public int time;
	private int curTime;
	public Vector3 direction;
	public float delta;

	// Use this for initialization
	void Start () {
		curTime = time;
	}
	
	// Update is called once per frame
	void Update () {
		curTime -= 1;
		if (curTime == 0) {
			GameObject myBullet = (GameObject) GameObject.Instantiate(bullet, transform.position, Quaternion.identity);
			Bullet thisBullet = myBullet.GetComponent<Bullet>();
			thisBullet.setDirection(direction);
			thisBullet.setDelta(delta);
			curTime = time;
		}
	}
}
