using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeUnit : MonoBehaviour
{
    BasicUnit unit;
    UnitManager unitManager;
    private bool isSelect = false;
    [SerializeField, Range(0f, 300f)] private float dragSpeed = 50f;
    private void Awake()
    {
        GameManager.Instance.unitManager = FindObjectOfType<UnitManager>();
        unitManager = GetComponent<UnitManager>();
        unit = GetComponent<BasicUnit>();
    }
    private void OnMouseDown()
    {
        isSelect = true;
    }
    private void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragSpeed);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = objPosition;        
    }
    private void OnMouseUp()
    {
        isSelect = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(isSelect &&  unit.name == other.GetComponent<BasicUnit>().name)
        {
            BasicUnit unit1 = gameObject.GetComponent<BasicUnit>();
            BasicUnit unit2 = other.gameObject.GetComponent<BasicUnit>();
            GameManager.Instance.unitManager.unitList.Remove(unit1);
            GameManager.Instance.unitManager.unitList.Remove(unit2);
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
            if (this.unit.name + "One" == unit.unitType + "One")
            {
                GameObject unitTwo = ObjectPooling.SpawnFromPool(unit.unitType + "Two", transform.position);
                GameManager.Instance.unitManager.unitList.Add(unitTwo.GetComponent<BasicUnit>());
            }
            if(this.unit.name == unit.unitType + "Two")
            {
                GameObject unitThree = ObjectPooling.SpawnFromPool(unit.unitType + "Three", transform.position);
                GameManager.Instance.unitManager.unitList.Add(unitThree.GetComponent<BasicUnit>());
            }
        }    
        else
        {
            isSelect = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
       
    }
}
