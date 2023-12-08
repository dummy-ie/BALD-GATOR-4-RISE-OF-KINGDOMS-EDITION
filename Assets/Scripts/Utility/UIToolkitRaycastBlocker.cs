using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIToolkitRaycastBlocker : MonoBehaviour
{
    VisualElement container;
    static List<UIToolkitRaycastBlocker> AllRayCastBlockers = new();

    private void OnEnable()
    {
        VisualElement rve = GetComponent<UIDocument>().rootVisualElement;
        container = rve[0];

        AllRayCastBlockers.Add(this);
    }

    private void OnDisable()
    {
        AllRayCastBlockers.Remove(this);
    }

    public bool IsMouseOverBlocker(Vector3 mousePosWorld)
    {
        Vector2 mousePosPanel = RuntimePanelUtils.CameraTransformWorldToPanel(container.panel, mousePosWorld, Camera.main);

        Rect layout = container.layout;
        Vector3 pos = container.transform.position;
        Rect blockingArea = new Rect(pos.x, pos.y, layout.width, layout.height);

        if (mousePosPanel.x <= blockingArea.xMax && mousePosPanel.x >= blockingArea.xMin && mousePosPanel.y <= blockingArea.yMax && mousePosPanel.y >= blockingArea.yMin)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public static bool IsMouseOverBlocker()
    {
        Vector3 mousePosScreen = Input.mousePosition;
        mousePosScreen.z = Camera.main.nearClipPlane;
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);

        foreach(var blocker in AllRayCastBlockers)
        {
            if(blocker.IsMouseOverBlocker(mousePosWorld))
            {
                return true;
            }
        }
        return false;
    }
}