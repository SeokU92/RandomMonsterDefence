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

    [SerializeField] private TextMeshProUGUI roundNum;      //���� ǥ��
    [SerializeField] private Transform startPoint;          //��������
    [SerializeField] private int spawnDelay;                //��ȯ ������
    [SerializeField] private float roundDelay;              //���� �������� ���ð�
    [SerializeField] private int curStageLevel;             //���� �������� ����
    public List<Stage> stages = new List<Stage>();          //�������� �ܰ� ����

    [Serializable]
    public class Stage
    {
        public int level;          //�������� �ܰ�
        public string enemyType;   //���庰 ���� ����
        public int maxEnemy;       //����� ��ȯ���� ��
        public int curEnemy;       //���� ��ȯ�� ���� ��
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
        if (stages.Count <= 1)                                                          // ����Ʈ�� �� ��� �ڷ�ƾ�� ��������
        {
            StopCoroutine(SpawnEnemy());
            stages.Clear();
            yield break;
        }
        if (stages[stage.level].level == curStageLevel)                                 // �������� ������ ���� ������ ������ �������� ����
        {
            roundNum.text = (curStageLevel + 1).ToString();
            while (stages[stage.level].curEnemy < stages[stage.level].maxEnemy)         // �ִ� ��ȯ ������ ����
            {
                ObjectPooling.SpawnFromPool(stage.enemyType, startPoint.position);
                stages[stage.level].curEnemy++;
                yield return new WaitForSeconds(spawnDelay);                            // ��ȯ ��Ÿ��
            } 
            yield return new WaitForSeconds(roundDelay);                                // �������� �ڵ����� �Ѿ���� �ð�
            stages.Remove(stages[0]);                                                   // �� ���尡 ������ ����Ʈ���� ����
            curStageLevel++;
            stage.enemyType = stages[0].enemyType;
            StartCoroutine(SpawnEnemy());
           
        }
    }
}
