using UnityEngine;

public class Destination : MonoBehaviour
{
    public int reward = 100;
    private bool reached = false;

    public void Reach()
    {
        if (reached) return;
        reached = true;

        Debug.Log("Destination reached! Reward: " + reward + " coins.");
        Destroy(this.gameObject);
    }
}