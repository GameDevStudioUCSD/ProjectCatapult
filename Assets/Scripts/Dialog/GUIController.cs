﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIController : MonoBehaviour {

	private Text leftText;
	private Text rightText;
	private Image leftImage;
	private Image rightImage;

	// Use this for initialization
	void Start () {
		Text[] texts = this.GetComponentsInChildren<Text> ();
		leftText = texts[0];
		rightText = texts [1];
		Image[] images = this.GetComponentsInChildren<Image> ();
		leftImage = images [0];
		rightImage = images [1];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void leftSay(string text) {
        rightText.enabled = false;
        leftText.enabled = true;
		leftText.text = text;
	}

	public void rightSay(string text) {
        leftText.enabled = false;
        rightText.enabled = true;
		rightText.text = text;
	}
}
