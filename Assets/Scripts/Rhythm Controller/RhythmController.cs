﻿using UnityEngine;
using System.Collections.Generic;

/**
 * Filename: RhythmController.cs \n
 * Author: Jason Wu \n
 * Contributing Authors: Michael Gonzalez and Sean Wenzel \n
 * Date Drafted: 10/10/2015 \n
 * Description: Plays and monitors the MusicalTrack, and calls \n
 * 				the RhythmEvents. \n
 */
public class RhythmController : MonoBehaviour {
	private float wholeNote;
	private float halfNote;
	private float quarterNote;
	private float eigthNote;
    private float sixteenthNote;
    private float tripleWholeNote;
    private float tripleHalfNote;
    private float tripleQuarterNote;
    private float tripleEigthNote;
    private float measureLength;

	public MusicalTrack[] channel1TrackList;
	public MusicalTrack[] channel2TrackList;


    private MusicalTrack song1;
    private MusicalTrack song2;
    [Range(0,7)]
    public int songIndex = 0;
    public float errorMargin = 1f;
    public float swapSpeed = 5f;
    public bool isDebugging;
    public bool isIntro = false;

    private List<int> measureKeys;
    private SortedDictionary<int, List<float>> measureTimeKeys;
    private SortedDictionary<int, SortedDictionary<float, List<RhythmEvent>>> events;
    private AudioSource channel1;
	private AudioSource channel2;
	private AudioSource activeChannel;
    private List<RhythmEvent> eventList;

    private bool shouldDestroy = false;

    static RhythmController singleton = null;

	int currentMeasure;
	int currentBeat;

	/*Internals*/
	MusicalTrack currentTrack;

	float startTime;
    float destPitch;

    public void StopSong()
    {
        channel1.Stop();
        channel2.Stop();
    }
    public void PlaySong()
    {
        channel1.Play();
        channel2.Play();
    }
    public void PlaySong(int i )
    {
        songIndex = i;
        SetupSong();
        StopSong();
        PlayTransition(song1);
    }

    // Preserves position in song
    public void SwapToSong(int i)
    {
        float song1Len = channel1TrackList[songIndex].bpm;
        float song2Len = channel1TrackList[i].bpm;
        float songRatio = song1Len / song2Len;
        float currentTime = channel1.time;
        PlaySong(i);
        PlaySong();
        channel1.time = currentTime * songRatio;
        channel2.time = currentTime * songRatio;
    }

    public static RhythmController GetController()
    {
        GameObject controller = GameObject.Find(Names.RHYTHMCONTROLLER);
        return controller.GetComponent<RhythmController>();
    }
    /**
     * Function Name: SwapChannel()
     * Description: This function will tell the rhythm controller to switch the
     *              channel it is currently playing music on. This allows us to
     *              use dynamic music. 
     */
    public void SwapChannel()
    {
        if (activeChannel == channel1)
        {
            PlayTransition(song1);
            activeChannel = channel2;
            destPitch = channel2TrackList[songIndex].pitch;
        }
        else
        {
            PlayTransition(song2);
            activeChannel = channel1;
            destPitch = channel1TrackList[songIndex].pitch;
        }
    }
    private void PlayTransition(MusicalTrack song)
    {
        if (song == null)
            return;
        if (song.transition != null)
        {
            StopSong();
            activeChannel.PlayOneShot(song.transition);
            Invoke("PlaySong", song.transition.length);
        }
    } 
    public void SwitchToChannel( int channel )
    {
        if( channel == 1 )
        {
            if (activeChannel == channel2)
                SwapChannel();
        }
        else if( activeChannel == channel1 )
        {
            SwapChannel();
        }
    }
    public float GetPitch()
    {
        return activeChannel.pitch;
    }
    /**
     * Function Name: ChannelLerp() \n
     * Description: This function will smoothly fade between the two channels
     *              on the rhythm controller. It should be called at the top of
     *              update to work correctly
     */
    void ChannelLerp() {
            if( activeChannel == channel1) {
                channel1.volume = Mathf.Lerp(channel1.volume, 1f, swapSpeed * Time.deltaTime);
                channel2.volume = Mathf.Lerp(channel2.volume, 0f, swapSpeed * Time.deltaTime);
            }
            if( activeChannel == channel2) {
                channel1.volume = Mathf.Lerp(channel1.volume, 0f, swapSpeed * Time.deltaTime);
                channel2.volume = Mathf.Lerp(channel2.volume, 1f, swapSpeed * Time.deltaTime);
            }
        channel1.pitch = Mathf.Lerp(activeChannel.pitch, destPitch, swapSpeed * Time.deltaTime);
        channel2.pitch = Mathf.Lerp(activeChannel.pitch, destPitch, swapSpeed * Time.deltaTime);
    }

	// Use this for initialization
	void Start () {
        // Create collection objects
        if(eventList == null)
            eventList = new List<RhythmEvent>(); 
        events = new SortedDictionary<int, SortedDictionary<float, List<RhythmEvent>>>();
		foreach( Transform channel in transform) {
			if(channel.gameObject.name == "Channel 1")
				channel1 = channel.gameObject.GetComponent<AudioSource>();
			if(channel.gameObject.name == "Channel 2")
				channel2 = channel.gameObject.GetComponent<AudioSource>();
		}
        SetupSong();
        if(isDebugging)
		    DebugLengths ();
        if (isIntro)
            DontDestroyOnLoad(this.gameObject);
	}

