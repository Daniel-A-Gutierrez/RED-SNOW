using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
 {
	Rigidbody2D rb ;
    float AngularSpeedTarget;
	float angularV;
	public float speedTarget;
	// Use this for initialization
	//turns out the camera views 10 in y dir and 17.77 in x dir. 
	//i should set bounds relative to those numbers.
	void Start ()
	{
		AngularSpeedTarget = -speedTarget/(GetComponent<CircleCollider2D>().radius)*180/3.14159f;
	}
	// Update is called once per frame
	void FixedUpdate ()
	{
		if(rb == null)
		{
			rb = GetComponent<Rigidbody2D>();
		}
		rb.angularVelocity = AngularSpeedTarget;
		
		
		//rb.AddForce(getNormal().normalized*700*Time.deltaTime);
	}
}
