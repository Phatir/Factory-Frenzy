using UnityEngine;
using UnityEngine.UI;

public class UIWireConnector : MonoBehaviour
{
    [Header("Références UI")]
    public RectTransform[] startPoints;
    public RectTransform[] endPoints;
    public RectTransform linePrefab;
    public Canvas canvas;

    private RectTransform currentLine;
    private RectTransform selectedStart;

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        // Début du tracé
        if (Input.GetMouseButtonDown(0))
        {
            foreach (RectTransform start in startPoints)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(start, mousePos, canvas.worldCamera))
                {
                    selectedStart = start;
                    currentLine = Instantiate(linePrefab, canvas.transform);
                    UpdateLine(selectedStart.position, mousePos);
                    break;
                }
            }
        }

        // Tracé en cours
        if (Input.GetMouseButton(0) && currentLine != null)
        {
            UpdateLine(selectedStart.position, mousePos);
        }

        // Relâchement
        if (Input.GetMouseButtonUp(0) && currentLine != null)
        {
            bool connected = false;

            foreach (RectTransform end in endPoints)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(end, mousePos, null))
                {
                    if (selectedStart.name == end.name.Replace("End", "Start"))
                    {
                        UpdateLine(selectedStart.position, end.position);
                        connected = true;
                    }
                    break;
                }
            }

            if (!connected)
            {
                Destroy(currentLine.gameObject);
            }

            currentLine = null;
            selectedStart = null;
        }
    }

    void UpdateLine(Vector2 startPos, Vector2 endPos)
    {
        Vector2 direction = endPos - startPos;
        float distance = direction.magnitude;

        currentLine.position = startPos;
        currentLine.sizeDelta = new Vector2(distance, currentLine.sizeDelta.y);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentLine.rotation = Quaternion.Euler(0, 0, angle);
    }
}