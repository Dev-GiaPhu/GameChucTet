using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; // BẮT BUỘC có thêm cái này

public class SimpleNPCDialogue : MonoBehaviour
{
    [Header("References")]
    public GameFlow gameFlow;
    public RectTransform npc;     
    public RectTransform frame;   
    public TMP_Text text;         

    [Header("Vị trí Trượt (Slide Settings)")]
    public Vector2 npcOffScreenPos = new Vector2(-1200, 0); 
    public Vector2 npcOnScreenPos = new Vector2(-450, 0);   
    public Vector2 frameOffScreenPos = new Vector2(0, -800);
    public Vector2 frameOnScreenPos = new Vector2(0, -350);

    [Header("Cấu hình Hiệu ứng")]
    public float slideSpeed = 8f;     
    public float rotateAmplitude = 8f; 
    public float rotateSpeed = 10f;    
    public float typingSpeed = 0.04f;

    private bool isTyping;
    private bool isShowing;
    private bool skipTyping;
    private string fullText;
    
    private Quaternion npcBaseRotation;
    private Vector3 npcTargetScale; 
    private Vector3 frameTargetScale;

    void Awake()
    {
        if (npc != null) 
        {
            npcBaseRotation = npc.localRotation;
            npcTargetScale = npc.localScale;
        }
        if (frame != null) frameTargetScale = frame.localScale;
        ResetVisual();
    }

    void ResetVisual()
    {
        isShowing = false;
        npc.anchoredPosition = npcOffScreenPos;
        frame.anchoredPosition = frameOffScreenPos;
        npc.localScale = Vector3.zero;
        frame.localScale = Vector3.zero;
    }

    public void Speak(string content)
    {
        fullText = content;
        if (isShowing)
        {
            StopAllCoroutines();
            StartCoroutine(TypeTextOnly());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(ShowRoutine());
        }
    }

    IEnumerator TypeTextOnly()
    {
        text.text = "";
        isTyping = true;
        skipTyping = false;
        foreach (char c in fullText)
        {
            if (skipTyping) break;
            text.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        text.text = fullText;
        isTyping = false;
    }

    IEnumerator ShowRoutine()
    {
        isShowing = true;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * slideSpeed;
            npc.anchoredPosition = Vector2.Lerp(npc.anchoredPosition, npcOnScreenPos, t);
            frame.anchoredPosition = Vector2.Lerp(frame.anchoredPosition, frameOnScreenPos, t);
            npc.localScale = Vector3.Lerp(Vector3.zero, npcTargetScale, t);
            frame.localScale = Vector3.Lerp(Vector3.zero, frameTargetScale, t);
            yield return null;
        }
        yield return StartCoroutine(TypeTextOnly());
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(HideRoutine());
    }

    IEnumerator HideRoutine()
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * slideSpeed;
            npc.anchoredPosition = Vector2.Lerp(npc.anchoredPosition, npcOffScreenPos, t);
            frame.anchoredPosition = Vector2.Lerp(frame.anchoredPosition, frameOffScreenPos, t);
            npc.localScale = Vector3.Lerp(npc.localScale, Vector3.zero, t);
            frame.localScale = Vector3.Lerp(frame.localScale, Vector3.zero, t);
            yield return null;
        }
        isShowing = false;
    }

    void Update()
    {
        if (isShowing && npc != null)
        {
            float angle = Mathf.Sin(Time.time * rotateSpeed) * rotateAmplitude;
            npc.localRotation = npcBaseRotation * Quaternion.Euler(0, 0, angle);
        }

        // KIỂM TRA CLICK CHUỘT NHƯNG CHỪA CÁC NÚT BẤM (BUTTON) RA
        if (isShowing && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Nếu chuột ĐANG KHÔNG đè lên một UI Object nào đó (như Button)
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (isTyping) skipTyping = true;
                else gameFlow.NextDialogue();
            }
        }
    }
}