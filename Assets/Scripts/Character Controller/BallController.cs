﻿using UnityEngine;
using System.Collections;

/**
 * Filename: BallController.cs \n
 * Author: Michael Gonzalez \n
 * Contributing Authors: N/A \n
 * Date Drafted: 8/2/2015 \n
 * Description: A simple ball controller script. The idea is to have all 
 *              ball control scripting needs here. By our game's nature, Dr. 
 *              Eric can simply be thought of as a ball. Currently, this script
 *              only plays Dr. Eric's landing sound when he collides with a 2D
 *              object collider.
 */
public class BallController : MonoBehaviour {
    public enum State { IDLE, STUCK, LAUNCHING, LANDING }
    public AudioClip landSound;
    public AudioSource audio;
    public State state = State.IDLE;
    public Platform controllingPlatform;
    public float landingTolerance = 1.1f;
    private OrientWithGravity orientor;
    private Rigidbody2D rb;
    private float lastHit;
    private float bounceBufferPeriod = .4f;
    private RespawnController respawner;

    /**
     * Description: This method currently sets up a reference to the ball's 
     *              AudioSource
     */
    void Start()
    {
        orientor = GetComponent<OrientWithGravity>();
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        respawner = GameObject.Find("Respawner/Spawner").GetComponent<RespawnController>();
    }
    /**
     * Description: This method currently will only play the landing sound when
     *              a ball hits a 2D collider.
     */
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (audio != null)
            audio.PlayOneShot(landSound, 1f);
        if (state == State.LAUNCHING)
            state = State.LANDING;
        lastHit = Time.time;
    }
    void Update()
    {
        switch (state)
        {
            case State.IDLE:
                break;
            case State.LAUNCHING:
                controllingPlatform = null;
                break;
            case State.LANDING:
                HasLanded();
                break;
        }
        //out of bounds kill
        if (respawner.player.transform.position.x < -1000 ||
            respawner.player.transform.position.x > 1000 ||
            respawner.player.transform.position.y < -1000 ||
            respawner.player.transform.position.y > 1000)
            respawner.kill();
        
    }
    void HasLanded()
    {
        if (Time.time - lastHit < bounceBufferPeriod)
            return;
        Vector3 gravity = Physics2D.gravity;
        Vector3 velocityProjG = Vector3.Project(rb.velocity, gravity);
        if ((velocityProjG+gravity).magnitude < landingTolerance * gravity.magnitude)
        {
            state = State.IDLE;
        }
    }
    
}
