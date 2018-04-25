using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SlowdownFlush : MonoBehaviour {
	public double stopHeight = 2;
	public State playerState;
	private new ParticleSystem particleSystem;
	private RaycastHit2D hit;
	private LayerMask mask;
	private ParticleSystem.MainModule main;
	private ParticleSystem.EmissionModule em;
	private ParticleSystem.NoiseModule noiseModule;
	private Rigidbody2D playerRb;
	private Vector2 prevVelocity;
	// Update is called once per frame
	void Awake()
	{
		particleSystem = GetComponent<ParticleSystem>();
		main = particleSystem.main;
		playerRb = GetComponentInParent<Rigidbody2D>();
		em = particleSystem.emission;
		noiseModule = particleSystem.noise;
	}
	void LateUpdate () {
		mask = LayerMask.GetMask("Ground");
        hit = Physics2D.Raycast(transform.position,-Vector3.up, 100, mask);
		main.simulationSpeed = Mathf.Max(playerRb.velocity.magnitude / 10f, 2f);
		main.startLifetime = Mathf.Min(playerRb.velocity.magnitude / 10f, 1f);
		noiseModule.strengthMultiplier = playerRb.velocity.magnitude / 20f;
		//https://answers.unity.com/questions/1210223/particle-system-wont-restart-emitting-before-last.html
		if(hit.collider == null || hit.distance > stopHeight || !playerState.playerLeft)
			em.enabled = false;
		else if(!em.enabled)
			em.enabled = true;
	}
}