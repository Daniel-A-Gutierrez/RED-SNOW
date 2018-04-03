using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeManager : MonoBehaviour {

	GameObject player;
	GameObject camera;
	public GameObject toInstantiate;
	public int score;
	List<Transform> slopes;
	public float slopeLengthFactor;
	public float slopeDeclineFactor;
	public float slopeTurbulenceFactor;
	public float renderDistance;
	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	    camera = GameObject.FindGameObjectWithTag("MainCamera");
		slopes = new List<Transform>();
		GameObject[] tarray =  GameObject.FindGameObjectsWithTag("Slope");
		foreach (GameObject g in tarray)
		{
			slopes.Add(g.transform);
		}

	}
	//what vars do i need right now for this? 
	//start end, a starting derivative
	//a mean slope
	// a random secondary point
	void CreateSlope(Vector2 p0, float density)
	{
		GameObject newSlope = Instantiate(toInstantiate,p0,Quaternion.identity);
		
		BezierCollider2D bezier = newSlope.GetComponent<BezierCollider2D>();
		BezierCollider2D previousBezier = slopes[slopes.Count-1].GetComponent<BezierCollider2D>();
		Vector2 d00 = previousBezier.getLastDerivative();
		Vector2 d3 = previousBezier.secondPoint;
		/*float decline;
		float length;
		float turbulence;
		if(score>=10000)
		{
		    decline = (slopeDeclineFactor*(Random.Range(1,3)))*3;
		}
		else
		{
			 decline = slopeDeclineFactor*(1/3330*score*Random.Range(.5f,score/4000+.5f)+.5f);
		}
		if(score>=10000)
		{
		 	length = slopeLengthFactor/4*Random.Range(.5f,4);
		}
		else
		{
			 length = slopeLengthFactor/(1/3333*score + 1)/Random.Range(.5f,3);
		}
		if(score>=10000)
		{
			turbulence = slopeTurbulenceFactor*Random.Range(.25f,2f);
		}
		else
		{
			turbulence = slopeTurbulenceFactor*(1/10000*score)+.2f;
		}
		float deviation1 = Random.Range(.1f,.7f);
		float deviation2 = Random.Range(deviation1,.9f);
		float regressive1 = Random.Range(length*decline*turbulence,length*decline*turbulence*-1);
		float regressive2 = Random.Range(length*decline*turbulence,length*decline*turbulence*-1);
		bezier.firstPoint = new Vector2(p0.x  + .001f, p0.y);
		bezier.secondPoint = new Vector2(d3.x+length,d3.y+decline *length);
		bezier.handlerFirstPoint = d3+deviation1*length*d00;
		bezier.handlerSecondPoint = new Vector2(d3.x+d00.x*length*deviation2,d3.y+d00.y*length*regressive2*deviation2);
		bezier.pointsQuantity = (int)(density*(length));
		*/
		bezier.firstPoint = new Vector2(0,0);
		bezier.secondPoint = new Vector2(10,bezier.firstPoint.y) ;
		bezier.handlerFirstPoint = bezier.firstPoint + new Vector2(1,0) ;
		bezier.handlerSecondPoint = bezier.firstPoint+ new Vector2(9,0) ;
		newSlope.GetComponent<EdgeCollider2D>().points = newSlope.GetComponent<BezierCollider2D>().calculate2DPoints();
		slopes.Add(newSlope.transform);
		print("position" + newSlope.transform.position + "p1 : " + bezier.firstPoint + "p2:" + bezier.handlerFirstPoint+ "p3 " + bezier.handlerSecondPoint + "p4: " +bezier.secondPoint);
	}

	// basically if the character gets within render distance
	//of the next chunk, generate a chunk that starts on the
	// end point of the last one and has procedurally generated
	//attributes. 
	//so basically if the slope between handler second and second
	//matches the slope from handler first to first of the next line it looks fine. 
	void FixedUpdate ()
	{
		score ++;
		if(slopes[slopes.Count-1].position.x-player.transform.position.x < renderDistance)
		{

			CreateSlope(slopes[slopes.Count-1].GetComponent<BezierCollider2D>().secondPoint + 
			new Vector2 ( slopes[slopes.Count-1].transform.position.x ,slopes[slopes.Count-1].transform.position.y) , 25);
		}
		if(player.transform.position.x - slopes[0].position.x > renderDistance)
		{
			GameObject obj = slopes[0].gameObject;
			slopes.RemoveAt(0);
			Destroy(obj);
		}
	}
}
