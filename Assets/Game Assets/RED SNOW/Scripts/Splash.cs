using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour {
	public new ParticleSystem particleSystem;
	public double triggerHeight = .5f;
	public double triggerVelocity = 2f;
	private RaycastHit2D hit;
	private LayerMask mask;
	private ParticleSystem.MainModule main;
	private ParticleSystem.EmissionModule em;
	private ParticleSystem.NoiseModule noiseModule;
	private ParticleSystem.ForceOverLifetimeModule fom;
	private Vector2 defaultForce;
	private Rigidbody2D playerRb;
	private BoxCollider2D playerColl;
	private bool splashTriggered = false;

	void Start()
	{
		main = particleSystem.main;
		playerRb = GetComponent<Rigidbody2D>();
		em = particleSystem.emission;
		em.enabled = false;
		noiseModule = particleSystem.noise;
		playerColl = GetComponent<BoxCollider2D>();
		fom = particleSystem.forceOverLifetime;
		defaultForce = new Vector2(fom.x.constant, fom.y.constant);
	}

	void LateUpdate () {
		mask = LayerMask.GetMask("Ground");
        hit = Physics2D.Raycast(transform.position,-Vector3.up, 100, mask);
		//https://answers.unity.com/questions/1210223/particle-system-wont-restart-emitting-before-last.html
		if(hit.distance > triggerHeight && !splashTriggered)
			splashTriggered = true;
		if(hit.distance < triggerHeight && splashTriggered)
			if(-playerRb.velocity.y > triggerVelocity)
				StartCoroutine(SplashEffect());
			else
				splashTriggered = false;
	}

	IEnumerator SplashEffect()
	{
		em.enabled = true;
		fom.x = -playerRb.velocity.x;
		fom.y = -playerRb.velocity.y / 8f;
		main.simulationSpeed = Mathf.Max(playerRb.velocity.magnitude / 10f, 1f);
		main.startLifetime = Mathf.Min(playerRb.velocity.magnitude / 10f, 1f);
		noiseModule.strengthMultiplier = playerRb.velocity.magnitude / 20f;
		em.rateOverTime = Mathf.Exp(playerRb.velocity.magnitude / 8f);
		yield return new WaitForSeconds(main.startLifetime.constant / 3);
		em.enabled = false;
		splashTriggered = false;
		fom.x = defaultForce.x;
		fom.y = defaultForce.y;
	}
}
