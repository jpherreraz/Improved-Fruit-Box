using UnityEngine;

public class Selector : MonoBehaviour
{
    private Vector2 startMousePos;
    private Vector2 currentMousePos;
    private bool dragging = false;
    private bool finishedDragging = false;
    [SerializeField]
    private Transform selectionBox;
    [SerializeField]
    new private Camera camera;
    

    void Update()
    {
        // Mouse button down: capture start position
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            dragging = true;
            selectionBox.gameObject.SetActive(true); // Show the selection box
        }

        // Mouse is being held: update current position
        if (Input.GetMouseButton(0) && dragging)
        {
            currentMousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            UpdateSelectionBox();
        }

        // Mouse button released: finalize the selection
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            finishedDragging = true;
            selectionBox.gameObject.SetActive(false); // Hide the selection box
        }
    }

    // Update the size and position of the selection box
    void UpdateSelectionBox()
    {
        Vector2 boxSize = new Vector2(currentMousePos.x - startMousePos.x, currentMousePos.y - startMousePos.y);
        selectionBox.localScale = new Vector3(Mathf.Abs(boxSize.x), Mathf.Abs(boxSize.y), 1);
        selectionBox.position = new Vector3((startMousePos.x + boxSize.x/2), startMousePos.y + boxSize.y/2, -1);
    }

    public bool hasFinishedDragging() {
        if (finishedDragging) {
            finishedDragging = false;
            return true;
        }
        return false;
    }

    public Vector2 getCurrentMousePos() {
        return currentMousePos;
    }

    public Vector2 getStartMousePos() {
        return startMousePos;
    }
}
