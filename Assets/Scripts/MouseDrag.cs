using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    [Header("MouseDrag")]
    [SerializeField]
    private RectTransform dragRectangle;         // ���콺�� �巡���� ���� ����ȭ
    private Rect dragRect;                       // ���콺�� �巡���� ����
    private Vector2 startPos = Vector2.zero;     // �巡�� ������ġ
    private Vector2 endPos = Vector2.zero;       // �巡�� ������ġ

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
            //���콺 Ŭ���� ���·� �巡�� ����
            DrawDragRectangle();
        }
        if (Input.GetMouseButtonUp(0))
        {
            //���콺 Ŭ���� ������ �� �巡�� ���� ���� ���� ����
            CalculateDragRect();
            SelectUnits();

            //���콺 Ŭ���� ������ �� �巡�� ���� �ʱ�ȭ
            startPos = endPos = Vector2.zero;
            DrawDragRectangle();
        }
    }
    private void MouseDrags()
    {
        
    }
    private void DrawDragRectangle()
    {
        //�巡�� ���� ��ġ
        dragRectangle.position = (startPos + endPos) * 0.5f;
        //�巡�� ���� ũ��
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
        //��� ���� �˻�
        foreach (UnitController unit in unitManager.unitList)
        {
            //���� ������ǥ�� ȭ�� ��ǥ�� ��ȯ �� �巡�� ���� ���� �ִ��� �˻�
            if (dragRect.Contains(mainCam.WorldToScreenPoint(unit.transform.position)))
            {
                unitManager.DragSelectUnit(unit);
            }
        }
    }
}
