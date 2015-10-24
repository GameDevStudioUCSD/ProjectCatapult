using UnityEngine;
using System.Collections;

public class Activator : MonoBehaviour {
    /* used jointly with Switch.cs, place switch on a Switcher,
     * drag all GameObjects activated/deactivated by the switch
     * into the Switch componoent of the switcher, make sure they
     * have a Activator component
     */


    // reverseActivate: if true, isActive->!isActive upon every call of Activate()
    //if false, isActive remain false if it is already false
    public bool reverseActivate; 
    //isActive: initially active
    public bool isActive;

	void Start () {
        reverseActivate = false;
        isActive = true;
    }
	

    // called when Switch is touched by placer 
    // put stuff that will happen(on/off) on DOSOMETHING() at the end 
   public void Activate()
    {
        if (reverseActivate)
        {
            isActive = !isActive;
            DoSomething();
        }
        else
        {
            isActive = false;
            DoSomething();
        }
         
    }

    
    private void DoSomething()
    {
      //  transform.Translate(new Vector3(1, 1, 0));
    }


}
