using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXDestroy : MonoBehaviour {

    [SerializeField] float destroyTime;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Destroy(this.gameObject, destroyTime);
	}
}
