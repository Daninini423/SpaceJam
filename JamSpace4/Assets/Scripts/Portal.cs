using UnityEngine;
using UnityEngine.SceneManagement; // Wajib diimport untuk pindah scene

public class Portal : MonoBehaviour
{
    [Header("Level Settings")]
    [Tooltip("Nama scene tujuan. Pastikan nama sesuai dan sudah dimasukkan ke Build Settings!")]
    public string nextSceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Memastikan yang menyentuh portal adalah Player
        if (collision.CompareTag("Player"))
        {
            TeleportToNextStage();
        }
    }

    void TeleportToNextStage()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Nama scene berikutnya belum diisi di Inspector Portal!");
        }
    }
}