using UnityEngine;

public class Passenger : MonoBehaviour
{
    public void PickUp()
    {
        Destroy(this.gameObject);
        Debug.Log("Passenger picked up: " + this.gameObject.name);
    }
}