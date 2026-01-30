using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class UserList : MonoBehaviour
{
    public static UserList Instance;

    public List<User> Users = new List<User>();

    string filePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            filePath = Path.Combine(
                Application.persistentDataPath,
                "users.txt"
            );
            Debug.Log(filePath);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UserAdd(User user)
    {
        Debug.Log("User add");
        Users.Add(user);
        SaveToFile(user);
        SceneManager.LoadScene("GamePlay");
    }

    // 🔹 GHI CHUNG FILE
    void SaveToFile(User user)
    {
        string line =
            $"{user.name}|{user.mssv}|{user.tuoi}|{user.nganhHoc}\n";

        File.AppendAllText(filePath, line, Encoding.UTF8);
    }
}
