using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerTest : MonoBehaviour
{
    private NavMeshAgent agent;
    private int PlayerHp = 10;
    // Start is called before the first frame update
    void Start()
    {
       // Application.targetFrameRate = 60;
    }

    /*// Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0, 0.25f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, -0.25f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-0.25f, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(0.25f, 0, 0);
        }
    }*/
    
}
