using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
            }
            else
            {
                // Phòng trường hợp UserList chưa khởi tạo
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}