using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionPJ : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Return());
    }
    private void Update()
    {
        gameObject.transform.Translate(Vector3.forward * 20f * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            GameManager.Instance.enemy.Hit(GameManager.Instance.unit.damage);
            gameObject.SetActive(false);   
        }
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Obstacle")))
        {
            gameObject.SetActive(false);
        }
    }
    IEnumerator Return()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        ObjectPooling.ReturnToPool(gameObject);
    }
}
