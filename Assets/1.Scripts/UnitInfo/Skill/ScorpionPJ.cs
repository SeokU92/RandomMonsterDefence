using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionPJ : MonoBehaviour
{
    BasicUnit unit;        
    private Transform trans;
    //public Renderer rd;
    
    private void Awake()
    {
        trans = GetComponent<Transform>();
    }
    private void Update()
    {
        gameObject.transform.Translate(Vector3.forward * 10f * Time.deltaTime);
    }
    //public void SetColor(Color color)
    //{
    //    rd.material.SetColor("_EmissionColor", color);
    //}
    //public void SetSize(Transform trans)
    //{
    //    gameObject.transform.localScale = trans.localScale;
    //}
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            Debug.Log(other.name);
            gameObject.SetActive(false);   
        }
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Obstacle")))
        {
            Debug.Log(other.name);
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        ObjectPooling.ReturnToPool(gameObject);
    }
}
