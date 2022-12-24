using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperSlash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            Debug.Log(other.name);
            StartCoroutine(WaitCo());
        }        
    }  
    IEnumerator WaitCo()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        ObjectPooling.ReturnToPool(gameObject);
    }
}
