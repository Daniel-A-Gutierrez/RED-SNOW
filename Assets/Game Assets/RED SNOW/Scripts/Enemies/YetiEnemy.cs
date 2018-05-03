using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Yeti will try to throw a projectile at the player the moment it sees the player, then acts like a NinjaGuy
public class YetiEnemy : NinjaGuy
{

    bool hasSeenPlayer = false;
    bool hasThrown = false;

    Transform target;
    Transform throwPoint;
    public GameObject rockPrefab;

    // Update is called once per frame
    void Update()
    {
        CheckVisible();
        NinjaBehavior();
    }

    public override void NinjaBehavior()
    {
        //Check if the Yeti has thrown a rock already and should move at the player like NinjaGuy
        base.NinjaBehavior();

        if (hasSeenPlayer && !hasThrown)
            StartCoroutine("ThrowRock");
    }

    void CheckVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (GeometryUtility.TestPlanesAABB(planes, GetComponent<CircleCollider2D>().bounds) && !hasSeenPlayer)
        {
            hasSeenPlayer = true;
        }
    }

    IEnumerator ThrowRock()
    {

        GameObject rock = Instantiate(rockPrefab,this.transform.position, Quaternion.Euler(0,0,0), this.transform);
        rock.GetComponent<FixedJoint2D>().connectedBody = rock.transform.parent.GetComponent<Rigidbody2D>();
        hasThrown = true;

 
        yield return new WaitForSeconds(0.75f);


        Rigidbody2D rb = rock.GetComponent<Rigidbody2D>();
        target = player.transform;
        throwPoint = rock.transform;
        float x = target.position.x - throwPoint.position.x;
        float y = target.position.y - throwPoint.position.y;

        rb.velocity = new Vector2(x, y);


        yield return null;

    }
}
