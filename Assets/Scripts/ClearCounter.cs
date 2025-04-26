using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private SmithObjectSO smithObjectSO;
    [SerializeField] private Transform counterTopPoint;

    private SmithObject smithObject;
    public void Interact()
    {
        if (smithObject == null)
        {
            Transform smithObjectTransform = Instantiate(smithObjectSO.prefab, counterTopPoint);
            smithObjectTransform.localPosition = Vector2.zero;
            smithObject = smithObjectTransform.GetComponent<SmithObject>();
        }
        
    }
}
