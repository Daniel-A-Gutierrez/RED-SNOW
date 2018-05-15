using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beholder : MonoBehaviour {

	GameObject player;
	LineRenderer lr;
	//Max Laser range
	float attackRange = 15.0f;
	bool canTarget = true;
	//Whether the Beholder has fired its damage laser yet
	bool fired = false;
	public LayerMask layerMask;

	void Start(){
		player = GameObject.FindGameObjectWithTag("Player");
		lr = GetComponent<LineRenderer>();
		lr.startWidth = lr.endWidth = 0.33f;
		lr.positionCount = 1;
	}

	void Update(){
		//Fire laser pointer at player until time to fire
		lr.SetPosition(0, transform.position);
		if (Vector2.Distance(transform.position, player.transform.position) < attackRange && canTarget){
			lr.positionCount = 2;
			lr.SetPosition(1, player.transform.position);
			if (!fired){
				StartCoroutine("LaserDamage");
			}
		}
	}
	

	IEnumerator LaserDamage(){
		//Amount of time spent on target laser
		fired = true;
		yield return new WaitForSeconds(1.25f);
		canTarget = false;
		//Amount of time before the laser damages the last position to give player a window to dodge
		yield return new WaitForSeconds(0.001f);

		//Widens laser to full size
		lr.startWidth = lr.endWidth = 1f;

		//Check if player is in range of the laser
		Vector2 direction = lr.GetPosition(1) - lr.GetPosition(0);
		float distance = Vector2.Distance(lr.GetPosition(0),lr.GetPosition(1));
		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distance, layerMask);
		
		if(hit){
			//Debug.Log("hit");
			player.transform.GetChild(0).GetComponent<PlayerHitBehavior>().Damage(1);
		}

		//Reset so beholder can shoot again
		yield return new WaitForSeconds(0.5f);
		fired = false;
		lr.startWidth = lr.endWidth = 0.33f;
		lr.positionCount = 1;
		canTarget = true;		
	}
}
