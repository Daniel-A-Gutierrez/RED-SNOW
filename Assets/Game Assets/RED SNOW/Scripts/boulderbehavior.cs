using UnityEngine;

public class boulderbehavior : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
