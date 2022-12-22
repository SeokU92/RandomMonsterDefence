using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private GameObject unitPrefab;     //���� ������
    [SerializeField] private Transform unitSpawnTrans;    //���� ��ȯ ��ġ

    private List<UnitController> selectUnitList = new List<UnitController>();   //���õ� ����
    public  List<UnitController> unitList { get; private set; }                 //�ʿ� �����ϴ� ��� ����    


    // ���� ���� ��ư
    public void SpawnUnitButton()
    {
        SpawnUnits();
    }

    // ���� ����
    public List<UnitController> SpawnUnits()
    {
        //List<UnitController> unitList = new List<UnitController>();

        GameObject unitSpawn = Instantiate(unitPrefab, unitSpawnTrans.position, Quaternion.identity);
        UnitController unit = unitSpawn.GetComponent<UnitController>();

        unitList.Add(unit);
        return unitList;
    }   
    /// <summary>
    /// ���콺 Ŭ������ ���� ����
    /// </summary>
    /// <param name="selectUnit"></param>
    public void ClickSelectUnit(UnitController selectUnit)
    {
        DeSelectAll(); //������ ���� �� ���� ����
        SelectUnit(selectUnit);
    }
    /// <summary>
    /// Controll + ���콺 Ŭ�� ���� ����
    /// </summary>
    /// <param name="selectUnit"></param>
    public void ControlClickSelectUnit(UnitController selectUnit)
    {
        if(selectUnitList.Contains(selectUnit)) DeSelectUnit(selectUnit); //������ ���õ� ���� �ߺ�����
        else SelectUnit(selectUnit);                                      //���ο� ���� ����
    }
    /// <summary>
    /// ���콺 �巡�� ���� ����
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
    private void SelectUnit(UnitController selectUnit)
    {
        // ������ ���õǾ��� ��
        selectUnit.SelectUnit();
        // ������ ���� ������ ����Ʈ�� ����
        selectUnitList.Add(selectUnit);
    }
    /// <summary>
    /// �Ű������� �޾ƿ� ���� ���� ���� ����
    /// </summary>
    private void DeSelectUnit(UnitController selectUnit)
    {
        //������ �����Ǿ��� ��
        selectUnit.DeSelectUnit();
        // ������ ���� ������ ����Ʈ���� ����
        selectUnitList.Remove(selectUnit);
    }
}
