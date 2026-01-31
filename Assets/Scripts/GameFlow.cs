using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameFlow : MonoBehaviour
{
    public SimpleNPCDialogue npcSpeak;
    [TextArea(3,50)]
    public List<string> dialogue;
    public int selectNPC;
    private int index;
    public bool isTalking = false;

    [Header("BlackImage")]
    public RectTransform blackImage;
    public Vector3 startPos;
    public Vector3 endPos;
    public float moveSpeed;
    public void setIdNPC(int id)
    {
        selectNPC = id;
    }

    public void Update()
    {
        if(selectNPC != 0 && Input.GetMouseButtonDown(0) && !isTalking)
        {
            
        }
    }

    public void StartFlow()
    {
        blackImage.transform.position = startPos;
        StartCoroutine(Flow());
    }

    IEnumerator Flow()
    {
        yield return new WaitForSeconds(2f);
        while (Vector2.Distance(blackImage.anchoredPosition, endPos) > 200f)
        {
            blackImage.anchoredPosition =
                Vector2.Lerp(
                    blackImage.anchoredPosition,
                    endPos,
                    Time.deltaTime * moveSpeed
                );

            yield return null;
        }

        blackImage.anchoredPosition = endPos;

        Debug.Log("GameFlow Start");
        NextDialogue();
    }

    void Start()
    {
        StartFlow();
    }

    public void NextDialogue()
    {
        if (index >= dialogue.Count)
        {
            npcSpeak.Hide(); // <-- BẮT 
            isTalking = false;
            return;
        }
        isTalking = true;
        npcSpeak.Speak(dialogue[index]);

        index++;
    }
}