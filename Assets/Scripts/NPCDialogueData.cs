using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Data", menuName = "NPC/Dialogue Data")]
public class NPCDialogueData : ScriptableObject {
    public int npcID;
    [TextArea(3, 10)]
    public string greetingText;
    public Color nameColor = Color.white;
}