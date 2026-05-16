using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    [Header("Portal Settings")]
    public GameObject portalPrefab; // Taruh prefab portal di sini
    public float spawnDistance = 2f; // Jarak portal di depan player
    public KeyCode spawnKey = KeyCode.E; // Tombol untuk memunculkan portal

    private GameObject currentPortal;

    void Update()
    {
        // Mendeteksi input tombol
        if (Input.GetKeyDown(spawnKey))
        {
            SpawnPortal();
        }
    }

    void SpawnPortal()
    {
        // Jika portal sudah ada, hancurkan yang lama sebelum membuat yang baru
        if (currentPortal != null)
        {
            Destroy(currentPortal);
        }

        // Menghitung posisi di depan player berdasarkan arah hadap (menggunakan localScale atau metode flip Anda)
        // Asumsi standar: Player menghadap kanan jika scale.x positif
        float direction = Mathf.Sign(transform.localScale.x);
        Vector3 spawnPosition = transform.position + new Vector3(direction * spawnDistance, 0f, 0f);

        // Memunculkan portal
        currentPortal = Instantiate(portalPrefab, spawnPosition, Quaternion.identity);
    }
}