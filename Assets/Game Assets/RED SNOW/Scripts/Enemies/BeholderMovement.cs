using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeholderMovement : MonoBehaviour {

	Rigidbody2D rb;
	public LayerMask layer_mask;
	float floatHeight = 5f;
	float horizontalSpeed = 15f;
	float horizontalDistanceBuffer = 10f;
	float oscillationDelta;
	GameObject player;
	
	void Start(){
		rb = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player");
		rb.velocity = new Vector2(horizontalSpeed,0);
	}

	void Update () {
		oscillationDelta = Mathf.Sin(Time.time * 2) * 2.5f;
		CheckHeight();
		CheckHorizontalDistance();
	}

	void CheckHeight(){
		RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 30f, layer_mask);

		if (hitInfo){
			
			transform.position = new Vector2(transform.position.x, hitInfo.point.y + floatHeight + oscillationDelta);
		}
	}

	void CheckHorizontalDistance(){
		float distance_x = transform.position.x - player.transform.position.x;
		if (distance_x >= 0 && distance_x > horizontalDistanceBuffer){
			rb.velocity = new Vector2(-horizontalSpeed, 0);
		}
		else if (distance_x < 0 && Mathf.Abs(distance_x) > horizontalDistanceBuffer + 10f){
			rb.velocity = new Vector2(horizontalSpeed * 2f, 0);
		}
		else
			rb.velocity = new Vector2(horizontalSpeed, 0);
	}
}
