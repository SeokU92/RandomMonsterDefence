using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private Transform unitSpawnTrans;                //유닛 소환 위치

    private List<BasicUnit> selectUnitList = new List<BasicUnit>();   //선택된 유닛
    public List<BasicUnit> unitList { get; private set; }             //맵에 존재하는 모든 유닛    

    private void Awake()
    {
        unitList = new List<BasicUnit>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (GameManager.Instance.Gold > GameManager.Instance.PurchaseCost)
                SpawnUnits();
            else return;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SellUnits();
        }
    }

    /// <summary>
    ///  유닛 1단계 랜덤소환
    /// </summary>
    /// <returns></returns>
    public List<BasicUnit> SpawnUnits()
    {        
        List<BasicUnit> unitLists = new List<BasicUnit>();

        //유닛타입 enum을 랜덤으로 받아와서 string 으로 풀링
        UnitType UT = (UnitType)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(UnitType)).Length));
        GameObject units = ObjectPooling.SpawnFromPool(UT.ToString(), unitSpawnTrans.position);
        BasicUnit unit = units.GetComponent<BasicUnit>();
        unitLists.Add(unit); 
        unitList.Add(unit);  //맵 전체 유닛 리스트에 추가

        GameManager.Instance.Gold -= GameManager.Instance.PurchaseCost;
        GameManager.Instance.PurchaseCost++;

        return unitLists;
    }  
    public List<BasicUnit> SellUnits()
    {
        for(int i = 0; i < selectUnitList.Count; i++)
        {
            BasicUnit unit = selectUnitList[i];
            unit.SellUnit();
            unitList.Remove(unit);
            GameManager.Instance.Gold += GameManager.Instance.PurchaseCost / 2;
        }
        return unitList;
    }
    /// <summary>
    /// 마우스 클릭으로 유닛 선택
    /// </summary>
    /// <param name="selectUnit"></param>
    public void ClickSelectUnit(BasicUnit selectUnit)
    {
        DeSelectAll(); //기존에 선택 된 유닛 해제
        SelectUnit(selectUnit);
    }
    /// <summary>
    /// Controll + 마우스 클릭 유닛 선택
    /// </summary>
    /// <param name="selectUnit"></param>
    public void ControlClickSelectUnit(BasicUnit selectUnit)
    {
        if(selectUnitList.Contains(selectUnit)) DeSelectUnit(selectUnit); //기존에 선택된 유닛 중복선택
        else SelectUnit(selectUnit);                                      //새로운 유닛 선택
    }
    /// <summary>
    /// 마우스 드래그 유닛 선택
    /// </summary>
    /// <param name="selectUnit"></param>
    public void DragSelectUnit(BasicUnit selectUnit)
    {
        if (!selectUnitList.Contains(selectUnit))
        {
            SelectUnit(selectUnit);
        }
    }
    /// <summary>
    /// 선택된 모든유닛 이동
    /// </summary>
    /// <param name="goal"></param>
    public void SelectUnitsMove(Vector3 goal)
    {
        for(int i = 0; i < selectUnitList.Count; ++i)
        {
            selectUnitList[i].Move(goal);
        }
    }
    /// <summary>
    /// 모든 유닛 선택 해제
    /// </summary>
    public void DeSelectAll()
    {
        for (int i = 0; i < selectUnitList.Count; ++i)
        {
            selectUnitList[i].DeSelectUnit();
        }
        selectUnitList.Clear();
    }
    /// <summary>
    /// 매개변수로 받아온 유닛 선택설정
    /// </summary>
    private void SelectUnit(BasicUnit selectUnit)
    {
        // 유닛이 선택되었을 때
        selectUnit.SelectUnit();
        // 선택한 유닛 정보를 리스트에 저장
        selectUnitList.Add(selectUnit);
    }
    /// <summary>
    /// 매개변수로 받아온 유닛 선택 해제 설정
    /// </summary>
    private void DeSelectUnit(BasicUnit selectUnit)
    {
        //유닛이 해제되었을 때
        selectUnit.DeSelectUnit();
        // 선택한 유닛 정보를 리스트에서 삭제
        selectUnitList.Remove(selectUnit);
    }
    private void OnDestroy()
    {
        unitList.Clear();
    }
}
