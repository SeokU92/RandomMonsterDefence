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
    [SerializeField] private GameObject unitMarker;                     //���õ� ���� ǥ��
    [SerializeField] private bool isAttackDelay = false;                //���� ��Ÿ��
    [SerializeField] private Transform attackPoint;

    [Header("View")]
    [SerializeField] private Transform targeting;                       //��
    [SerializeField,Range(0f, 50f)] private float viewRadius;           //�þ߰Ÿ�
    [SerializeField,Range(0f, 360f)] private float viewAngle;           //�þ߰�
    [SerializeField] private LayerMask targetLayer;                     //�� ���̾�
    List<GameObject> targetList = new List<GameObject>();
    
    [Header("UnitInfo")]
    [HideInInspector]public UnitType unitType;
    [SerializeField] private string unitName;                           //�̸�
    [SerializeField] private float speed;                               //�̵��ӵ�
    [SerializeField] private float attackSpeed;                         //���� �ӵ�
    [SerializeField] private float attackRange;                         //���� ��Ÿ�
    [SerializeField] private string attackName;                         //���� �̸�
    public int damage;                                                  //������
    
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
