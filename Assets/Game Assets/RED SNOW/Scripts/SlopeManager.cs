using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeManager : MonoBehaviour
{
    GameObject player;
    bool lastPitfall = true;
    public GameObject pitfallPrefab;
    public GameObject slopePrefab;
    public int score;
    List<Transform> slopes;
    public float slopeLengthFactor;
    public float slopeDeclineFactor;
    public float slopeTurbulenceFactor;
    public float renderDistance;
    public GameObject[] enemies;
    public float spawnChance;    //0 to 1
    public int enemyCount;
    public GameObject boulder; // ask about this - HLE
    public GameObject rocketBoostItem;
    public float itemChance;
    public float obstacleChance;
    public float pitfallChance ;
    float originalRD;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Finish");
        slopes = new List<Transform>();
        GameObject[] tarray = GameObject.FindGameObjectsWithTag("Slope");
        foreach (GameObject g in tarray)
        {
            slopes.Add(g.transform);
        }
        originalRD = renderDistance;
    }

    void Update()
    {
        renderDistance = originalRD * transform.localScale.magnitude;
    }

    
	// basically if the character gets within render distance
	//of the next chunk, generate a chunk that starts on the
	// end point of the last one and has procedurally generated
	//attributes. If the first element in slopes is too far behind, 
    //delete it. 
	void FixedUpdate ()
	{
		score = (int)transform.position.magnitude;
		if(slopes[slopes.Count-1].position.x-player.transform.position.x < renderDistance)
		{
            //creates a slope 4 to 9 to the right and 4 to 13 below where itd normally be
			if ( !lastPitfall & Random.Range(0f,1f) < pitfallChance) {
				CreateSlope (slopes [slopes.Count - 1].GetComponent<BezierCollider2D> ().secondPoint +
				    new Vector2 (slopes [slopes.Count - 1].transform.position.x, slopes [slopes.Count - 1].transform.position.y), 7.5f
                    ,true, Random.Range(8,16) );
                lastPitfall = true;
			} 
            else
            {
				CreateSlope (slopes [slopes.Count - 1].GetComponent<BezierCollider2D> ().secondPoint +
				new Vector2 (slopes [slopes.Count - 1].transform.position.x, slopes [slopes.Count - 1].transform.position.y), 7.5f);
                lastPitfall = false;
			}
		}
		if(player.transform.position.x - slopes[0].position.x > renderDistance)
		{
			GameObject obj = slopes[0].gameObject;
			slopes.RemoveAt(0);
			Destroy(obj);
		}
	}

    void CreateSlope(Vector2 p0, float density, bool Pitfall = false, float length = 0)
    {
        GameObject newSlope;
        if(Pitfall)
        {
            newSlope = Instantiate(pitfallPrefab, p0, Quaternion.identity);            
        }
        else
        {
            newSlope = Instantiate(slopePrefab, p0, Quaternion.identity);
        }

        BezierCollider2D bezier = newSlope.GetComponent<BezierCollider2D>();
        BezierCollider2D previousBezier = slopes[slopes.Count - 1].GetComponent<BezierCollider2D>();
        Vector2 d00 = previousBezier.getLastDerivative();
        float decline = slopeDeclineFactor * Random.Range(.5f, 2f);
        if(length == 0)
        {
            length = slopeLengthFactor * Random.Range(.25f, 2);
            decline = slopeDeclineFactor * Random.Range(-.2f, 2f);
        }
        float turbulence = slopeTurbulenceFactor * Random.Range(0, 2);
        float deviation1 = Random.Range(.2f, .7f);
        float deviation2 = Random.Range(deviation1, .8f);
        float regressive2 = Random.Range(-length * turbulence, length * turbulence);

        bezier.firstPoint = new Vector2(0, 0);
        bezier.handlerFirstPoint = deviation1 * length * d00.normalized;
        bezier.handlerSecondPoint = new Vector2(length * deviation2, length * decline * deviation2 + regressive2);
        bezier.secondPoint = new Vector2(length, decline * length);
        bezier.pointsQuantity = (int)(density * (length));

        Vector2[] points = newSlope.GetComponent<BezierCollider2D>().calculate2DPoints();
        newSlope.GetComponent<EdgeCollider2D>().points = points;
        slopes.Add(newSlope.transform);

        List<int> pointIndeces = new List<int>();
            for (int i = 0; i < bezier.pointsQuantity; i += 25)
            {
                pointIndeces.Add(i);
            }
        if(!Pitfall)
        {
            SpawnThings(newSlope.transform.position,pointIndeces,points);
        }
	}
    
    //spawns enemies, items, and obstacles.
    private void SpawnThings(Vector3 newSlopePosition, List<int> pointIndeces, Vector2[] points  )
    {
        if (enemyCount < 15)
        {
            int spawnEnemy = (int)(spawnChance / Random.Range(0, 1f));
            if (spawnEnemy > 5)
            {
                spawnEnemy = 5;
            }
            enemyCount += spawnEnemy;
            if (spawnEnemy > 0)
            {
                // get a list of all the points on the slope,
                // pick spawnEnemy number randomly
                // spawn random Enemies from Enemies at those points. 

                for (int i = 0; i < spawnEnemy; i++)
                {
                    Vector2 spawn = points[pointIndeces[(int)Random.Range(0, pointIndeces.Count)]] + new Vector2(0, .12f) + (Vector2)newSlopePosition;
                    GameObject go = enemies[(int)Random.Range(0, enemies.Length)];
                    Instantiate(go, spawn, Quaternion.identity);
                }
            }

        }
		if (Random.Range(0, 1f) <= itemChance )
        {
                Vector2 spawn_r = points[pointIndeces[(int)Random.Range(0, pointIndeces.Count)]] + new Vector2(0, .12f) + (Vector2)newSlopePosition;
                Instantiate(rocketBoostItem, spawn_r, Quaternion.identity);
        }
        if (Random.Range(0, 1f) <= obstacleChance )
        {
                Vector2 spawn_b = points[pointIndeces[(int)Random.Range(0, pointIndeces.Count)]] + new Vector2(0, .12f) + (Vector2)newSlopePosition;
                Instantiate(boulder, spawn_b, Quaternion.identity);
        }
    }

    
}
