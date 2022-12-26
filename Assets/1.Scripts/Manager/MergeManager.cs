using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitTypeOne
{
    Reaper, Plant, Scorpion, Worm
}
public enum UnitTypeTwo
{
    Reaper, Plant, Scorpion, Worm
}
public enum UnitTypeThree
{
    Reaper, Plant, Scorpion, Worm
}
public class MergeManager : Singleton<MergeManager>
{
    [SerializeField] private GameObject mergePoint1;
    [SerializeField] private GameObject mergePoint2;
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    Merge();
        //}
    }
    public void Merge()
    {
        Color mergePointColor1 = mergePoint1.GetComponent<Renderer>().material.color;
        Color mergePointColor2 = mergePoint2.GetComponent<Renderer>().material.color;
        if (mergePointColor1 == mergePointColor2)
        {
            mergePoint1.GetComponent<Renderer>().material.color = Color.green;
            mergePoint2.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            mergePoint1.GetComponent<Renderer>().material.color = Color.red;
            mergePoint2.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}