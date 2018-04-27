using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ninja guy's behavior will be simple : Get as close to the player as possible, and then stab them. 
[RequireComponent(typeof (EnemyControl))]
public class NinjaGuy : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;
	EnemyControl enemyControl;
	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		enemyControl = GetComponent<EnemyControl>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        NinjaBehavior();
		
	}

    public virtual void NinjaBehavior()
    {
        if ((player.transform.position - transform.position).x > 2)
        {
            enemyControl.SetInputAxis(1);
        }
        if ((player.transform.position - transform.position).x < -2)
        {
            enemyControl.SetInputAxis(-1);
        }
        if (Mathf.Abs((player.transform.position - transform.position).x) < 2)
        {
            enemyControl.SetInputAxis(0);
        }
    }
}
