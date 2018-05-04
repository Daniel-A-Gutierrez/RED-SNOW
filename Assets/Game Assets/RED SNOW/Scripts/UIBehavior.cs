using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIBehavior : MonoBehaviour {

    Animator c_animator;
    public GameObject DiedPanel;
    public Text DeadText;

    private void Start()
    {
        c_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (c_animator.GetBool("Dead") && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void SetDead()
    {
        c_animator.SetBool("Dead", true);
    }


}
