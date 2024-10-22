using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;


//�[�����̂ǂ�
public class Offspring : MonoBehaviour
{
    //�p�����[�^�[
    [SerializeField] public int EHp = 50;
    [SerializeField] float Range=0.1f;
    private ShoggothAnimation Animation;

    [SerializeField]
    private EnemySponePoollManager poolManager;

    private bool stan;
    public bool deathstop=false;

    public GameObject target;
    private NavMeshAgent agent;
    public bool OffspringMove = true;
    [SerializeField] private BoxCollider[] col;

    [SerializeField]
    private GameObject deathEffect;
    [SerializeField] float DeathAnimTime = 2;

    [SerializeField]
    private Renderer renderer;
    private float lightTimer;

    public bool CutuluhuMini;

    void Start()
    {
        Animation = GetComponent<ShoggothAnimation>();

        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = true;
        poolManager = EnemySponePoollManager.enemySponePoolManager;

    }

    private IEnumerator PoolAnim()
    {
        GetComponent<BoxCollider>().enabled = false;
        agent.isStopped = true;
        Animation.ShoggothDeathAnimation();
        
        foreach (BoxCollider collider in col)
        {
            collider.enabled = false;
        }
        yield return new WaitForSeconds(DeathAnimTime);
        GameManager.GameManagerClass.soundManager.Play("EnemyDie"); 
        deathstop = false;
        ExperiencePointController.ExpPointControllerInstance.SetEnemyDead(this.gameObject.transform.position);

        var eff = Instantiate(deathEffect, this.transform.position, Quaternion.identity);
        if(CutuluhuMini) CthulhuManager.cthulhuManager.StanStack(200f);
        poolManager.OnReleaseToPool3(this.gameObject);
    }

    private IEnumerator DeathAnim()
    {
        GetComponent<BoxCollider>().enabled = false;
        agent.isStopped = true;
        
        foreach (BoxCollider collider in col)
        {
            collider.enabled = false;
        }
        Animation.ShoggothDeathAnimation();
        yield return new WaitForSeconds(DeathAnimTime);
        GameManager.GameManagerClass.soundManager.Play("EnemyDie");
        deathstop = false;
        ExperiencePointController.ExpPointControllerInstance.SetEnemyDead(this.gameObject.transform.position);

        var eff = Instantiate(deathEffect, this.transform.position, Quaternion.identity);
        if(CutuluhuMini) CthulhuManager.cthulhuManager.StanStack(200f);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(lightTimer != 0f)
        {
            lightTimer += Time.deltaTime;
            if(lightTimer > 0.3f)
            {
                renderer.material.SetFloat("_RimLight", 0f);
                lightTimer = 0f;
            }

        }

        if(stan) return;
/*
        //�ǐ�AI
        if (target)
        {
            agent.SetDestination(PlayerController.PlayerGameObject.transform.position);
        }
*/
        agent.SetDestination(PlayerController.PlayerGameObject.transform.position);

        //�U���͈͓��̎��ɐÎ~
        float dis = Vector3.Distance(this.transform.position, PlayerController.PlayerGameObject.transform.position);
        if (dis <Range)
        {
            OffspringMove = false;
            agent.isStopped = true;
            Vector3 vector3 = PlayerController.PlayerGameObject.transform.position - this.transform.position;
            

            this.transform.LookAt(PlayerController.PlayerGameObject.transform);
        }
        else if(deathstop==false)
        {
            OffspringMove = true;
            agent.isStopped = false;
        }

        //HP0�Ŏ�
        if (EHp <= 0)
        {
            deathstop = true;
            agent.isStopped = true;
            EHp = 50;
            if (poolManager != null)
            {
                
                StartCoroutine(PoolAnim());
            }
            else
            {

                StartCoroutine(DeathAnim());
            }
        }
    }
    private IEnumerator MackrelAttack()
    {
        stan = true;
        agent.isStopped = true;
        EHp -= SabaStats.SabaPower;
        yield return new WaitForSeconds(SabaStats.SabaStanTime);
        agent.isStopped = false;
        stan = false;
    }


    void OnTriggerEnter(Collider Other)
    {
        if (Other.gameObject.tag == "Player")
        {         
            HpInvinciblyManager.IsDeceleration = true;
        }
        if (Other.gameObject.tag == "Mackrel")
        {
            var saba = Other.gameObject.GetComponent<SabaResidue>();
            saba.boxCollider.enabled = false;
            StartCoroutine(saba.fixedSaba("Enemy"));
            StartCoroutine(RimLightSet());
            StartCoroutine(MackrelAttack());

            //Other.transform.parent.gameObject.SetActive(false);
        }
        else if(Other.gameObject.tag == "SwordFish")
        {
            EHp -= KazikiStats.KazikiPower;
            StartCoroutine(RimLightSet());
        }
    }

    private IEnumerator RimLightSet()
    {
        renderer.material.SetFloat("_RimLight", 1f);
        lightTimer = 0.1f;
        yield break;
    }

}
