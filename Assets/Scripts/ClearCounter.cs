using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private SmithObjectSO smithObjectSO;
    [SerializeField] private Transform counterTopPoint;
    public void Interact()
    {
        Debug.Log("Interact");  // Create ore on top of the counter
        Transform smithObjectTransform = Instantiate(smithObjectSO.prefab, counterTopPoint);
        smithObjectTransform.localPosition = Vector2.zero;
    }
}
