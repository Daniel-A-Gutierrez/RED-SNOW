using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
 {
	public RaycastHit2D hit;
	public float speedTarget;	
	public LayerMask ground;
	public float virtualMass;
	public float jumpPower;
	public State state;
	DistanceJoint2D dj;
	Rigidbody2D rb;
	GameObject cam;
	GameObject player;
	Vector2 normal;
	bool grounded = true;
	bool dashed = false;
	bool jumped = false;
	float angularSpeedTarget;
	

	// note - the player must be the first object underneath the parent in the hierarchy. The ball must be second. 
	void Start ()
	{
		player = transform.parent.GetChild(0).gameObject;
		if(player.tag!="Player")
		{
			print("error, player isnt player: tag : " + player.tag);
		}
		rb = GetComponent<Rigidbody2D>();
		angularSpeedTarget = -speedTarget/(GetComponent<CircleCollider2D>().radius)*180/3.14159f;
		dj = transform.GetChild(0).GetComponent<DistanceJoint2D>();
		cam = GameObject.FindGameObjectWithTag("MainCamera");
		if(virtualMass == 0)
		{
			virtualMass = 3;
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{// inefficient
		if(collision.gameObject.tag == "Slope")
		{
        	normal = collision.contacts[0].normal;
		}
    }

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(grounded == false)
			{
				grounded = true;
				jumped = false;
				// dj.maxDistanceOnly = false;
				// dj.autoConfigureDistance = false;
				// dj.distance = .64f;
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
		// handle side to side movement
		rb.angularVelocity = angularSpeedTarget;
		if(rb == null)
		{
			rb = GetComponent<Rigidbody2D>();
		}
		if(Input.GetAxis("Horizontal") > .1f &((transform.position - cam.transform.position).x < 6))
		{
			rb.angularVelocity = angularSpeedTarget*(1.75f);
			state.playerLeft = false;
		}
		if(Input.GetAxis("Horizontal") < -.1f & ((transform.position - cam.transform.position).x > -6)  )
		{
			rb.angularVelocity = angularSpeedTarget*(.25f);
			state.playerLeft = true;
		}
		


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
			// dj.maxDistanceOnly = true;
			// dj.autoConfigureDistance = false;
			// dj.distance = 10;
			rb.AddForce(jumpPower*normal*15f/Time.deltaTime);
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
		
		if(jumped==false)
		{
			//virtual mass style  rb.AddForce(-normal*Mathf.Abs((virtualMass - normalImpulse))/Time.deltaTime);
			//rb.AddForce(-normal*(1+hit.distance*hit.distance)*virtualMass/Time.deltaTime);
		}
		

	}

	void Jump()
	{

	}
	//makes the player seem like theyre actually riding a slope. 
	void Update()
	{		
		player.transform.up = normal;
	}

    public void Die()
    {
        Debug.Log("You Died");
    }
}
