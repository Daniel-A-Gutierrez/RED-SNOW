using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeManager : MonoBehaviour {

	GameObject player;
	public GameObject toInstantiate;
	public int score;
	List<Transform> slopes;
	public float slopeLengthFactor;
	public float slopeDeclineFactor;
	public float slopeTurbulenceFactor;
	public float renderDistance;
	public GameObject[] enemies;
	public float spawnChance;
	//0 to 1
	public int enemyCount;

	float originalRD;
	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		slopes = new List<Transform>();
		GameObject[] tarray =  GameObject.FindGameObjectsWithTag("Slope");
		foreach (GameObject g in tarray)
		{
			slopes.Add(g.transform);
		}
		originalRD = renderDistance;
	}

	void Update()
	{
		renderDistance = originalRD*transform.localScale.magnitude;
	}

	void CreateSlope(Vector2 p0, float density)
	{
		GameObject newSlope = Instantiate(toInstantiate,p0,Quaternion.identity);
		
		BezierCollider2D bezier = newSlope.GetComponent<BezierCollider2D>();
		BezierCollider2D previousBezier = slopes[slopes.Count-1].GetComponent<BezierCollider2D>();
		Vector2 d00 = previousBezier.getLastDerivative();
		Vector2 d3 = previousBezier.secondPoint;
		float decline = slopeDeclineFactor*Random.Range(-.2f,1.5f);
		float length = slopeLengthFactor*Random.Range(.25f,2);
		float turbulence = slopeTurbulenceFactor*Random.Range(0,2);
		float deviation1 = Random.Range(.2f,.7f);
		float deviation2 = Random.Range(deviation1,.8f);
		float regressive1 = Random.Range(-length*turbulence,length*turbulence);
		float regressive2 = Random.Range(-length*turbulence,length*turbulence);

		bezier.firstPoint = new Vector2(0,0);
		bezier.handlerFirstPoint = deviation1*length*d00.normalized;
		bezier.handlerSecondPoint = new Vector2(length*deviation2,length*decline*deviation2+regressive2);
		bezier.secondPoint = new Vector2(length,decline *length);
		bezier.pointsQuantity = (int)(density*(length));

		Vector2[] points = newSlope.GetComponent<BezierCollider2D>().calculate2DPoints();
		newSlope.GetComponent<EdgeCollider2D>().points = points;
		slopes.Add(newSlope.transform);

		if(enemyCount < 15)
		{
			int spawnEnemy = (int)(spawnChance / Random.Range(0,1f));
			enemyCount += spawnEnemy;
			if(spawnEnemy>5)
			{
				spawnEnemy = 5;
			}
			if(spawnEnemy>0)
			{
				// get a list of all the points on the slope,
				// pick spawnEnemy number randomly
				// spawn random Enemies from Enemies at those points. 
				List<int> pointIndex = new List<int>();
				for(int i = 0 ; i < bezier.pointsQuantity ; i += 10)
				{
					pointIndex.Add(i);
				}
				
				for(int i = 0 ; i < spawnEnemy ; i++)
				{
					Vector2 spawn = points[pointIndex[(int)Random.Range(0,pointIndex.Count) ] ] + new Vector2(0,.12f) + (Vector2)newSlope.transform.position ;
					GameObject go = enemies[(int)Random.Range(0,enemies.Length)];
					Instantiate (go, spawn,Quaternion.identity);
				}

			}
		}
	}

	// basically if the character gets within render distance
	//of the next chunk, generate a chunk that starts on the
	// end point of the last one and has procedurally generated
	//attributes. 
	void FixedUpdate ()
	{
		score = (int)transform.position.magnitude;
		if(slopes[slopes.Count-1].position.x-player.transform.position.x < renderDistance)
		{

			CreateSlope(slopes[slopes.Count-1].GetComponent<BezierCollider2D>().secondPoint + 
			new Vector2 ( slopes[slopes.Count-1].transform.position.x ,slopes[slopes.Count-1].transform.position.y) , 7.5f);
		}
		if(player.transform.position.x - slopes[0].position.x > renderDistance)
		{
			GameObject obj = slopes[0].gameObject;
			slopes.RemoveAt(0);
			Destroy(obj);
		}
	}
}
