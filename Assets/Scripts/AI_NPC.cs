using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#region DATA STRUCT
[System.Serializable]
public class Message
{
    public string role;
    public string content;
}

[System.Serializable]
public class ChatRequest
{
    public string model;
    public Message[] messages;
    public ResponseFormat response_format = new ResponseFormat { type = "json_object" };
}

[System.Serializable]
public class ResponseFormat { public string type; }

[System.Serializable]
public class GroqResponse
{
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public MessageContent message;
}

[System.Serializable]
public class MessageContent
{
    public string content;
}

// ===== AI RESULT =====
[System.Serializable]
public class AIScoreResult
{
    public string reply;
    public int score;
}
#endregion

public class AI_NPC : MonoBehaviour
{
    [Header("Prompt AI (System)")]
    [TextArea(10, 1000)]
    public string promptAI;

    [Header("Player Input")]
    [TextArea(3, 10)]
    public string playerQuestion;

    [Header("API Key (KHÔNG COMMIT)")]
    public string apiKey;

    [Header("Output")]
    public int lastScore;
    [TextArea(3, 10)]
    public string lastReply;

    [Header("Liên kết UI")]
    public DialogueUIController dialoCtr; 

    public void StartChat()
    {
        Debug.Log("<color=yellow>Người chơi chúc:</color> " + playerQuestion);
        StartCoroutine(PostToGroq());
    }

    IEnumerator PostToGroq()
    {
        string url = "https://api.groq.com/openai/v1/chat/completions";

        ChatRequest requestData = new ChatRequest
        {
            model = "llama-3.1-8b-instant",
            messages = new Message[]
            {
                new Message { role = "system", content = promptAI },
                new Message { role = "user", content = playerQuestion }
            }
        };

        string jsonPayload = JsonUtility.ToJson(requestData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonPayload));
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Authorization", "Bearer " + apiKey.Trim());
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            GroqResponse response = JsonUtility.FromJson<GroqResponse>(request.downloadHandler.text);
            string rawContent = response.choices[0].message.content;

            Debug.Log("<color=green>AI RAW CONTENT:</color>\n" + rawContent);
            ParseAIResult(rawContent);
        }
        else
        {
            Debug.LogError("LỖI API: " + request.downloadHandler.text);
        }
    }

    void ParseAIResult(string raw)
    {
        try
        {
            AIScoreResult data = JsonUtility.FromJson<AIScoreResult>(raw);

            lastScore = Mathf.Clamp(data.score, 0, 10);
            lastReply = data.reply;

            // IN CẢ REPLY VÀ SCORE RA ĐÂY
            Debug.Log("<color=cyan>NPC nói:</color> " + lastReply);
            Debug.Log("<color=orange>ĐIỂM SỐ:</color> " + lastScore);

            // ĐẨY CHỮ LÊN UI
            if (dialoCtr != null)
            {
                dialoCtr.NPC_Say(lastReply);
            }
        }
        catch
        {
            Debug.LogError("❌ AI trả JSON sai format hoặc thiếu trường!");
            Debug.LogError("Raw trả về: " + raw);
        }
    }
}