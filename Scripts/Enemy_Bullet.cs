using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.EditorTools;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    [SerializeField]
    private float BulletSpeed = 50.0f;

    private GameObject target;



    private void Start()
    {
        target =GameObject.Find("Target");
        this.transform.LookAt(PlayerController.PlayerGameObject.transform);
        Transform myTransform =this.transform;
        Vector3 rot=myTransform.localEulerAngles;
        rot.x=0;
        myTransform.eulerAngles=rot;
        Vector3 pos=myTransform.localPosition;
        pos.y=1.0f;
        myTransform.position=pos;
        

    }
    void OnEnable()
    {
        //this.transform.LookAt(target.transform);
    }
    void Update()
    {
        transform.position+=transform.forward*BulletSpeed*Time.deltaTime;
    }
    public EnemyBulletPoolManager PoolManager { get; set; }

    public void StartDestroyTimer(float time)
    {
        StartCoroutine(DestroyTimer(time));
    }

    IEnumerator DestroyTimer(float time)
    {
        yield return new WaitForSeconds(time);

        if (PoolManager != null)
        {
            PoolManager.ReleaseGameObject(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TargetForcus(GameObject obj)
    {
        this.transform.LookAt(obj.transform);
    }

    void OnTriggerEnter(Collider Other)
    {
        if (Other.gameObject.tag == "Player")
        {
            StartCoroutine(HpInvinciblyManager.Invincible());
            if (PoolManager != null)
            {
                PoolManager.ReleaseGameObject(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            //Debug.Log("atari");
        }
    }
}