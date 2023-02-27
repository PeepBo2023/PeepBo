using Naninovel;
using PeepBo.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Room : MonoBehaviour
{
    [SerializeField] bool IsMovable;

    private Vector2 touchPosition, delta;
    private Camera mainCamera;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    private float screenX, screenY;
    private float limitX, limitY;

    void Start()
    {
        var cameraManager = Engine.GetService<ICameraManager>();
        mainCamera = cameraManager.Camera;
        screenY = mainCamera.orthographicSize / 2;
        screenX = screenY / Screen.height * Screen.width;
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Vector3 spriteSize = spriteRenderer.sprite.bounds.size;
        boxCollider.size = spriteSize;

        float factor = transform.lossyScale.x;
        limitX = (spriteSize.x * factor) / 2 - screenX * 2;
        limitY = (spriteSize.y * factor) / 2 - screenY * 2;
    }

    bool CanProcessInput()
    {
        var scriptPlayer = Engine.GetService<IScriptPlayer>();
        var inputManager = Engine.GetService<IInputManager>();
        if (scriptPlayer.Playing) // printer 나오고 있으면
            return false;

        if (inputManager.ProcessInput) // 나니 input을 받고 있으면
            return false;

        return true;
    }

    public void OnMouseDown()
    {
        if (!CanProcessInput()) return;

        if (IsMovable)
            touchPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnMouseDrag()
    {
        if (!CanProcessInput()) return;

        if (IsMovable)
        {
            delta = new Vector2(mainCamera.ScreenToWorldPoint(Input.mousePosition).x - touchPosition.x, mainCamera.ScreenToWorldPoint(Input.mousePosition).y - touchPosition.y);
            touchPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.localPosition = new Vector3(Mathf.Clamp(transform.position.x + delta.x, -limitX, limitX), Mathf.Clamp(transform.position.y + delta.y, -limitY, limitY), 0);
        }
    }
}
