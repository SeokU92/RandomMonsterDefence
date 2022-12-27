using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public enum UnitType
{
    Reaper, Plant, Scorpion, Worm
}
public class BasicUnit : MonoBehaviour, IAttackble
{
    private NavMeshAgent agent;
    private Animator anim;
    [SerializeField] private GameObject unitMarker;                     //선택된 유닛 표시
    [SerializeField] private bool isAttackDelay = false;                //공격 쿨타임
    [SerializeField] private Transform attackPoint;

    [Header("View")]
    [SerializeField] private Transform targeting;                       //적
    [SerializeField,Range(0f, 50f)] private float viewRadius;           //시야거리
    [SerializeField,Range(0f, 360f)] private float viewAngle;           //시야각
    [SerializeField] private LayerMask targetLayer;                     //적 레이어
    List<GameObject> targetList = new List<GameObject>();
    
    [Header("UnitInfo")]
    [HideInInspector]public UnitType unitType;
    [SerializeField] private string unitName;                           //이름
    [SerializeField] private float speed;                               //이동속도
    [SerializeField] private float attackSpeed;                         //공격 속도
    [SerializeField] private float attackRange;                         //공격 사거리
    [SerializeField] private string attackName;                         //공격 이름
    public int damage;                                                  //데미지
    
    private void OnEnable()
    {
        GameManager.Instance.unit = FindObjectOfType<BasicUnit>();
        anim = GetComponent<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();     
        agent.speed = speed;
        attackRange = viewRadius;
    }   
    private void Update()
    {
        FindTarget();
    }
    /// <summary>
    /// 선택된 유닛 표시 함수
    /// </summary>
    public void SelectUnit()
    {
        unitMarker.SetActive(true);
        unitMarker.gameObject.transform.localScale = this.gameObject.transform.localScale * 1.5f;
    }
    /// <summary>
    /// 선택 해제 표시 함수
    /// </summary>
    public void DeSelectUnit()
    {
        unitMarker.SetActive(false);
    }   
    /// <summary>
    /// 유닛 이동 함수
    /// </summary>
    /// <param name="goal"></param>
    public void Move(Vector3 goal)
    {
        if (agent != null)
        {
            anim.SetBool("IsMove", true);
            agent.SetDestination(goal);
            if (agent.stoppingDistance < 1f)
            {
                anim.SetBool("IsMove", false);
            }
        }
        else return;
    }
    /// <summary>
    /// 유닛 판매기능
    /// </summary>
    public void SellUnit()
    {
        gameObject.SetActive(false);
    } 
    /// <summary>
    /// 적 탐색 함수
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void FindTarget()
    {
        //targetList.Clear();
        // 범위 내에 몬스터 있는가
        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, targetLayer);
        Transform shortestTarget = null; //가장 가까운 타겟
        if(targets.Length > 0)
        {
            float shortDistance = Mathf.Infinity; // 객체 거리 비교
            foreach(Collider target in targets)   // 검출된 콜라이더만큼 반복문
            {
                float targetDistance = Vector3.SqrMagnitude(transform.position - target.transform.position);
                                              //SqrMagnitude = 제곱반환 (실제거리 x 실제거리)
                if(shortDistance > targetDistance)
                {
                    shortDistance = targetDistance;
                    shortestTarget = target.transform;
                }
            }
        }
        targeting = shortestTarget;

        if (targeting != null)
            StartCoroutine(Attack());
        else StopCoroutine(Attack());
        
    } 
    public IEnumerator Attack()
    {       
        if(!isAttackDelay)
        {
            isAttackDelay = true;
            transform.LookAt(targeting.transform);
            anim.SetTrigger("Attack");           
            if(attackName == "WormDeath")
            {
                ObjectPooling.SpawnFromPool(attackName, targeting.position, Quaternion.Euler(-180, 0, 0));
            }
            else
            {
                ObjectPooling.SpawnFromPool(attackName, attackPoint.position, attackPoint.rotation);
            }
            yield return new WaitForSeconds(attackSpeed);
            isAttackDelay = false;
        }          
    }   

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 lookDir = AngleToDir(transform.eulerAngles.y);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - viewAngle * 0.5f);
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + viewAngle * 0.5f);

        Debug.DrawRay(transform.position, lookDir * viewRadius, Color.green);
        Debug.DrawRay(transform.position, leftDir * viewRadius, Color.white);
        Debug.DrawRay(transform.position, rightDir * viewRadius, Color.white);
    }
    public Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }
    private void OnDisable()
    {
        ObjectPooling.ReturnToPool(gameObject);
    }   
}
