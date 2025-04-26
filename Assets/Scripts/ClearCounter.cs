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
            // Create new smith object if none exist
            Transform smithObjectTransform = Instantiate(smithObjectSO.prefab, counterTopPoint);
            smithObjectTransform.localPosition = Vector2.zero;
            smithObject = smithObjectTransform.GetComponent<SmithObject>(); // Set Smith Object to Clear Counter
            smithObject.SetClearCounter(this); // Set Clear Counter to Smith Object.
        }
        else
        {
            Debug.Log(smithObject.GetClearCounter());
        }
        
    }
}
