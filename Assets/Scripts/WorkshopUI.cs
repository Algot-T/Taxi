using UnityEngine;

public class WorkshopUI : MonoBehaviour
{
    public PlayerController player;

    [Header("Upgrade Costs")]
    public int repairCost = 20;
    public int hpCost = 40;
    public int speedCost = 40;
    public int rewardCost = 50;

    public void RepairTaxi()
    {
        if (player.money >= repairCost)
        {
            player.money -= repairCost;
            player.HealFull();
            MoneyUI.Instance.UpdateMoney(player.money);
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    public void UpgradeHP()
    {
        if (player.money >= hpCost)
        {
            player.money -= hpCost;
            player.UpgradeHP();
            MoneyUI.Instance.UpdateMoney(player.money);
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    public void UpgradeSpeed()
    {
        if (player.money >= speedCost)
        {
            player.money -= speedCost;
            player.UpgradeSpeed();
            MoneyUI.Instance.UpdateMoney(player.money);
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    public void UpgradeReward()
    {
        if (player.money >= rewardCost)
        {
            player.money -= rewardCost;
            player.UpgradePassengerReward();
            MoneyUI.Instance.UpdateMoney(player.money);
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    public void CloseWorkshop()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}