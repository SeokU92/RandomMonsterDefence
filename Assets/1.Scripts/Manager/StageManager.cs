using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro; 
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

    [SerializeField] private TextMeshProUGUI roundNum;      //라운드 표시
    [SerializeField] private Transform startPoint;          //시작지점
    [SerializeField] private int spawnDelay;                //소환 딜레이
    [SerializeField] private float roundDelay;              //다음 레벨구간 대기시간
    [SerializeField] private int curStageLevel;             //현재 스테이지 레벨
    public List<Stage> stages = new List<Stage>();          //스테이지 단계 구성

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
    }    
    IEnumerator SpawnEnemy()
    {
        if (stages.Count <= 1)                                                          // 리스트가 다 비면 코루틴을 빠져나감
        {
            StopCoroutine(SpawnEnemy());
            stages.Clear();
            yield break;
        }
        if (stages[stage.level].level == curStageLevel)                                 // 스테이지 레벨과 현재 레벨이 맞으면 스테이지 시작
        {
            roundNum.text = (curStageLevel + 1).ToString();
            while (stages[stage.level].curEnemy < stages[stage.level].maxEnemy)         // 최대 소환 마릿수 제한
            {
                ObjectPooling.SpawnFromPool(stage.enemyType, startPoint.position);
                stages[stage.level].curEnemy++;
                yield return new WaitForSeconds(spawnDelay);                            // 소환 쿨타임
            } 
            yield return new WaitForSeconds(roundDelay);                                // 다음라운드 자동으로 넘어가지는 시간
            stages.Remove(stages[0]);                                                   // 한 라운드가 끝나면 리스트에서 삭제
            curStageLevel++;
            stage.enemyType = stages[0].enemyType;
            StartCoroutine(SpawnEnemy());
           
        }
    }
}
