using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class SimpleNPCDialogue : MonoBehaviour
{
    [Header("Reference")]
    public GameFlow gameFlow;
    public RectTransform npc;
    public RectTransform frame;
    public TMP_Text text;

    [Header("NPC Position")]
    public Vector2 npcOffScreenPos;
    public Vector2 npcOnScreenPos;

    [Header("Frame Position")]
    public Vector2 frameOffScreenPos;
    public Vector2 frameOnScreenPos;

    [Header("NPC Scale")]
    public Vector3 npcStartScale = Vector3.zero;
    public Vector3 npcTargetScale = Vector3.one;

    [Header("Frame Scale")]
    public Vector3 frameStartScale = Vector3.zero;
    public Vector3 frameTargetScale = Vector3.one;

    [Header("Speed")]
    public float slideSpeed = 8f;
    public float scaleSpeed = 8f;
    public float typingSpeed = 0.04f;

    [Header("NPC Idle Motion")]
    public float rotateAmplitude = 5f;     // độ xoay (độ)
    public float rotateSpeed = 2f;          // tốc độ xoay

    bool isTyping;
    bool isShowing;
    bool skipTyping;

    string fullText;
    Coroutine dialogueRoutine;

    Quaternion npcBaseRotation;

    void Awake()
    {
        npcBaseRotation = npc.localRotation;
        ResetVisual();
    }

    void ResetVisual()
    {
        npc.anchoredPosition = npcOffScreenPos;
        frame.anchoredPosition = frameOffScreenPos;

        npc.localScale = npcStartScale;
        frame.localScale = frameStartScale;

        npc.localRotation = npcBaseRotation;

        text.text = "";

        isShowing = false;
        isTyping = false;
        skipTyping = false;
    }

    // ===================== PUBLIC =====================

    public void Speak(string sentence)
    {
        fullText = sentence;

        if (dialogueRoutine != null)
            StopCoroutine(dialogueRoutine);

        dialogueRoutine = StartCoroutine(DialogueFlow());
    }

    public void Hide()
    {
        if (dialogueRoutine != null)
            StopCoroutine(dialogueRoutine);

        StartCoroutine(HideRoutine());
    }

    // ===================== CORE =====================

    IEnumerator DialogueFlow()
    {
        // ===== CHỈ ANIMATE LẦN ĐẦU =====
        if (!isShowing)
        {
            isShowing = true;

            while (
                Vector2.Distance(npc.anchoredPosition, npcOnScreenPos) > 0.5f ||
                Vector2.Distance(frame.anchoredPosition, frameOnScreenPos) > 0.5f
            )
            {
                npc.anchoredPosition =
                    Vector2.Lerp(npc.anchoredPosition, npcOnScreenPos, Time.deltaTime * slideSpeed);
                frame.anchoredPosition =
                    Vector2.Lerp(frame.anchoredPosition, frameOnScreenPos, Time.deltaTime * slideSpeed);

                npc.localScale =
                    Vector3.Lerp(npc.localScale, npcTargetScale, Time.deltaTime * scaleSpeed);
                frame.localScale =
                    Vector3.Lerp(frame.localScale, frameTargetScale, Time.deltaTime * scaleSpeed);

                yield return null;
            }

            npc.anchoredPosition = npcOnScreenPos;
            frame.anchoredPosition = frameOnScreenPos;
            npc.localScale = npcTargetScale;
            frame.localScale = frameTargetScale;
        }

        // ===== TYPE TEXT =====
        text.text = "";
        isTyping = true;
        skipTyping = false;

        foreach (char c in fullText)
        {
            if (skipTyping) break;

            text.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        text.text = fullText;
        isTyping = false;
    }

    IEnumerator HideRoutine()
    {
        isShowing = false;

        while (
            Vector2.Distance(npc.anchoredPosition, npcOffScreenPos) > 0.5f ||
            Vector2.Distance(frame.anchoredPosition, frameOffScreenPos) > 0.5f
        )
        {
            npc.anchoredPosition =
                Vector2.Lerp(npc.anchoredPosition, npcOffScreenPos, Time.deltaTime * slideSpeed);
            frame.anchoredPosition =
                Vector2.Lerp(frame.anchoredPosition, frameOffScreenPos, Time.deltaTime * slideSpeed);

            yield return null;
        }

        ResetVisual();
    }

    // ===================== INPUT =====================

    void Update()
    {
        // ===== IDLE ROTATION NPC =====
        if (isShowing)
        {
            float angle =
                Mathf.Sin(Time.time * rotateSpeed) * rotateAmplitude;
            npc.localRotation =
                npcBaseRotation * Quaternion.Euler(0, 0, angle);
        }

        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (isTyping)
            {
                skipTyping = true; // click 1 → skip chữ
            }
            else
            {
                gameFlow.NextDialogue(); // click 2 → câu tiếp
            }
        }
    }
}
