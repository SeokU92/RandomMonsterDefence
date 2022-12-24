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
    [SerializeField] private Transform startPoint;  //시작지점
    [SerializeField] private int spawnDelay;        //소환 딜레이
    public List<Stage> stages = new List<Stage>();  //스테이지 단계 구성
    [Serializable]
    public class Stage
    {
        public int level;          //스테이지 단계
        public string enemyType;
        public int maxEnemy;       //라운드당 소환몬스터 수
        public int curEnemy;       //현재 소환된 몬스터 수
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
