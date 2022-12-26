using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class BasicUnit : MonoBehaviour, IAttackble
{
    [SerializeField] private GameObject unitMarker;                     //���õ� ���� ǥ��
    private NavMeshAgent agent;
    private Animator anim;
    [SerializeField] private bool isAttackDelay = false;                //���� ��Ÿ��
    [SerializeField] private Transform attackPoint;

    [Header("View")]
    [SerializeField] public Transform targeting;                        //��
    [SerializeField,Range(0f, 50f)] private float viewRadius;           //�þ߰Ÿ�
    [SerializeField,Range(0f, 360f)] private float viewAngle;           //�þ߰�
    [SerializeField] private LayerMask targetLayer;                     //�� ���̾�
    List<GameObject> targetList = new List<GameObject>();
    
    [Header("UnitInfo")]
    public UnitTypeOne unitType;
    public Color unitColor;
    [SerializeField] private string unitName;         //�̸�
    [SerializeField] private float speed;             //�̵��ӵ�
    [SerializeField] private float attackSpeed;       //���� �ӵ�
    [SerializeField] private float attackRange;       //���� ��Ÿ�
    [SerializeField] private string attackName;       //���� �̸�
    public int damage;                                //������
    
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
    /// ���õ� ���� ǥ�� �Լ�
    /// </summary>
    public void SelectUnit()
    {
        unitMarker.SetActive(true);
        unitMarker.gameObject.transform.localScale = this.gameObject.transform.localScale * 1.5f;
    }
    /// <summary>
    /// ���� ���� ǥ�� �Լ�
    /// </summary>
    public void DeSelectUnit()
    {
        unitMarker.SetActive(false);
    }   
    /// <summary>
    /// ���� �̵� �Լ�
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
    /// ���� �Ǹű��
    /// </summary>
    public void SellUnit()
    {
        gameObject.SetActive(false);
    } 
    /// <summary>
    /// �� Ž�� �Լ�
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void FindTarget()
    {
        //targetList.Clear();
        // ���� ���� ���� �ִ°�
        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, targetLayer);
        Transform shortestTarget = null; //���� ����� Ÿ��
        if(targets.Length > 0)
        {
            float shortDistance = Mathf.Infinity; // ��ü �Ÿ� ��
            foreach(Collider target in targets)   // ����� �ݶ��̴���ŭ �ݺ���
            {
                float targetDistance = Vector3.SqrMagnitude(transform.position - target.transform.position);
                                              //SqrMagnitude = ������ȯ (�����Ÿ� x �����Ÿ�)
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
    /// <summary>
    //for(int i = 0; i < targets.Length; i++)
    //{
    //    Transform target = targets[i].transform;
    //    Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
    //    //���� ���� �ִ°�
    //    if (Vector3.Dot(transform.forward, dirToTarget) < Mathf.Cos(viewAngle * 0.5f * Mathf.Deg2Rad))
    //    {
    //        float distToTarget = Vector3.Distance(transform.position, target.position);
    //        //��ֹ��� ���°�
    //        if(!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleLayer))
    //        {
    //            targetList.Add(target.transform.gameObject);
    //            Debug.DrawRay(transform.position, dirToTarget * distToTarget, Color.red);
    //        }
    //    }
    //}
    //if(targetList.Count != 0)
    //{
    //    targeting = targetList[0];
    //    shortDis = Vector3.Distance(transform.position, targetList[0].transform.position);
    //    foreach(GameObject found in targetList)
    //    {
    //        float distance = Vector3.Distance(transform.position, found.transform.position);
    //        if(distance < shortDis)
    //        {
    //            targeting = found;
    //            shortDis = distance;
    //        }
    //    }
    //    Debug.Log(targeting.name);
    //}
    /// </summary>
    public IEnumerator Attack()
    {       
        if(!isAttackDelay)
        {
            isAttackDelay = true;
            transform.LookAt(targeting.transform);
            anim.SetTrigger("Attack");           
            if(attackName == "WormDeath")
            {
                ObjectPooling.SpawnFromPool(attackName, targeting.position, targeting.rotation);
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
