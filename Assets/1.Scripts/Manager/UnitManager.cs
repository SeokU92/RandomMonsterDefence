using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum UnitTypeOne
{
    ReaperOne, PlantOne, ScorpionOne, WormOne
}
public enum UnitTypeTwo
{
    ReaperTwo, PlantTwo, ScorpionTwo, WormTwo
}
public enum UnitTypeThree
{
    ReaperThree, PlantThree, ScorpionThree, WormThree
}
public class UnitManager : MonoBehaviour
{
    [SerializeField] private Transform unitSpawnTrans;                //���� ��ȯ ��ġ

    private List<BasicUnit> selectUnitList = new List<BasicUnit>();   //���õ� ����
    public List<BasicUnit> unitList { get; private set; }             //�ʿ� �����ϴ� ��� ����    

    private void Awake()
    {
        unitList = new List<BasicUnit>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnUnits();

        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SellUnits();
        }
    }

    // ���� ����
    public List<BasicUnit> SpawnUnits()
    {        
        List<BasicUnit> unitLists = new List<BasicUnit>();

        UnitTypeOne UT = (UnitTypeOne)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(UnitTypeOne)).Length));
        GameObject units = ObjectPooling.SpawnFromPool(UT.ToString(), unitSpawnTrans.position);
        BasicUnit unit = units.GetComponent<BasicUnit>();
        unitLists.Add(unit); 
        unitList.Add(unit);  //�� ��ü ���� ����Ʈ�� �߰�

        return unitLists;
    }  
    public List<BasicUnit> SellUnits()
    {
        for(int i = 0; i < selectUnitList.Count; i++)
        {
            BasicUnit unit = selectUnitList[i];
            unit.SellUnit();
            unitList.Remove(unit);
        }
        return unitList;
    }
    /// <summary>
    /// ���콺 Ŭ������ ���� ����
    /// </summary>
    /// <param name="selectUnit"></param>
    public void ClickSelectUnit(BasicUnit selectUnit)
    {
        DeSelectAll(); //������ ���� �� ���� ����
        SelectUnit(selectUnit);
    }
    /// <summary>
    /// Controll + ���콺 Ŭ�� ���� ����
    /// </summary>
    /// <param name="selectUnit"></param>
    public void ControlClickSelectUnit(BasicUnit selectUnit)
    {
        if(selectUnitList.Contains(selectUnit)) DeSelectUnit(selectUnit); //������ ���õ� ���� �ߺ�����
        else SelectUnit(selectUnit);                                      //���ο� ���� ����
    }
    /// <summary>
    /// ���콺 �巡�� ���� ����
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
    /// ���õ� ������� �̵�
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
    /// ��� ���� ���� ����
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
    /// �Ű������� �޾ƿ� ���� ���ü���
    /// </summary>
    private void SelectUnit(BasicUnit selectUnit)
    {
        // ������ ���õǾ��� ��
        selectUnit.SelectUnit();
        // ������ ���� ������ ����Ʈ�� ����
        selectUnitList.Add(selectUnit);
    }
    /// <summary>
    /// �Ű������� �޾ƿ� ���� ���� ���� ����
    /// </summary>
    private void DeSelectUnit(BasicUnit selectUnit)
    {
        //������ �����Ǿ��� ��
        selectUnit.DeSelectUnit();
        // ������ ���� ������ ����Ʈ���� ����
        selectUnitList.Remove(selectUnit);
    }
    private void OnDestroy()
    {
        unitList.Clear();
    }
}
