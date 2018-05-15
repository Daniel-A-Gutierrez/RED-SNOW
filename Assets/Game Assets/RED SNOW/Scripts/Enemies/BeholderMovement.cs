using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeholderMovement : MonoBehaviour {

	Rigidbody2D rb;
	public LayerMask layer_mask;
	float floatHeight = 5f;
	float horizontalSpeed = 10f;

	void Start(){
		rb = GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2(horizontalSpeed,0);
	}

	// Update is called once per frame
	void Update () {
		CheckHeight();
	}

	void CheckHeight(){
			RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 20f, layer_mask);

			if (hitInfo.distance != floatHeight){
				transform.position = new Vector2(transform.position.x, hitInfo.point.y + floatHeight);
			}

	}
}
