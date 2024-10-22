using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CthulhuHand : MonoBehaviour
{
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Death()
    {
        animator.SetTrigger("Death");
    }

    void Shot()
    {
        animator.SetTrigger("Shot");
    }
}
