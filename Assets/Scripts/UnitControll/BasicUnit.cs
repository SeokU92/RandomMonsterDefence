using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class BasicUnit : MonoBehaviour
{
    [SerializeField]
    private GameObject unitMarker;
    private NavMeshAgent agent;

    [Serializable]
    public class UnitInfo
    {
        public UnitTypeOne unitType;
        public string name;
        public float damage;
        public float speed;
        public float attackRange;
    }
    [SerializeField] private UnitInfo unitInfo;
    private void OnEnable()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();     
        agent.speed = unitInfo.speed;
    }
    public void SelectUnit()
    {
        unitMarker.SetActive(true);
        unitMarker.gameObject.transform.localScale = this.gameObject.transform.localScale * 1.5f;
    }
    public void DeSelectUnit()
    {
        unitMarker.SetActive(false);
    }   
    public void Move(Vector3 goal)
    {
        if (agent != null)
            agent.SetDestination(goal);
        else return;
    }
    public void SellUnit()
    {
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        ObjectPooling.ReturnToPool(gameObject);        
    }
}
