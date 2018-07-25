using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Score : MonoBehaviour {

    Text scoreText;
    int currentscore;

	void Start () {
        scoreText = GetComponent<Text>();
        currentscore = 0;

	}
	
	// Update is called once per frame
	void Update () {
        UpdateScore();
	}
    public void UpdateScore(){
        scoreText.text = "Score:" + currentscore;
    }
    public void AddToScore(int score){
        currentscore = currentscore + score;
    }
}

