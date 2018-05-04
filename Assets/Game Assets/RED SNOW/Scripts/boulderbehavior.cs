using UnityEngine;

public class boulderbehavior : MonoBehaviour {

    public GameObject boulder;

    // Use this for initialization
    void Start()
    {
            boulder = Resources.Load("boulder") as GameObject;
       

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        try
        {


            if (other.gameObject.name == "Player")
            {
                // Die();
            }
        }
        catch 
        {
            print()
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
