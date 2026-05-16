using UnityEngine;
using System.Collections.Generic;

public class TimeGhost : MonoBehaviour
{
    private List<ActiveData> ghostData;
    private int currentFrame = 0;
    private SpriteRenderer sr;
    private Rigidbody2D rb; // Tambahan untuk pergerakan fisik yang mulus

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>(); // Ambil komponen Rigidbody2D

        if (sr != null)
        {
            Color color = sr.color;
            color.a = 0.5f;
            sr.color = color;
        }
    }

    public void SetData(List<ActiveData> dataToReplay)
    {
        ghostData = dataToReplay;
        currentFrame = 0;
    }

    void FixedUpdate()
    {
        if (ghostData != null && currentFrame < ghostData.Count)
        {
            // MENGGUNAKAN MovePosition: Jauh lebih mulus di Unity 6 untuk objek Kinematic 
            // yang memiliki Collider dan akan diinjak oleh objek lain.
            rb.MovePosition(ghostData[currentFrame].position);

            // Atur arah hadap (Flip) mengikuti rekaman
            Vector3 localScale = transform.localScale;
            if (ghostData[currentFrame].isFacingRight)
            {
                localScale.x = Mathf.Abs(localScale.x);
            }
            else
            {
                localScale.x = -Mathf.Abs(localScale.x);
            }
            transform.localScale = localScale;

            currentFrame++;
        }
    }
}