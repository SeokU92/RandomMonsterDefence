using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergePoint : MonoBehaviour
{
    Renderer rd;
    [SerializeField] private GameObject mergePoint;
    [SerializeField] private Color mergePointColor;
    private void Awake()
    {
        rd = GetComponent<Renderer>();
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        if(rd.material.color == Color.green)
    //        {
              
    //        }
    //        if(rd.material.color == Color.red)
    //        {
              
    //        }
    //    }
    //}
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Unit")))
        {
            rd.material.color = other.gameObject.GetComponent<BasicUnit>().unitColor;    //유닛의 고유 색을 mergePoint에게 전달        
        }      
    }
    private void OnTriggerExit(Collider other)
    {
        rd.material.color = Color.yellow;        
    }
}
