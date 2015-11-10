using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Filename: Barrel.cs
 * Author: Steven Lee
 * Contributing Authors: 
 * Date Drafted: 10/24/2015
 * Description: Defines the behaviours of the Barrel object.  Player can enter
 *              the barrel through triggering collision and then fire out of the
 *              barrel.
 */
public class Barrel : MonoBehaviour {

    // Activate for debugging purposes
    const bool isDebugging = true;
    

    //============ Constants =====================================================

    // Tag of object that can interact with barrel
    const string ALLOWED_TAG = "Player";

    // The barrel's transform has to be offset so the barrel opening is forward
    const float ROTATION_OFFSET = 90.0f;

    //============ Inspector =====================================================

    // Time it takes for barrel to be ready for empty state
    public float cooldownDuration = 1.0f;

    // The force at which the barrel fires the object
    public int fireForce = 1500;

    // Movement speed of the barrel
    public int movementSpeed = 2;

    // Holds the objects that will determine the patrol path of the barrel
    public List<GameObject> positionObjects = new List<GameObject>();

    //============ Private =======================================================

    // Transform of the barrel, required to turn barrel
    private Transform barrelTransform;

    // Object that is currently inside the barrel
    private GameObject enteredObject;

    // When barrel was fired and next time it will be empty state
    private float firedTime;
    private float cooldownTime;

    // Vectors used for mouse tracking
    private Vector2 initialVector, finalVector;

    /*
     * Which State the Barrel is in right now.
     * 
     * empty - Barrel has no object inside and can be occupied
     * occupied - Barrel has object inside and can be fired
     * cooldown - Barrel has recently fired and has no object inside but cannot be occupied
     */
    private enum State { empty, occupied, cooldown };
    private State currentState;

    private int positionNumber;

	// Use this for initialization
	void Start () {

        barrelTransform = GetComponent<Transform>();

        // The barrel is initially empty state.
        currentState = State.empty;

        // Which position the barrel should move to first
        positionNumber = 0;
	}
	
	// Update is called once per frame
	void Update () {

        Move();

        switch(currentState)
        {
            case State.empty: break;

            case State.occupied: TrackMouse();
                break;

            case State.cooldown: Cooldown();
                break;
        }

	}

    /*
     * OnTriggerEnter2D
     * 
     * Triggered when a collider with allowed tag collides with barrel.
     * Will only function in the empty state.
     */
    void OnTriggerEnter2D(Collider2D other)
    {
        // If barrel is not empty, then return
        if (currentState != State.empty)
        {
            return;
        }

        // Check if object is allowed to enter the barrel
        if(other.CompareTag(ALLOWED_TAG))
        {
            Debug.Log("Player tag object triggered Barrel");

            currentState = State.occupied;

            // Get the object that entered the barrel
            enteredObject = other.gameObject;

            // Deactivate the object to hide it
            enteredObject.SetActive(false);

        }
        else
        {
            Debug.Log("Non-player tag object triggered Barrel");
        }
    }


    /*
     * TrackMouse
     * 
     * Tracks mouse presses.
     * Will only be called during the occupied state.
     */
    void TrackMouse()
    {
        // Get the instant the player presses the mouse button
        if (Input.GetMouseButtonDown(0))
        {
            initialVector = Input.mousePosition;

            if (isDebugging)
            {
                Debug.Log("Barrel - Mouse down at: " + initialVector);
            }
        }

        // Get every frame the mouse button is held down
        if (Input.GetMouseButton(0))
        {
            finalVector = Input.mousePosition;

            PointBarrel();
        }

        if (Input.GetMouseButtonUp(0))
        {
            FireBarrel();
        }
    }

    /*
     * PointBarrel
     * 
     * Point the barrel in the direction the player is dragging cursor.
     * Will only be called in the occupied state.
     */
    void PointBarrel()
    {
        /*
         * If the player doesn't move the mouse after clicking
         * and holding, then the barrel shouldn't do anything
         */
        if (finalVector == initialVector)
        {
            return;
        }

        // Calculations to find the rotation
        Vector2 deltaVector = finalVector - initialVector;
        float angle = Mathf.Atan2(deltaVector.y, deltaVector.x) * Mathf.Rad2Deg;
        barrelTransform.rotation = Quaternion.Euler(0, 0, angle + ROTATION_OFFSET);
    }

    /*
     * FireBarrel
     * 
     * Fires the object inside of the barrel using the cursor
     * vector as direction.
     * Can only be fired during the occupied state.
     * Switches the state to the cooldown state.
     */
    void FireBarrel()
    {
        // Switch state to cooldown state
        currentState = State.cooldown;

        // Get the time the barrel was fired
        firedTime = Time.time;

        // The next time the barrel can be fired
        cooldownTime = firedTime + cooldownDuration;

        /*
         * It is better to fire the ball in the direction
         * the barrel is facing so we can make the firing
         * more general.  It will also help later if we want
         * to make the barrel rotate uncontrollably.
         * 
         * Vector2 fireDirection = barrelTransform.forward;
         */

        // Reactivate the object in the Unity hierarchy so it can interarct
        enteredObject.SetActive(true);

        // Get direction to fire the object
        Vector2 directionVector = initialVector - finalVector;
        directionVector.Normalize();

        // Get object components to fire it
        Transform objectTransform = enteredObject.GetComponent<Transform>();
        Rigidbody2D objectRigidbody2D = enteredObject.GetComponent<Rigidbody2D>();
        objectTransform.position = barrelTransform.position;

        // Fire the object
        objectRigidbody2D.AddForce(directionVector * fireForce, ForceMode2D.Force);
    }

    /*
     * Cooldown
     * 
     * The time it takes for the barrel to be ready to fire again.
     * Will only be called during the cooldown state.
     * Increments the timer until firedTime meets the cooldownTime.
     */
    void Cooldown()
    {
        firedTime += Time.deltaTime;

        if(firedTime >= cooldownTime)
        {
            currentState = State.empty;
        }
    }

    /*
     * Move
     * 
     * Moves the barrel to the next position.
     * Will be called regardless of state.
     */
    void Move()
    {
        // If there is no position to move to, then don't move
        if(positionObjects.Count == 0)
        {
            return;
        }

        // Get the location of the position to move to
        Vector2 targetPosition = positionObjects[positionNumber].transform.position;

        // Move the barrel to that location
        barrelTransform.position = Vector2.MoveTowards(barrelTransform.position, targetPosition, movementSpeed * Time.deltaTime);

        // Don't move onto next position until the barrel has reached target position
        if(!(((Vector2)barrelTransform.position).Equals(targetPosition)))
        {
            return;
        }

        // Get next position
        positionNumber = (positionNumber + 1) % positionObjects.Count;
    }

}
