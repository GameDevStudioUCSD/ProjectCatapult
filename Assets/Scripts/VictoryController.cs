using UnityEngine;
using System.Collections;
/**
 * Filename: VictoryController.cs \n
 * Author: Michael Gonzalez \n
 * Contributing Authors: N/A \n
 * Date Drafted: 8/2/2015 \n
 * Description: A very simple controller script that currently will only 
 *              instantiate a copy of the victory screen when another object
 *              collides with this one.
 */
public class VictoryController : MonoBehaviour {
    /** The victory screen prefab */
    public GameObject victoryScreen;
    /** Spawns a victory screen when an object collides with this */
    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(victoryScreen);
        Invoke("nextLevel", 2.0f);
    }

    void nextLevel()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
