using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormDeath : MonoBehaviour
{    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            GameManager.Instance.enemy.Hit(GameManager.Instance.unit.damage);
            StartCoroutine(WaitCo());
        }
    }
    IEnumerator WaitCo()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        ObjectPooling.ReturnToPool(gameObject);
    }
}
