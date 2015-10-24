using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {
    /* used jointly with Switch.cs, place switch on a Switcher,
    * drag all GameObjects activated/deactivated by the switch
    * into the Switch componoent of the switcher, make sure they
    * have a Activator component
    */


    public GameObject[] objectToBeActivated;
    private Activator[] act;

    // Use this for initialization
    void Start()
    {
        act = new Activator[objectToBeActivated.Length];
        for (int i = 0; i < objectToBeActivated.Length; i++)
        {
           int hash = objectToBeActivated[0].transform.GetHashCode();
           // Debug.Log("Activator " + i + " hashcode: " + hash);
            act[i] = (objectToBeActivated[i]).GetComponent<Activator>() as Activator;
        }
    }



    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("Activator touched");
          if (other.gameObject.tag == "Player" )
          {
              for (int i = 0; i < act.Length ; i++)
                  act[i].Activate();
          }
    }


}
