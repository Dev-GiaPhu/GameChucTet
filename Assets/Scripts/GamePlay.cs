using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GamePlay : MonoBehaviour
{
    [Header("BlackOut")]
    public GameObject blackImage;
    public Vector3 endPos;
    public float speed = 0.1f;

    [Header("BeXuan")]
    public GameObject beXuan;
    public Vector3 beXuanEndPos;
    private Vector3 beXuanStartPos;
    public float beXuanSpeed = 0.1f;

    [Header("Frame Text Be Xuan")]
    public RectTransform frameTextBeXuan;
    private float frameStartRight;
    public float speedFrame;

    [Header("Text Talking")]
    public TextMeshProUGUI textTalking;

    [Header("Text Skip")]
    public GameObject textSkip;

    [Header("SelectCode")]
    public bool readySelect = false;

    [Header("ChatNPC")]
    public List<GameObject> chatNpcs;
    public bool openChating = false;


    [Header("Internet")]
    public bool connected = true;

    // ===== Dialogue =====
    private string[] dialogues =
    {
        "Chào bạn, mình là Bé Xuân - tinh linh ngày tết.",
        "Bạn hãy đi chúc những người thân của mình, để có một cái tết thật hạnh phúc với những lời chúc đi nào!!"
    };
    private int dialogueIndex = 0;

    // ===== State =====
    private bool isTalking = false;
    private bool skip = false;
    private bool isFrameMoving = false;
    private bool dialogueFinished = false;

    void Start()
    {
        
        dialogueFinished = false;
        dialogueIndex = 0;
        beXuanStartPos = beXuan.transform.position;
        frameStartRight = frameTextBeXuan.offsetMax.x;

        // reset UI
        textTalking.text = "";
        textSkip.SetActive(false);

        StartCoroutine(blackOut());
        StartCoroutine(FrameTextBeXuanIn());
    }

    void Update()
    {
        if((openChating == true && Input.GetKeyDown(KeyCode.Escape)) || openChating == true && connected == false)//ESC
        {
            foreach (GameObject chatNpc in chatNpcs)
            {
                chatNpc.GetComponent<AI_NPC>().textSpeak.text = "";
                chatNpc.GetComponent<AI_NPC>().isChatting = false;
                chatNpc.SetActive(false);
                connected = true;
            }
            openChating = false;
            readySelect = true;
        }
        if (!Input.GetMouseButtonDown(0)) return;
        if (isFrameMoving) return;

        if (isTalking)
        {
            skip = true;
        }
        else
        {
            NextDialogue();
        }
    }

    // ================= BLACK OUT =================
    IEnumerator blackOut()
    {
        blackImage.SetActive(true);
        yield return new WaitForSeconds(1f);

        while (blackImage.transform.position.y < endPos.y)
        {
            blackImage.transform.position += Vector3.up * speed;
            yield return new WaitForSeconds(0.01f);
        }

        blackImage.SetActive(false);
        StartCoroutine(beXuanIn());
    }

    // ================= BÉ XUÂN =================
    IEnumerator beXuanIn()
    {
        yield return new WaitForSeconds(0.5f);

        while (Vector3.Distance(beXuan.transform.position, beXuanEndPos) > 0.01f)
        {
            beXuan.transform.position = Vector3.MoveTowards(
                beXuan.transform.position,
                beXuanEndPos,
                beXuanSpeed * Time.deltaTime * 100
            );
            yield return null;
        }
    }

    // ================= FRAME =================
    IEnumerator FrameTextBeXuanIn()
    {
        isFrameMoving = true;
        yield return new WaitForSeconds(3f);

        float targetRight = 1293f;
        float speed = speedFrame * 100;

        while (Mathf.Abs(frameTextBeXuan.offsetMax.x - targetRight) > 0.5f)
        {
            Vector2 offset = frameTextBeXuan.offsetMax;
            offset.x = Mathf.MoveTowards(offset.x, targetRight, speed * Time.deltaTime);
            frameTextBeXuan.offsetMax = offset;
            yield return null;
        }

        Vector2 finalOffset = frameTextBeXuan.offsetMax;
        finalOffset.x = targetRight;
        frameTextBeXuan.offsetMax = finalOffset;

        yield return new WaitForSeconds(0.5f);
        textSkip.SetActive(true);

        isFrameMoving = false;

        dialogueIndex = 0;
        StartCoroutine(TypeText(dialogues[dialogueIndex]));
    }

    // ================= TYPE TEXT =================
    IEnumerator TypeText(string fullText)
    {
        isTalking = true;
        skip = false;
        textTalking.text = "";

        foreach (char c in fullText)
        {
            if (skip)
            {
                textTalking.text = fullText;
                isTalking = false;
                yield break;
            }

            textTalking.text += c;
            yield return new WaitForSeconds(0.04f);
        }

        isTalking = false;
    }

    // ================= NEXT / END =================
    void NextDialogue()
    {
        if (dialogueFinished) return;

        dialogueIndex++;

        if (dialogueIndex >= dialogues.Length)
        {
            dialogueFinished = true;
            EndDialogue();
            return;
        }

        StartCoroutine(TypeText(dialogues[dialogueIndex]));
    }

    void EndDialogue()
    {
        // reset text
        textTalking.text = "";
        textSkip.SetActive(false);

        // reset frame
        Vector2 offset = frameTextBeXuan.offsetMax;
        offset.x = frameStartRight;
        frameTextBeXuan.offsetMax = offset;

        // reset Bé Xuân
        beXuan.transform.position = beXuanStartPos;

        // reset state
        isTalking = false;
        skip = false;
        isFrameMoving = false;
        readySelect = true;


        Debug.Log("Dialogue reset hoàn tất");
    }

    public void OpenChatNPC(int idNPC)
    {
        idNPC -= 1;
        readySelect = false;
        openChating = true;
        chatNpcs[idNPC].SetActive(true);
        chatNpcs[idNPC].GetComponent<AI_NPC>().OpenChat();
    }
}