    void SetupSong()
    {
        measureKeys = new List<int>();
        measureTimeKeys = new SortedDictionary<int, List<float>>();
        song1 = channel1TrackList[songIndex];
        song2 = channel2TrackList[songIndex];
        currentTrack = channel1TrackList[songIndex];
        channel1.clip = currentTrack.song;
        channel2.clip = channel2TrackList[songIndex].song;
        channel1.pitch = channel1TrackList[songIndex].pitch;
        destPitch = channel1.pitch;
        channel2.volume = 0f;
        SetNoteLengths();
        activeChannel = channel1;
        foreach (RhythmEvent e in eventList)
            RegisterEventWithController(e);
        PlaySong();
    }
	
	// Update is called once per frame
	void Update () {
        if (channel1 == null)
            return;
        ChannelLerp(); 
        foreach (int measure in measureKeys)
        {
			float t = activeChannel.time * 1000 * activeChannel.pitch; // t == time
            if ((int)(t / measureLength) % measure == 0)
            { // We know that we're in an appropriate measure to call methods on
                List<float> timeKeys = measureTimeKeys[measure];
                SortedDictionary<float, List<RhythmEvent>> eventMap = events[measure];
                foreach (float timeKey in timeKeys)
                {
                    if (WithinErrorMargin(t % timeKey, timeKey)) {
                        List<RhythmEvent> eventList = eventMap[timeKey];
                        foreach (RhythmEvent e in eventList)
                        {
                            float ms = t / activeChannel.pitch;
                            if(ms - e.GetLastInvokeTime() > errorMargin * 5 || e.GetLastInvokeTime() > ms)
                            {
                                e.OnEvent.Invoke();
                                e.SetLastInvokeTime(ms); 
                            }
                        }
                    }
                }
            }
        }
	}

    void SetNoteLengths() { /** multiply by 1000 to convert to milliseconds*/
        wholeNote = 60f * 1000f / currentTrack.bpm * 4;
        halfNote = 60f * 1000f / currentTrack.bpm * 2;
        quarterNote = 60f * 1000f / currentTrack.bpm;
        eigthNote = 60f * 1000f / currentTrack.bpm / 2;
        sixteenthNote = 60f * 1000f / currentTrack.bpm / 4;
        tripleWholeNote = 60f * 1000f / currentTrack.bpm * 4 / 3;
        tripleHalfNote = 60f * 1000f / currentTrack.bpm * 2 / 3;
        tripleQuarterNote = 60f * 1000f / currentTrack.bpm / 3;
        tripleEigthNote = 60f * 1000f / currentTrack.bpm / 6;
        measureLength = quarterNote * currentTrack.timeSigUpper;
    }

	void DebugLengths(){
		Debug.Log ("quarter note " + quarterNote);
		Debug.Log ("half note " + halfNote);
		Debug.Log ("eigth note " + eigthNote);
		Debug.Log ("whole note " + wholeNote);
		Debug.Log ("triple eigth note" + tripleEigthNote);
		Debug.Log ((300000%tripleEigthNote));
		Debug.Log (WithinErrorMargin (300000 % tripleEigthNote, tripleEigthNote));
	}
	/** workaround to margins of error involving modulo with floating point numbers*/
	bool WithinErrorMargin( float modResult, float noteLength ){
		if (modResult < errorMargin || noteLength - modResult < errorMargin) {
			return true;
		}
		return false;
	}

    public void RegisterEvent(RhythmEvent e)
    {
        if (eventList == null)
            eventList = new List<RhythmEvent>();
        eventList.Add(e);
        RegisterEventWithController(e);
    }
    private void RegisterEventWithController(RhythmEvent e)
    {
        float noteLength = GetNoteLength(e.noteDivision);
        int measure = e.measureSeparation;
        // Add measure key to measure list
        if (!measureKeys.Contains(measure)) {
            measureKeys.Add(measure);
        }
        // Instantiate potentially null containers
        if (!measureTimeKeys.ContainsKey(measure) ) {
            measureTimeKeys[measure] = new List<float>();
        }
        List<float> timeKeysList = measureTimeKeys[measure];
        // Associate timekey to a measure
        if (!timeKeysList.Contains(noteLength)) {
            timeKeysList.Add(noteLength);
        }
        // Instantiate potentially null containers
        if (!events.ContainsKey(measure)) {
            events[measure] = new SortedDictionary<float, List<RhythmEvent>>();
        }
        SortedDictionary<float, List<RhythmEvent>> eventsAtMeasure = events[measure];
        if (!eventsAtMeasure.ContainsKey(noteLength)) {
            eventsAtMeasure[noteLength] = new List<RhythmEvent>();
        }
        List<RhythmEvent> eventList = eventsAtMeasure[noteLength];
        // Finally, we add the event
        eventList.Add(e);

    }
    float GetNoteLength(NoteDivision note)
    {
        switch (note)
        {
            case NoteDivision.eighthNote:
                return eigthNote;
            case NoteDivision.halfNote:
                return halfNote;
            case NoteDivision.quarterNote:
                return quarterNote;
            case NoteDivision.wholeNote:
                return wholeNote;
            case NoteDivision.sixteenthNote:
                return sixteenthNote;
            case NoteDivision.tripleEigthNote:
                return tripleEigthNote;
            case NoteDivision.tripleQuarterNote:
                return tripleQuarterNote;
            case NoteDivision.tripleHalfNote:
                return tripleHalfNote;
            case NoteDivision.tripleWholeNote:
                return tripleWholeNote;
        }
        Debug.LogError("Check the GetNoteLength Function");
        return -1; // something really bad happened if we get here
    }
    void OnLevelWasLoaded(int level)
    {
        if (isIntro)
        {
            isIntro = false;
            shouldDestroy = true;
            return;
        }
        if (shouldDestroy)
            Destroy(this.gameObject);
        
        if(channel1 != null)
            channel1.Stop();
        if(channel2 != null)
		    channel2.Stop();
    }
  
}
