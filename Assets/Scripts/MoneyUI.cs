using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public static MoneyUI Instance;

    public TextMeshProUGUI moneyText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateMoney(int amount)
    {
        moneyText.text = "Money: " + amount;
    }
}