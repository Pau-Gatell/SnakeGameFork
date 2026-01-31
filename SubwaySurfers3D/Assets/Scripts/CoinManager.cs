using UnityEngine;
using TMPro;
using System.Collections;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public TextMeshProUGUI coinsText;

    private int coins = 0;

    private bool doubleCoinsActive = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        if (doubleCoinsActive)
            amount *= 2;

        coins += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        coinsText.text = "Coins: " + coins;
    }

    // ================= DOUBLE COINS =================

    public void ActivateDoubleCoins(float duration)
    {
        StartCoroutine(DoubleCoinsCoroutine(duration));
    }

    private IEnumerator DoubleCoinsCoroutine(float duration)
    {
        doubleCoinsActive = true;
        yield return new WaitForSeconds(duration);
        doubleCoinsActive = false;
    }
} 