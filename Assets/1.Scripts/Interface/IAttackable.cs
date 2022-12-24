using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackble
{
    void FindTarget();                  // 적탐색 함수
    IEnumerator Attack();               // 공격 패턴 코루틴
    void OnDrawGizmosSelected();        // SceneView 기즈모 
    Vector3 AngleToDir(float angle);    // 기즈모 시야 각도
}
