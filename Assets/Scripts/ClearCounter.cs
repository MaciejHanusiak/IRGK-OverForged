using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private Transform orePrefab;
    [SerializeField] private Transform counterTopPoint;
    public void Interact()
    {
        Debug.Log("Interact");
        Instantiate(orePrefab, counterTopPoint);
    }
}
