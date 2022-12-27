using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    [SerializeField] private LayerMask unitLayer;
    [SerializeField] private LayerMask groundLayer;
    private Camera mainCam;
    private UnitManager unitManager;

    private void Awake()
    {    
        mainCam = Camera.main;
        unitManager = GetComponent<UnitManager>();
    }
    private void Update()
    {
        UnitControll();        
    }
    /// <summary>
    /// ���콺 ��Ŭ�� ���� ����
    /// </summary>
    private void UnitControll()
    {
        //���콺 ��Ŭ�� ���� ����,����
        if (Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            //���̿� unitLayer�� �ش��ϴ� ������Ʈ�� ���� ��
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
            {
                if (hit.transform.GetComponent<BasicUnit>() == null) return;
                if(Input.GetKey(KeyCode.LeftControl))                
                    unitManager.ControlClickSelectUnit(hit.transform.GetComponent<BasicUnit>());
                else
                    unitManager.ClickSelectUnit(hit.transform.GetComponent<BasicUnit>());
            }
            //���̿� �ش��ϴ� ������Ʈ�� ���� ��
            else
            {

                if (!Input.GetKey(KeyCode.LeftControl))
                    unitManager.DeSelectAll();
            }
        }        
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                unitManager.SelectUnitsMove(hit.point);
                //Ŭ���Ѱ� ǥ���ϴ� ��ƼŬ �߰�
            }
        }
    }
}
