using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueUIController : MonoBehaviour
{
    public RectTransform npcFrame, playerFrame;
    public TMP_Text npcText;
    public TMP_InputField playerInput;
    
    [Header("Kéo Object AI_Manager vào đây")]
    public AI_NPC aiScript; 

    public void SubmitChat() 
    {
        if (string.IsNullOrEmpty(playerInput.text)) return;
        
        if (aiScript != null) 
        {
            // Gán dữ liệu vào AI_NPC
            aiScript.playerQuestion = playerInput.text; 
            // Gọi hàm bắt đầu Chat
            aiScript.StartChat(); 
        }
        
        playerInput.text = ""; 
    }

    public void NPC_In() 
    {
        npcFrame.gameObject.SetActive(true);
        playerFrame.gameObject.SetActive(true);
    }

    public void NPC_Say(string content) 
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(content));
    }

    IEnumerator TypeText(string msg) 
    {
        npcText.text = "";
        foreach (char c in msg) {
            npcText.text += c;
            yield return new WaitForSeconds(0.03f);
        }
    }
}