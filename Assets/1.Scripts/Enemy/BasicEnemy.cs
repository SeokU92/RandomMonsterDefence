using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class BasicEnemy : MonoBehaviour, IHittable
{
    private NavMeshAgent agent;

    [Header("NavMesh")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform goalPoint;

    [Header("EnemyInfo")]   
    [SerializeField] private string enemyName;
    [SerializeField] private float speed;
    [SerializeField] private int dropGold;
    [SerializeField] private float maxHp;
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
                Die();
            }
        }
    }       
    private void OnEnable()
    {
        GameManager.Instance.enemy = FindObjectOfType<BasicEnemy>();
        agent.speed = speed;
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
                GameManager.Instance.Life--;
                gameObject.SetActive(false);
            }
        }
    }
    public void Hit(int damage)
    {
        Hp -= damage;
        //데미지 텍스트
    }
    private void Die()
    {
        if(Hp <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.Gold += dropGold;
        }
    }
    private void OnDisable()
    {
        ObjectPooling.ReturnToPool(gameObject);
        Hp = maxHp;
    }

    
}
