using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
 {

	// so with this script
	/* i want jumping to be as follows : enable max distance only on the dist joint,
	disable auto configure distance
	set max distance to like 100,
	apply a force in the opposite direction of the local transform  of the guidewheel,
	while the button is still pressed down and the player has not contacted the ground keep same gravity, 
	if they release the button before hitting the ground increase gravity 2x,
	if they hit the ground set the distance joint back to its original constrant distance and disable max distance only and reenable auto distance and move the guideball closer.
	
	as for movement
	i can add hitboxes to the player and the camera such that they only interact with eachother. this can be a layer exclusive to enemies, the player, etc. 
	these hitboxes will be rectangle colliders, and use follow, not be directly on the body. They can follow the gameobject and pass their collisions to the parent. 

	collision layers
	The ground impacts items, hazards, bullets, characterupperbody, wheel
	the wheel only impacts the ground. 
	character bump hits itself and the screen
	character upper body hits bullets, hazards, items, and the ground. 
	the screen hits character bump, and bullets. 

	the player
	I really want the player to have the ability to get the normal of the slope. maybe i can use the mesh...? 
	but i kindof need that to be able to orient the player correctly. and while theyre in the air...  just orient them by physics? 
	maybe if i have beziercollider2d have a function that takes transform data on collision with anything that touches it and outputs a vector2 normal to 
	the surface at that point. 
	What if i also have an 'upper body' hitbox on the player opposite to the wheels, which, if it contacts the ground, makes you 'wipeout' and lose hp. if you had 1hp you 
	die, more and you recover. 
	im thinkin i should differentiate player bump and enemy bump.

	so todays work order : build the player
	step 1 : create a normal retrieving function into bezier collider
	step 2 : create a parent-child hierarchy on the wheel which recieves information from the slope on how to orient itself on the ground
	step 3 : create a movement system with that hierarchy considering the camera follow
	step 4 : create an algorithm for camera behavior on contact with its colliders. 
	 */
	GameObject guideWheel;
	DistanceJoint2D dj;
	Rigidbody2D rb;
	BallMovement bm;
	GameObject cam;
	public LayerMask ground;
	public float gravityMulti;
	Collider2D[] colliders;
	GameObject player;
	Vector2 normal;
	bool grounded = true;
	public RaycastHit2D hit;
	bool dashed = false;
	bool jumped = false;
	public float speedTarget;	
	float angularSpeedTarget;

	void Start ()
	{
		player = transform.parent.Find("Player").gameObject;
		bm = GetComponent<BallMovement>();
		rb = GetComponent<Rigidbody2D>();
		angularSpeedTarget = -speedTarget/(GetComponent<CircleCollider2D>().radius)*180/3.14159f;
		dj = transform.GetChild(0).GetComponent<DistanceJoint2D>();
		cam = GameObject.FindGameObjectWithTag("MainCamera");
	}

	void OnCollisionStay2D(Collision2D collision)
	 {
        normal = collision.contacts[0].normal;
    }

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(grounded == false)
			{
				grounded = true;
				jumped = false;
				dj.maxDistanceOnly = false;
				dj.autoConfigureDistance = false;
				dj.distance = .6f;
			}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if(jumped)
		{
			grounded = false;
		}
	}

	
	void FixedUpdate ()
	{
		rb.angularVelocity = angularSpeedTarget;
		if(rb == null)
		{
			rb = GetComponent<Rigidbody2D>();
		}
		if(Input.GetAxis("Horizontal") > .1f &((transform.position - cam.transform.position).x < 6))
		{
			rb.angularVelocity = angularSpeedTarget*(1.75f);
		}
		if(Input.GetAxis("Horizontal") < -.1f & ((transform.position - cam.transform.position).x > -6)  )
		{
			rb.angularVelocity = angularSpeedTarget*(.25f);
		}
		//i wonder if these transforms are affected by the local scale... 
		// if(Mathf.Abs( CalculateZoom(transform.position , cam.transform.position, new Vector2(12,6))) > 1)
		// {
		// 	cam.GetComponent<Camera>().orthographicSize = CalculateZoom(transform.position , cam.transform.position, new Vector2(12,6) ) *6;
		// }
		
/* i want jumping to be as follows : enable max distance only on the dist joint,
	disable auto configure distance
	set max distance to like 100,
	apply a force in the opposite direction of the local transform  of the guidewheel,
	while the button is still pressed down and the player has not contacted the ground keep same gravity, 
	if they release the button before hitting the ground increase gravity 2x,
	if they hit the ground set the distance joint back to its original constrant distance and disable max distance only and reenable auto distance and move the guideball closer.
		
 */
 		if(grounded == false)
		{
			hit = Physics2D.Raycast(transform.position,-Vector3.up, 1000, ground) ;
			if (hit.collider!=null)
			{
				dj.distance = 3*hit.distance+.2f;
			}
		}

		if(Input.GetKeyDown(KeyCode.Space)& grounded == true)
		{
			grounded = false;
			dj.maxDistanceOnly = true;
			dj.autoConfigureDistance = false;
			dj.distance = 10;
			rb.AddForce(normal*15f/Time.deltaTime);
			dashed = false;
			jumped = true;
		}
		// i think this would be better with a coroutine that handles the transform. 
		if(Input.GetKey(KeyCode.A)&Input.GetKey(KeyCode.LeftShift)& grounded == false & dashed == false)
		{
			rb.AddForce( new Vector2(-1,-1)*25f/Time.deltaTime);
			dashed = true;
		}

		

	}

	void Jump()
	{

	}

	void LateUpdate()
	{
		
		player.transform.up = normal;
		//player.GetComponent<Follower>().offset = normal/transform.parent.localScale.magnitude*.9f;
	}
}
