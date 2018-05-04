using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ZoomInEffect : MonoBehaviour {
	private new Camera camera;
	private Rect camrect;
	private float sz = .1f;
	void Start()
	{
		camera = Camera.main;
		camera.rect = new Rect(0, 0, sz, sz);
		camera.orthographicSize = 1f;
		StartCoroutine(UpdateRect());
	}
	IEnumerator UpdateRect()
	{
		while(sz < 1f)
		{
			sz += .01f;
			if(camera.orthographicSize < 7f)
				camera.orthographicSize += 1;
			camera.rect = new Rect(0, 0, sz, sz);
			yield return new WaitForSeconds(.01f);
		}
	}
}
