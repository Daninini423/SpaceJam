using UnityEngine;
using UnityEngine.InputSystem; // Menggunakan Input System baru Unity 6
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private float moveInput;
    private bool isFacingRight = true;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("Time Ghost Mechanic")]
    [SerializeField] private GameObject ghostPrefab; // Tarik Prefab Bayangan Anda ke sini di Inspector
    private List<ActiveData> currentRunData = new List<ActiveData>();
    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Catat posisi awal sebagai tempat checkpoint/reset
        startPosition = transform.position;
    }

    void Update()
    {
        moveInput = 0f;

        // Membaca input menggunakan Input System Baru (Keyboard)
        if (Keyboard.current != null)
        {
            // Input Horizontal (A/D atau Panah Kiri/Kanan)
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput = 1f;

            // Input Lompat (Spasi)
            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }

            // TOMBOL RESET / TIME REWIND (Tekan R)
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                ResetTimeAndSpawnGhost();
            }
        }

        // Mengatur arah hadap sprite karakter (Flip)
        Flip();
    }

    private void FixedUpdate()
    {
        // Ground Check milik Anda
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Pergerakan fisik dipindah ke FixedUpdate agar sinkron dengan perekaman data fisik
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // REKAM posisi dan arah hadap Anda di setiap frame fisik
        currentRunData.Add(new ActiveData(transform.position, isFacingRight));
    }

    private void ResetTimeAndSpawnGhost()
    {
        // Hanya buat bayangan jika player sempat bergerak (ada data yang direkam)
        if (currentRunData.Count > 0)
        {
            // Spawn bayangan di posisi awal
            GameObject newGhost = Instantiate(ghostPrefab, startPosition, Quaternion.identity);

            // Kirim rekaman perjalanan kita ke bayangan tersebut
            TimeGhost ghostScript = newGhost.GetComponent<TimeGhost>();
            if (ghostScript != null)
            {
                ghostScript.SetData(new List<ActiveData>(currentRunData));
            }

            // Reset data run saat ini agar siap merekam perjalanan yang baru
            currentRunData = new List<ActiveData>();
        }

        // Kembalikan Player Utama ke posisi awal
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;

        // Reset arah hadap player ke kanan semula
        isFacingRight = true;
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    private void Flip()
    {
        if ((isFacingRight && moveInput < 0f) || (!isFacingRight && moveInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}