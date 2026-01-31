using UnityEngine;
using UnityEngine.InputSystem;

public class Select : MonoBehaviour
{
    public SpriteRenderer image;
    public Sprite normalSprite;
    public Sprite selectSprite;
    public int IdNPC;

    public GameObject gameFlowOJ;
    public GameObject pickIcon;

    private bool isHover = false;
    private GameFlow gameFlow;

    void Awake()
    {
        gameFlow = gameFlowOJ.GetComponent<GameFlow>();
    }

    void Update()
    {
        if (Mouse.current == null || gameFlow.isTalking == true) return; // an toàn

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        bool hitThis = hit.collider != null && hit.collider.gameObject == gameObject;

        if (hitThis && !isHover)
        {
            image.sprite = selectSprite;
            pickIcon.SetActive(true);
            isHover = true;
            gameFlow.setIdNPC(IdNPC);
        }
        else if (!hitThis && isHover)
        {
            image.sprite = normalSprite;
            pickIcon.SetActive(false);
            isHover = false;
            gameFlow.setIdNPC(0);
        }
    }
}
