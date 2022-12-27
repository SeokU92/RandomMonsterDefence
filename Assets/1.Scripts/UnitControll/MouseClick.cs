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
    /// 마우스 좌클릭 유닛 선택
    /// </summary>
    private void UnitControll()
    {
        //마우스 좌클릭 유닛 선택,해제
        if (Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            //레이에 unitLayer에 해당하는 오브젝트가 있을 때
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
            {
                if (hit.transform.GetComponent<BasicUnit>() == null) return;
                if(Input.GetKey(KeyCode.LeftControl))                
                    unitManager.ControlClickSelectUnit(hit.transform.GetComponent<BasicUnit>());
                else
                    unitManager.ClickSelectUnit(hit.transform.GetComponent<BasicUnit>());
            }
            //레이에 해당하는 오브젝트가 없을 때
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
                //클릭한곳 표시하는 파티클 추가
            }
        }
    }
}
