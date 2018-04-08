using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
 {
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
		player = transform.parent.GetChild(0).gameObject;
		if(player.tag!="Player")
		{
			print("error, player isnt player: tag : " + player.tag);
		}
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
			hit = Physics2D.Raycast(transform.position,-Vector3.up, 1000, ground);
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
			if(rb.velocity.y>0)
			{
				rb.AddForce(new Vector2(0,-2*rb.velocity.y/Time.deltaTime));
			}
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
	}
}
