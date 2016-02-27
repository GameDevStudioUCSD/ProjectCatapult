﻿using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {


    private float starttime;
    public float checktime;
	private float dietime;
    private int speed = 3;
    public float secondsAliveOutOfWater;
    public int health;
    public FishSchool schoolOfFish;

	// Use this for initialization
	void Start () {
		dietime = Time.time;
        starttime = Time.time;
        //Debug.Log(currentpos);

	}
	
	// Update is called once per frame
	void Update () {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        Vector3 vectorToTarget = rigid.velocity;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
        Debug.Log(transform.rotation.eulerAngles.z);
        if (transform.rotation.eulerAngles.z < 180 && transform.localScale.y < 0) transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y);
        if (transform.rotation.eulerAngles.z > 180 && transform.localScale.y > 0) transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y);
        if (Time.time - dietime > secondsAliveOutOfWater)
		{
			if (schoolOfFish != null)
			{
				schoolOfFish.KillAFish();
			}
            Destroy(this.gameObject);
        }
    }

	void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Water")
        {
            dietime = Time.time;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
    	if (col.gameObject.tag == "Player")
    	{
    		health--;
    		if (health <= 0)
    		{
				if (schoolOfFish != null)
				{
					schoolOfFish.KillAFish();
				}
           		Destroy(this.gameObject);
    		}
    	}
    }
}
