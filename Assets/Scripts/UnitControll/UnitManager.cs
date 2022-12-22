using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private GameObject unitPrefab;     //유닛 프리팹
    [SerializeField] private Transform unitSpawnTrans;    //유닛 소환 위치

    private List<UnitController> selectUnitList = new List<UnitController>();   //선택된 유닛
    public  List<UnitController> unitList { get; private set; }                 //맵에 존재하는 모든 유닛    


    // 유닛 스폰 버튼
    public void SpawnUnitButton()
    {
        SpawnUnits();
    }

    // 유닛 스폰
    public List<UnitController> SpawnUnits()
    {
        //List<UnitController> unitList = new List<UnitController>();

        GameObject unitSpawn = Instantiate(unitPrefab, unitSpawnTrans.position, Quaternion.identity);
        UnitController unit = unitSpawn.GetComponent<UnitController>();

        unitList.Add(unit);
        return unitList;
    }   
    /// <summary>
    /// 마우스 클릭으로 유닛 선택
    /// </summary>
    /// <param name="selectUnit"></param>
    public void ClickSelectUnit(UnitController selectUnit)
    {
        DeSelectAll(); //기존에 선택 된 유닛 해제
        SelectUnit(selectUnit);
    }
    /// <summary>
    /// Controll + 마우스 클릭 유닛 선택
    /// </summary>
    /// <param name="selectUnit"></param>
    public void ControlClickSelectUnit(UnitController selectUnit)
    {
        if(selectUnitList.Contains(selectUnit)) DeSelectUnit(selectUnit); //기존에 선택된 유닛 중복선택
        else SelectUnit(selectUnit);                                      //새로운 유닛 선택
    }
    /// <summary>
    /// 마우스 드래그 유닛 선택
    /// </summary>
    /// <param name="selectUnit"></param>
    public void DragSelectUnit(UnitController selectUnit)
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
    private void SelectUnit(UnitController selectUnit)
    {
        // 유닛이 선택되었을 때
        selectUnit.SelectUnit();
        // 선택한 유닛 정보를 리스트에 저장
        selectUnitList.Add(selectUnit);
    }
    /// <summary>
    /// 매개변수로 받아온 유닛 선택 해제 설정
    /// </summary>
    private void DeSelectUnit(UnitController selectUnit)
    {
        //유닛이 해제되었을 때
        selectUnit.DeSelectUnit();
        // 선택한 유닛 정보를 리스트에서 삭제
        selectUnitList.Remove(selectUnit);
    }
}
