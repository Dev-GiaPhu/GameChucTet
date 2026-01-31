using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public TMP_InputField nameInput, mssvInput;
    public TMP_Dropdown tuoiInput, nganhInput;
    public GameObject nhapLaiPrefab;
    public Transform canvas;

    public void startGame() {
        if (string.IsNullOrWhiteSpace(nameInput.text) || string.IsNullOrWhiteSpace(mssvInput.text) || tuoiInput.value == 0) {
            Instantiate(nhapLaiPrefab, canvas);
            return;
        }

        User newUser = new User(
            nameInput.text, 
            mssvInput.text, 
            nganhInput.options[nganhInput.value].text, 
            tuoiInput.options[tuoiInput.value].text
        );

        UserList.Instance.UserAdd(newUser);
    }
}