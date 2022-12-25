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
    [SerializeField] private float roundDelay;      //다음 레벨구간 대기시간
    [SerializeField] private int curStageLevel;     //현재 스테이지 레벨
    public List<Stage> stages = new List<Stage>();  //스테이지 단계 구성

    [Serializable]
    public class Stage
    {
        public int level;          //스테이지 단계
        public string enemyType;   //라운드별 몬스터 종류
        public int maxEnemy;       //라운드당 소환몬스터 수
        public int curEnemy;       //현재 소환된 몬스터 수
    }
    [SerializeField] private Stage stage;

    private void OnEnable()
    {
        stage.level = 0;
    }
    private void Update()
    {        
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(SpawnEnemy());
        }
        if (stages.Count <= 1)
        {
            StopCoroutine(SpawnEnemy());
            stages.Clear();
        }
    }    
    IEnumerator SpawnEnemy()
    {
        if(stages[stage.level].level == curStageLevel)
        {
            while(stages[stage.level].curEnemy < stages[stage.level].maxEnemy)
            {
                ObjectPooling.SpawnFromPool(stage.enemyType, startPoint.position);
                stages[stage.level].curEnemy++;
                yield return new WaitForSeconds(spawnDelay);
            }
            yield return new WaitForSeconds(roundDelay);
            stages.Remove(stages[0]);
            curStageLevel++;
            stage.enemyType = stages[0].enemyType;
            StartCoroutine(SpawnEnemy());
        }
    }
}
