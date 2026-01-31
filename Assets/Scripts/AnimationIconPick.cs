using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float speed = 2f;
    Vector3 startPos;

    void Start() => startPos = transform.localPosition;
    void Update() => transform.localPosition = startPos + new Vector3(0, Mathf.Sin(Time.time * speed) * amplitude, 0);
}