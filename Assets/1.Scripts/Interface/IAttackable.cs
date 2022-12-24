using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackble
{
    void FindTarget();                  // ��Ž�� �Լ�
    IEnumerator Attack();               // ���� ���� �ڷ�ƾ
    void OnDrawGizmosSelected();        // SceneView ����� 
    Vector3 AngleToDir(float angle);    // ����� �þ� ����
}
