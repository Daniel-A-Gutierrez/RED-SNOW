using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour {

	public void beginSpray()
	{
		StartCoroutine(BloodSpray());
	}
	private IEnumerator BloodSpray()
	{
		transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_StartTime", Time.time / 20);
		transform.parent = transform.parent.parent;
		yield return new WaitForSeconds(1.5f);
		transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
		Destroy(gameObject.transform.parent.gameObject);
	}
}
