using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    [Header("Float Settings")]
    public float amplitudeY = 10f;   // độ cao lên xuống (trục Y)
    public float speed = 2f;          // tốc độ dao động

    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * speed) * amplitudeY;
        transform.localPosition = startPos + new Vector3(0, offsetY, 0);
    }
}
