using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public Transform canvas;
    public GameObject nameOJ;
    public GameObject mssvOJ;
    public GameObject tuoiOJ;
    public GameObject nganhOJ;

    public TMP_InputField nameInput => nameOJ.GetComponent<TMP_InputField>();
    public TMP_InputField mssvInput => mssvOJ.GetComponent<TMP_InputField>();
    public TMP_Dropdown tuoiInput => tuoiOJ.GetComponent<TMP_Dropdown>();
    public TMP_Dropdown nganhInput => nganhOJ.GetComponent<TMP_Dropdown>();

    public GameObject nhapLaiPrefab;

    public void startGame()
    {
        if (string.IsNullOrWhiteSpace(nameInput.text) ||
            string.IsNullOrWhiteSpace(mssvInput.text) ||
            tuoiInput.value == 0 ||
            nganhInput.value == 0)
        {
            var text = Instantiate(nhapLaiPrefab, canvas);
            text.transform.SetParent(canvas, false);
            return;
        }

        var user = new User(
            nameInput.text,
            mssvInput.text,
            tuoiInput.options[tuoiInput.value].text,
            nganhInput.options[nganhInput.value].text
        );

        UserList.Instance.UserAdd(user);
    }
}
