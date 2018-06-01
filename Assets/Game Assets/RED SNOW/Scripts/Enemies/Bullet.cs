using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float speed = 5f;
	Transform player;
	Vector2 pos;
	Vector2 v;
	void OnCollisionEnter2D(Collision2D col){
		Destroy(this.gameObject);
	}

	void Awake(){
		player = GameObject.FindGameObjectWithTag("Player").transform;
		pos = player.position;
		v.x = speed * (pos.x - transform.position.x);
		v.y = speed * (pos.y - transform.position.y);
		GetComponent<Rigidbody2D>().velocity = v;
	}

	void LateUpdate(){
		GetComponent<Rigidbody2D>().velocity = v;	
	}
}
