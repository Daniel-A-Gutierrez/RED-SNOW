using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float speed = 7.5f;
	Transform player;
	Vector2 pos;
	void OnCollisionEnter2D(Collision2D col){
		Destroy(this.gameObject);
	}

	void Awake(){
		player = GameObject.FindGameObjectWithTag("Player").transform;
		pos = player.position;
		pos += player.GetComponent<Rigidbody2D>().velocity;

	}

	void Update(){
		transform.position = Vector2.MoveTowards(transform.position, pos, speed * Time.deltaTime);
	}
}
