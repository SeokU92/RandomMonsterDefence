using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// ����Ƽ ����
/// User Layer6 : Unit
/// User Layer7 : Gound
/// User Layer8 : Enemy
/// User Layer9 : Obstacle
/// User Layer10 : Skill
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public BasicUnit unit;
    [HideInInspector] public BasicEnemy enemy;
    [HideInInspector] public UnitManager unitManager;

    [SerializeField] private TextMeshProUGUI goldText;      //���
    [SerializeField] private TextMeshProUGUI lifeText;      //��������
    [SerializeField] private TextMeshProUGUI cost;          //���� ���ź��
    private int gold;
    public int Gold
    {
        get { return gold; }
        set 
        {
            gold = value; 
        }
    }
    private int life;
    public int Life
    {
        get { return life; }
        set 
        { 
            life = value; 
            if(life <= 0)
            {
                GameOver();
            }
        }
    }
    private int purchaseCost;
    public int PurchaseCost
    {
        get { return purchaseCost; }
        set { purchaseCost = value; }
    }
    private void OnEnable()
    {
        gold = 100;
        life = 10;
        purchaseCost = 10;        
    }

    private void Update()
    {
        State();
    }
    private void State()
    {
        goldText.text = gold.ToString();
        lifeText.text = life.ToString();
        cost.text = "Cost : " + purchaseCost.ToString();
    }
    public void GameOver()
    {

    }
}
