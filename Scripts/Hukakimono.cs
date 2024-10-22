using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;


//�[�����̂ǂ�
public class Hukakimono : MonoBehaviour
{
    //�p�����[�^�[
    [SerializeField] public int EHp = 30;

    [SerializeField]
    private EnemySponePoollManager poolManager;


    [SerializeField] GameObject target;
    private NavMeshAgent agent;
    [SerializeField] float Delay;
    [SerializeField] float AttackSpeed;
    [SerializeField] float DeathAnimTime = 2;
    private bool Attack = false;
    private bool Stan = false;
    private ShoggothAnimation Animation;
    [SerializeField] private BoxCollider[] col;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private Renderer renderer;
    private float lightTimer;

    public bool CutuluhuMini;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private ParticleSystem attackEff;

    void Start()
    {
        attackEff.Stop();
        agent = GetComponent<NavMeshAgent>();
        Animation = GetComponent<ShoggothAnimation>();
        Attack = false;
        Stan = false;
        agent.isStopped = false;
        //EHp = 30;
        poolManager = EnemySponePoollManager.enemySponePoolManager;
        col = this.gameObject.GetComponents<BoxCollider>();
    }

    private void OnEnable()
    {
        attackEff.Stop();
    }


    private void AttackTest()
    {
        //        HpInvinciblyManager.IsInvincible = true;
        //Debug.Log("Attack:" + Attack);
        if (Attack)
            StartCoroutine(HpInvinciblyManager.Invincible());
    }
    
    private IEnumerator EnemyAttack()
    {
        if (Attack) yield break;
        Attack = true;
        yield return new WaitForSeconds(Delay);

        while (Attack == true)
        {
            //HpInvinciblyManager.Damage();
            yield return new WaitForSeconds(AttackSpeed);
            Animation.ShoggothAttackAnimation();
            //Debug.Log("atari");


            if (!HpInvinciblyManager.IsInvincible)
            {
                yield return new WaitForSeconds(0.2f);
                AttackBullet();
                //StartCoroutine(HpInvinciblyManager.Invincible());
                HpInvinciblyManager.IsDeceleration = true;

                //HpInvinciblyManager.IsInvincible = true;
                //StartCoroutine(HpInvinciblyManager.Invincible());
            }
        }
    }

    public void PlayEffect()
    {
        StartCoroutine(EffectOneShot());
    }

    private IEnumerator EffectOneShot()
    {
        attackEff.Play();
        yield return new WaitForSeconds(0.5f);
        attackEff.Stop();
    }

    private void AttackBullet()
    {
        Instantiate(bullet,this.transform.position,Quaternion.identity);
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
        EHp = 30;
        ExperiencePointController.ExpPointControllerInstance.SetEnemyDead(this.gameObject.transform.position);
        var eff = Instantiate(deathEffect, this.transform.position, Quaternion.identity);
        if(CutuluhuMini) CthulhuManager.cthulhuManager.StanStack(200f);
        poolManager.OnReleaseToPool1(this.gameObject);
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
        EHp = 30;
        ExperiencePointController.ExpPointControllerInstance.SetEnemyDead(this.gameObject.transform.position);
        var eff = Instantiate(deathEffect, this.transform.position, Quaternion.identity);
        if(CutuluhuMini) CthulhuManager.cthulhuManager.StanStack(200f);
        Destroy(this.gameObject);
    }




    void OnTriggerEnter(Collider Other)
    {
        if (Other.gameObject.tag == "Player")
        {
            HpInvinciblyManager.IsDeceleration = true;
            if (Stan == false)
            {
                StartCoroutine(EnemyAttack());
            }

        }
        else if (Other.gameObject.tag == "SwordFish")
        {
            Attack = false;
            StartCoroutine(RimLightSet());
            StartCoroutine(SwordFishAttack());
        }
        else if (Other.gameObject.tag == "Mackrel")
        {
            //Debug.Log("Hit");
            Attack = false;
            var saba = Other.gameObject.GetComponent<SabaResidue>();
            saba.boxCollider.enabled = false;
            StartCoroutine(saba.fixedSaba("Enemy"));
            StartCoroutine(RimLightSet());
            //Other.transform.parent.gameObject.SetActive(false);
            StartCoroutine(MackrelAttack());
        }
    }

    void OnTriggerExit(Collider Other)
    {
        Attack = false;
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

        //�ǐ�AI
        /*
          if (target)
          {
              agent.destination = target.transform.position;
          }*/

        agent.SetDestination(PlayerController.PlayerGameObject.transform.position);
        //HP0�Ŏ�
        if (EHp <= 0)
        {
            agent.isStopped = true;
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

    private IEnumerator DeathEffect()
    {
        var eff = Instantiate(deathEffect, this.transform.position, Quaternion.identity, this.transform);
        var particle = eff.GetComponent<ParticleSystem>();
        yield return new WaitUntil(() => particle.isStopped);
    }

    private IEnumerator RimLightSet()
    {
        renderer.material.SetFloat("_RimLight", 1f);
        lightTimer = 0.1f;
        yield break;
    }

}


