using UnityEngine;
using UnityEngine.EventSystems;

public class CursorGrabber : MonoBehaviour, IPointerDownHandler
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void MouseGrab()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MouseGrab();
    }
}
