using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameFlow : MonoBehaviour
{
    public SimpleNPCDialogue npcSpeak;
    public DialogueUIController dialoCtr;
    [TextArea(3,50)] public List<string> dialogue;
    public int selectNPC;
    private int index;
    public bool isTalking = false;

    [Header("BlackImage Settings")]
    public RectTransform blackImage;
    public Vector3 startPos;
    public Vector3 endPos;
    public float moveSpeed = 5f;

    public void setIdNPC(int id) { if (!isTalking) selectNPC = id; }

    void Start()
    {
        isTalking = true;
        blackImage.anchoredPosition = startPos;
        StartCoroutine(Flow());
    }

    IEnumerator Flow()
    {
        yield return new WaitForSeconds(1f);
        while (Vector2.Distance(blackImage.anchoredPosition, endPos) > 10f)
        {
            blackImage.anchoredPosition = Vector2.Lerp(blackImage.anchoredPosition, endPos, Time.deltaTime * moveSpeed);
            yield return null;
        }
        blackImage.anchoredPosition = endPos;
        yield return new WaitForSeconds(0.5f);
        NextDialogue();
    }

    public void NextDialogue()
    {
        if (index >= dialogue.Count)
        {
            npcSpeak.Hide();
            isTalking = false; 
            index = 0; 
            return;
        }
        isTalking = true;
        npcSpeak.Speak(dialogue[index]);
        index++;
    }

    public void Chat()
    {
        if (selectNPC == 0) return;
        isTalking = true;
        dialoCtr.NPC_In();
        
        string intro = selectNPC switch {
            1 => "Ôi cháu của bà!! Có chuyện gì thế con?",
            2 => "Cháu của ông có chuyện gì muốn nói sao?",
            3 => "Hửm..Có chuyện gì không em~",
            4 => "Muốn nói gì với anh hửm~?",
            5 => "Kêu anh có gì không?",
            _ => "Hửm?"
        };
        dialoCtr.NPC_Say(intro);
    }
}