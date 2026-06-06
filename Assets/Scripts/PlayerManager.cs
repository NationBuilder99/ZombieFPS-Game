using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public float health = 100f;
    public Text healthText;

    public GameManager gameManager;

    void Start()
    {
        UpdateHealthUI();
    }

    public void Hit(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, 100);

        UpdateHealthUI();

        if (health <= 0)
        {
           gameManager.EndGame();
        }
    }

    void UpdateHealthUI()
    {
        healthText.text = "Health: " + health.ToString();
    }
}
