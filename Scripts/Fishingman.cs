using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;


//深きものども
public class Fishingman : MonoBehaviour
{
    //パラメーター
    
    [SerializeField]   int EHp;


    public GameObject target;
    private NavMeshAgent agent;
    public PlayerTest PlayerTest;
    [SerializeField] float Delay;
    [SerializeField] float AttackSpeed;
    private bool Attack = false;
    bool Stan = false;

    void Start()
    {
        Application.targetFrameRate = 60;
        agent = GetComponent<NavMeshAgent>();
    }

    private IEnumerator EnemyAttack()
    {
        Attack = true;
        yield return new WaitForSeconds(Delay);

        while (Attack == true)
        {
            StartCoroutine(HpInvinciblyManager.Invincible());
            yield return new WaitForSeconds(AttackSpeed);
        }
    }

    private IEnumerator SwordFishAttack()
    {
        agent.isStopped = true;
        Stan = true;
        EHp -= KazikiStats.KazikiPower;
        yield return new WaitForSeconds(KazikiStats.KazikiStanTime);
        agent.isStopped = false;
        Stan = false;

    }

    private IEnumerator MackrelAttack()
    {
        agent.isStopped = true;
        Stan = true;
        EHp -= SabaStats.SabaPower;
        yield return new WaitForSeconds(SabaStats.SabaStanTime);
        agent.isStopped = false;
        Stan = false;
    }


    void OnTriggerEnter(Collider Other)
    {
       if (Other.gameObject.tag == "Player")
        {         
            HpInvinciblyManager.IsDeceleration = true;
            if(Stan==false){
            StartCoroutine(EnemyAttack());
            }
        }
        else if (Other.gameObject.tag == "SwordFish")
        {
            Attack = false;
            StartCoroutine(SwordFishAttack());
        }
        else if (Other.gameObject.tag == "Mackrel")
        {
            Attack = false;
            StartCoroutine(MackrelAttack());
        }
        else
        {
            Attack = false;
        }
    }

    void OnTriggerExit(Collider Other)
    {
        Attack = false;
    }

    // Update is called once per frame
    void Update()
    {
        //追跡AI
        if (target)
        {
            agent.destination = target.transform.position;
        }


        //HP0で死
        if (EHp == 0)
        {
            Destroy(gameObject);
        }
    }
   
}
