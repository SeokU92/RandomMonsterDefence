using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EnemyType
{
    Egg, Mushroom, Bird, Spider, Bat, Bigmushroom, Phantom
}
public enum BossType
{
    Boss, Boss2, Boss3, Boss4
}
public class StageManager : MonoBehaviour
{
    BasicEnemy basicEnemy;    
    [SerializeField] private Transform startPoint;  //��������
    [SerializeField] private int spawnDelay;        //��ȯ ������
    public List<Stage> stages = new List<Stage>();  //�������� �ܰ� ����
    [Serializable]
    public class Stage
    {
        public int level;          //�������� �ܰ�
        public string enemyType;
        public int maxEnemy;       //����� ��ȯ���� ��
        public int curEnemy;       //���� ��ȯ�� ���� ��
    }
    [SerializeField] private Stage stage;

    private void Update()
    {        
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(SpawnEnemy());
        }
    }    
    IEnumerator SpawnEnemy()
    {
        while (stage.curEnemy < stage.maxEnemy)
        {
            Debug.Log("222");
            if (stage.level/5 != 0f)
            {
                Debug.Log("111");
                ObjectPooling.SpawnFromPool(stage.enemyType, startPoint.position, startPoint.rotation);
                stage.curEnemy++;
            }            
            if(stage.level/5 == 0f)
            {
                Debug.Log("333");
                ObjectPooling.SpawnFromPool(stage.enemyType, startPoint.position, startPoint.rotation);
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
