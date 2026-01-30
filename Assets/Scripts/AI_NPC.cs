
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

[System.Serializable] public class GroqResponse { public Choice[] choices; }
[System.Serializable] public class Choice { public MessageContent message; }
[System.Serializable] public class MessageContent { public string content; }

public class AI_NPC : MonoBehaviour 
{
    // DÁN KEY GSK CỦA BẠN VÀO ĐÂY
    public string apiKey = "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"; 
    public string playerQuestion = "Chúc bà năm mới sống lâu trăm tuổi!";
    public void StartChat() 
    {
        Debug.Log("<color=yellow>Người chơi chúc:</color> " + playerQuestion);
        StartCoroutine(PostToGroq());
    }

    IEnumerator PostToGroq() 
    {
        string url = "https://api.groq.com/openai/v1/chat/completions";
        
        // Làm sạch Key một lần nữa
        string cleanKey = apiKey.Trim();

        // Thay llama3-8b-8192 bằng llama-3.1-8b-instant
        string jsonPayload = "{\"model\": \"llama-3.1-8b-instant\", \"messages\": [" +
        "{\"role\": \"system\", \"content\": \"Bạn là NPC người bà, trong gian ngày mùng 1 tết, và người chơi sẽ chúc tết, bạn hãy đánh giá lời chúc người chơi và trả về điểm số từ 1 đến 10 theo tiêu chí sở thích nhân vật và chúc tết lại người chơi, trả lời ngắn gọn, người bà thường thích được chúc về sức khoẻ và tuổi tác sống lâu, và thường chúc về học tập. Tự xưng là bà và gọi người chơi là con. Trả lời ngắn gọn theo công thức: Bà cảm ơn con! Lời chúc của con bà đánh giá x/10. Bà cũng chúc con .... Ngoài ra bà không đánh giá gì khác.\"}," +
        "{\"role\": \"user\", \"content\": \"" + playerQuestion + "\"}]}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        
        // CÁCH THIẾT LẬP HEADER KIỂU MỚI
        request.SetRequestHeader("Authorization", "Bearer " + cleanKey);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            GroqResponse response = JsonUtility.FromJson<GroqResponse>(request.downloadHandler.text);
            Debug.Log("<color=green>AI Trả lời:</color> " + response.choices[0].message.content);
        }
        else
        {
            // Xem mã lỗi 401 (Key), 429 (Quota), 400 (JSON lỗi)
            Debug.LogError("Mã phản hồi: " + request.responseCode);
            Debug.LogError("Phản hồi lỗi: " + request.downloadHandler.text);
        }
    }
}