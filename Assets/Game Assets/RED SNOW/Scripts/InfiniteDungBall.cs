using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(Rigidbody2D))]
public class InfiniteDungBall : MonoBehaviour {

	Rigidbody2D rb;
	public float assist = 1;
	public float targetSpeed;
	
	// Update is called once per frame
	void FixedUpdate ()
	{

		
		if(rb == null)
		{
			rb = GetComponent<Rigidbody2D>();
		}
		
		 
		// if(rb.velocity.magnitude < 2 | rb.velocity.x < 0)
		// {
		// 	rb.AddForce(Time.deltaTime*new Vector2(5,5)*assist);
		// 	assist += .5f;
		// }
		// else if(assist>1)
		// {
		// 	assist -=.025f;
		// }
	}

}
