using UnityEngine;

public class User
{
    public string name;
    public string mssv;
    public string nganhHoc;
    public string tuoi;

    public User(string name, string mssv, string nganhHoc, string tuoi)
    {
        this.name = name;
        this.mssv = mssv;
        this.nganhHoc = nganhHoc;
        this.tuoi = tuoi;
    }
}