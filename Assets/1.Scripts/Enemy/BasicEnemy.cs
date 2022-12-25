using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class BasicEnemy : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("NavMesh")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform goalPoint;

    [Serializable]
    public class EnemyInfo
    {
        BasicEnemy basicEnemy;
        public string enemyName;
        public float speed;
        public float maxHp;
        [SerializeField] private float dropGold;
        [SerializeField] private float curHp;
        public float Hp
        {
            get { return curHp; }
            set
            {
                curHp = value;
                if(curHp > maxHp)
                    curHp = maxHp;
                if(curHp <= 0)
                {
                    basicEnemy.Die();
                }
            }
        }
    }   
    [SerializeField] private EnemyInfo enemyInfo;
    private void OnEnable()
    {
        agent.speed = enemyInfo.speed;
        gameObject.transform.position = startPoint.position;
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }    
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        if (agent != null)
        {
            agent.destination = goalPoint.position;
            if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.5f)
            {
                gameObject.SetActive(false);
            }
        }
    }  
   
    private void Die()
    {
        if(enemyInfo.Hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        ObjectPooling.ReturnToPool(gameObject);
        enemyInfo.Hp = enemyInfo.maxHp;
    }
}
