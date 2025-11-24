using UnityEngine;

public class LobbyCameraRotate : MonoBehaviour
{
    public float rotationRange = 30f; // Sağ-sol ne kadar açıyla dönsün
    public float speed = 1f;          // Ne kadar hızlı dönsün

    private float startY;

    void Start()
    {
        startY = transform.eulerAngles.y;
    }

    void Update()
    {
        // PingPong: 0 ile rotationRange arasında ileri-geri gider
        float offset = Mathf.PingPong(Time.time * speed, rotationRange);

        // -half ile +half arasında osilasyon
        float angle = startY + (offset - rotationRange / 2f);

        // Kamerayı o açıya getir
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
