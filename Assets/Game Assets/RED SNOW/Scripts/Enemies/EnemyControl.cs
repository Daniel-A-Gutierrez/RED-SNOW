using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour 
{

	public RaycastHit2D hit;
	public float speedTarget;	
	public LayerMask ground;
	public float forwardSpeedModifier;
	public float backwardSpeedModifier;
	public float jumpForce;
	 
	GameObject player;
	GameObject guideWheel;
	DistanceJoint2D dj;
	Rigidbody2D rb;
	GameObject cam;
	GameObject enemy;
	Vector2 normal;
	bool grounded = true;
	bool dashed = false;
	bool jumped = false;
	float angularSpeedTarget;

	float inputAxis;
	bool jump= false;
	float spawnTime;
	

	// note - the player must be the first object underneath the parent in the hierarchy. The ball must be second. 
	void Start ()
	{
		enemy = transform.parent.GetChild(0).gameObject;
		if(enemy.tag!="Enemy")
		{
			print("error, the enemy object has the wrong tag : " + enemy.tag);
		}
		rb = GetComponent<Rigidbody2D>();
		angularSpeedTarget = -speedTarget/(GetComponent<CircleCollider2D>().radius)*180/3.14159f;
		dj = transform.GetChild(0).GetComponent<DistanceJoint2D>();
		player= GameObject.FindGameObjectWithTag("Player");
		//cam = GameObject.FindGameObjectWithTag("MainCamera");
		spawnTime = Time.time;
	}



	void OnCollisionStay2D(Collision2D collision)
	{
		if(collision.contacts != null && collision.contacts.Length>0)
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


	public void SetInputAxis(float value)
	{
		inputAxis = value;
	}

	public void Jump()
	{
		jump = true;
	}
	
	void FixedUpdate ()
	{
		// handle side to side movement
			if(Time.time - spawnTime > 3)
			{
			rb.angularVelocity = angularSpeedTarget;
			if(rb == null)
			{
				rb = GetComponent<Rigidbody2D>();
			}
			if(inputAxis> .1f)
			{
				rb.angularVelocity = angularSpeedTarget*(1 + forwardSpeedModifier);
			}
			if(inputAxis< -.1f )
			{
				rb.angularVelocity = angularSpeedTarget*( 1 - backwardSpeedModifier);
			}
			if(grounded == false)
			{
				hit = Physics2D.Raycast(transform.position,-Vector3.up, 1000, ground);
				if (hit.collider!=null)
				{
					dj.distance = 3*hit.distance+.2f;
				}
			}

			// handle jumping
			if(jump & grounded == true)
			{
				jump = false;
				grounded = false;
				//dj.maxDistanceOnly = true;
				//dj.autoConfigureDistance = false;
				//dj.distance = 10;
				rb.AddForce(normal*jumpForce/Time.deltaTime);
				dashed = false;
				jumped = true;
			}

			if(player.transform.position.y - transform.position.y > 100)
			{
				Die();
			}
		}
	}

	public void Die()
	{
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SlopeManager>().enemyCount --;
		GameObject splatter = transform.parent.GetChild(0).GetChild(1).gameObject;
		splatter.SetActive(true);
		splatter.GetComponent<BloodSplatter>().beginSpray();
		Destroy(gameObject.transform.parent.GetChild(0).gameObject);
		Destroy(gameObject);
	}

	

	//makes the player seem like theyre actually riding a slope. 
	void LateUpdate()
	{		
		enemy.transform.up = normal;
	}
}
