using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VuiLongNhapDDTT : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Des());
    }

    IEnumerator Des()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    
}
