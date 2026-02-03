using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInput;
    public TMP_InputField mssvInput;
    public TMP_Dropdown tuoiInput;
    public TMP_Dropdown nganhInput;
    
    [Header("Settings")]
    public GameObject nhapLaiPrefab; // Prefab thông báo lỗi
    public Transform canvasTransform;
    public string nextSceneName = "GamePlay";

    [Header("Prefab image changes scene")]
    public GameObject blackImage;


    IEnumerator ImageBlackIn()
    {
        while (blackImage.transform.position.y != 540)
        {
            Debug.Log(blackImage.transform.position.y);
            var y = Mathf.MoveTowards(blackImage.transform.position.y, 540, 500 * Time.deltaTime);
            blackImage.transform.position = new Vector3(blackImage.transform.position.x, y, blackImage.transform.position.z);
            yield return new WaitForSeconds(0.01f);
        }
        blackImage.transform.position = new Vector3(blackImage.transform.position.y, 540, blackImage.transform.position.z);//
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nextSceneName);
    }
    public void OnClickStart()
    {
        // Kiểm tra điều kiện: Tên trống OR MSSV trống OR chưa chọn Dropdown
        if (string.IsNullOrWhiteSpace(nameInput.text) || 
            string.IsNullOrWhiteSpace(mssvInput.text) || 
            tuoiInput.value == 0 || 
            nganhInput.value == 0)
        {
            // Nếu thiếu thông tin: Tạo thông báo lỗi (Prefab)
            if (nhapLaiPrefab != null)
            {
                GameObject warning = Instantiate(nhapLaiPrefab, canvasTransform);
                // Prefab này thường có script tự hủy sau x giây như file VuiLongNhapDDTT.cs bạn gửi
            }
            Debug.Log("Vui lòng nhập đầy đủ thông tin!");
        }
        else
        {
            // Nếu đủ thông tin: Lưu vào UserList và chuyển Scene
            var user = new User(
                nameInput.text,
                mssvInput.text,
                tuoiInput.options[tuoiInput.value].text,
                nganhInput.options[nganhInput.value].text
            );

            if (UserList.Instance != null)
            {
                UserList.Instance.UserAdd(user); // Hàm này trong UserList đã có SceneManager.LoadScene rồi
                Debug.Log("UserList.Instance is null");
                StartCoroutine(ImageBlackIn());
            }
            else
            {
                Debug.Log("UserList.Instance is null");
                StartCoroutine(ImageBlackIn());
            }
        }
    }
}