using System.Collections;
using System.Collections.Generic;
//using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyBulletSpawn : MonoBehaviour
{
    [SerializeField]
    bool useObjectPool = true;
    [SerializeField]
    EnemyBulletPoolManager poolManager;
    [SerializeField]
    GameObject bulletprefab;
    [SerializeField]
    int spawnCount = 1;
    [SerializeField]
    float spawnInterval = 0.1f;
    [SerializeField]
    float destroyWaitTime = 3;
    [SerializeField]
    GameObject Enemy;

    Animator animator;

    static bool EnemyAttackWait=false;

    private GameObject target;

    WaitForSeconds spawnIntervalWait;

    private NavMeshAgent agent;

    public Offspring offspring;//Offspring���Q��

    void Start()
    {
        spawnIntervalWait = new WaitForSeconds(spawnInterval);

        StartCoroutine(nameof(SpawnTimer));

        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

    }

    IEnumerator SpawnTimer()
    {
        int i;

            while (true)
            {
            if (offspring.OffspringMove == false&&offspring.deathstop == false)//���Ƃ��q�������Ă��Ȃ��i�U���͈͓��j�Ȃ�΍U��  //offspring.deathstop == false追加
            {
                for (i = 0; i < spawnCount; i++)
                {
                    animator.SetTrigger("toAttack");

                    yield return new WaitForSeconds(1.1f);

                    Spawn(bulletprefab);
                }
            }

                yield return spawnIntervalWait;
            }
    }

    void Spawn(GameObject prefab)
    {
        Enemy_Bullet destroyer;
        target = PlayerController.PlayerGameObject;
        Vector3 pos = new Vector3(Enemy.transform.position.x, Enemy.transform.position.y, Enemy.transform.position.z);

        if (useObjectPool)
        {
            destroyer = poolManager.GetGameObject(prefab, pos, Quaternion.identity).GetComponent<Enemy_Bullet>();
            destroyer.PoolManager = poolManager;
            destroyer.TargetForcus(target);

            //Debug.Log(target.transform.rotation);
        }
        else
        {
            destroyer = Instantiate(prefab, pos, this.transform.rotation).GetComponent<Enemy_Bullet>();
            destroyer.TargetForcus(target);
        }

        if (destroyer != null)
        {
            destroyer.StartDestroyTimer(destroyWaitTime);
        }
    }
}
