using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshAgent))]
public class UnitController : MonoBehaviour
{
    [SerializeField]
    private GameObject unitMarker;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();        
    }
    public void SelectUnit()
    {
        unitMarker.SetActive(true);
       // unitMarker.gameObject.transform.localScale = this.gameObject.transform.localScale * 1.5f;
    }
    public void DeSelectUnit()
    {
        unitMarker.SetActive(false);
    }
    public void Move(Vector3 goal)
    {
        agent.SetDestination(goal);
    }
}
