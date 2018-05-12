using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketItemBehavior : MonoBehaviour {
	private GameObject player;
	private State state;
	void Awake() {
		state = GameObject.FindGameObjectWithTag("Player").GetComponent<State>();
	}
	void OnTriggerEnter2D(Collider2D col) {
		if(state.boostDelay && col.gameObject.tag == "Player") {
			state.isBoosted = true;
			state.boostDelay = false;
			Destroy(gameObject);
		}
	}
}
