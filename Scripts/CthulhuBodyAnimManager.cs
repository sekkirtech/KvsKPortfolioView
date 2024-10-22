using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CthulhuBodyAnimManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private IEnumerator StunDersy(int Deray)
    {
        yield return new WaitForSeconds(Deray);

        animator.SetTrigger("WakeUp");
    }

    void DeathAnim()
    {
        animator.SetTrigger("Death");
    }

    void StunAnim(int Deray)
    {
        animator.SetTrigger("Stun");

        StartCoroutine(StunDersy(Deray));
    }

    void Shout() 
    {
        animator.SetTrigger("Shout");
    }
}
