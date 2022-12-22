using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    [Header("MouseDrag")]
    [SerializeField]
    private RectTransform dragRectangle;         // 마우스로 드래그한 범위 가시화
    private Rect dragRect;                       // 마우스로 드래그한 범위
    private Vector2 startPos = Vector2.zero;     // 드래그 시작위치
    private Vector2 endPos = Vector2.zero;       // 드래그 종료위치

    private Camera mainCam;
    private UnitManager unitManager;

    private void Awake()
    {
        mainCam = Camera.main;
        unitManager = GetComponent<UnitManager>();

        DrawDragRectangle();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            dragRect = new Rect();
        }
        if (Input.GetMouseButton(0))
        {
            endPos = Input.mousePosition;
            //마우스 클릭한 상태로 드래그 범위
            DrawDragRectangle();
        }
        if (Input.GetMouseButtonUp(0))
        {
            //마우스 클릭을 종료할 때 드래그 범위 내에 유닛 선택
            CalculateDragRect();
            SelectUnits();

            //마우스 클릭을 종료할 때 드래그 범위 초기화
            startPos = endPos = Vector2.zero;
            DrawDragRectangle();
        }
    }
    private void MouseDrags()
    {
        
    }
    private void DrawDragRectangle()
    {
        //드래그 범위 위치
        dragRectangle.position = (startPos + endPos) * 0.5f;
        //드래그 범위 크기
        dragRectangle.sizeDelta = new Vector2(Mathf.Abs(startPos.x - endPos.x), Mathf.Abs(startPos.y - endPos.y));
    }
    private void CalculateDragRect()
    {
        if (Input.mousePosition.x < startPos.x)
        {
            dragRect.xMin = Input.mousePosition.x;
            dragRect.xMax = startPos.x;
        }
        else
        {
            dragRect.xMin = startPos.x;
            dragRect.xMax = Input.mousePosition.x;
        }
        if (Input.mousePosition.y < startPos.y)
        {
            dragRect.yMin = Input.mousePosition.y;
            dragRect.yMax = startPos.y;
        }
        else
        {
            dragRect.yMin = startPos.y;
            dragRect.yMax = Input.mousePosition.y;
        }
    }
    private void SelectUnits()
    {
        //모든 유닛 검사
        foreach (UnitController unit in unitManager.unitList)
        {
            //유닛 월드좌표를 화면 좌표로 변환 후 드래그 범위 내에 있는지 검사
            if (dragRect.Contains(mainCam.WorldToScreenPoint(unit.transform.position)))
            {
                unitManager.DragSelectUnit(unit);
            }
        }
    }
}
