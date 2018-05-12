using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBehavior : MonoBehaviour
{
	public int HP;
	void OnCollisionEnter2D(Collision2D collision)
	{
		// do other stuff that happens when it dies
		if(HP>1)
		{
			HP -= 1;
			// enter invincibility for a couple seconds
		}
		else
		{
			//do death stuff
			transform.parent.parent.GetChild(1).GetComponent<EnemyControl>().Die();
		}
	}

	void OnParticleCollision(GameObject go) {
		Debug.Log(go);
	}
}