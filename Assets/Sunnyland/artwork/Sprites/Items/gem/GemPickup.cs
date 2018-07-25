using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPickup : MonoBehaviour {

    [SerializeField] GameObject pickupEffects;
    [SerializeField] AudioClip pickedUp;
    PlayerScript health;
    Score score;
    bool triggered = false;

	private void Start()
	{
        score = FindObjectOfType<Score>();
        health = FindObjectOfType<PlayerScript>();
	}
	// Update is called once per frame
	void Update () {
        if(triggered){
            Instantiate(pickupEffects, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(pickedUp, Camera.main.transform.position, .5f);
            score.AddToScore(5);
            Destroy(this.gameObject);
        }
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (triggered) { return; }
        if(collision.tag == "Player"){
            triggered = true;
        }
	}
}
