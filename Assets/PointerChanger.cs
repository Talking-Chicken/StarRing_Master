using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerChanger : MonoBehaviour
{
    public static PointerChanger Instance { get; private set; }

    // 为每种InteractableType定义一个鼠标图标
    public Texture2D objCursor;
    public Texture2D npcCursor;
    public Texture2D exmCursor;
    public Texture2D doorCursor;
    public Texture2D interactionCursor;
    public Texture2D normalCursor;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeCursor(InteractableType type)
    {
        switch (type)
        {
            case InteractableType.OBJ:
                Cursor.SetCursor(objCursor, Vector2.zero, CursorMode.Auto);
                break;
            case InteractableType.NPC:
                Cursor.SetCursor(npcCursor, Vector2.zero, CursorMode.Auto);
                break;
            case InteractableType.EXM:
                Cursor.SetCursor(exmCursor, Vector2.zero, CursorMode.Auto);
                break;
            case InteractableType.DOOR:
                Cursor.SetCursor(doorCursor, Vector2.zero, CursorMode.Auto);
                break;
            case InteractableType.INTERACTION:
                Cursor.SetCursor(interactionCursor, Vector2.zero, CursorMode.Auto);
                break;
            case InteractableType.Normal:
                Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
}
