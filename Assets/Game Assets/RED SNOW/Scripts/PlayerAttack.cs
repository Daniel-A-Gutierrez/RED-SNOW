using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
	Collider2D attackcol;
	Animator myanimator;

	// Use this for initialization
	void Start () {
		attackcol = GameObject.Find ("Triangle").GetComponent<Collider2D>();
		myanimator = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.X)) {
			myanimator.SetTrigger ("attack");
		}
	}
	void Attack()
	{
			attackcol.enabled = true;
	}

	void NoAttack()
	{
			attackcol.enabled = false;
			myanimator.SetTrigger ("attack");
	}

}
