using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour {
	private Rigidbody2D playerRb;
	void Start()
	{
		playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
	}
	public void beginSpray()
	{
		StartCoroutine(BloodSpray());
	}
	private IEnumerator BloodSpray()
	{
		transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_StartTime", Time.time / 20);
		transform.parent = transform.parent.parent;
		ParticleSystem spray = transform.GetChild(1).GetComponent<ParticleSystem>();
		ParticleSystem.ShapeModule shape = spray.shape;
		shape.radius *= playerRb ? playerRb.velocity.magnitude / 5f : 1f;
		yield return new WaitForSeconds(.25f);
		spray.Stop();
		yield return new WaitForSeconds(1f);
		Destroy(gameObject.transform.parent.gameObject);
	}
}
