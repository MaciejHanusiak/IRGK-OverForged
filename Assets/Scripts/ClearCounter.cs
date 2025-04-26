using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private SmithObjectSO smithObjectSO;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private ClearCounter secondClearCounter;

    public bool testing;

    private SmithObject smithObject;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && testing)
        {
            if (smithObject != null)
            {
                smithObject.SetClearCounter(secondClearCounter);
            }
            else
            {
                Debug.LogError("Clear counter does't has any object!");
            }
        }
    }
    public void Interact()
    {

        if (smithObject == null)
        {
            // Create new smith object if none exist
            Transform smithObjectTransform = Instantiate(smithObjectSO.prefab, counterTopPoint);
            smithObjectTransform.GetComponent<SmithObject>().SetClearCounter(this);
            smithObjectTransform.localPosition = Vector2.zero;
        }
        else
        {
            Debug.LogError(smithObject.GetClearCounter() + " already has a object on it!");
        }
        
    }
    public Transform GetSmithObjectFollowTransform()
    {
        return counterTopPoint;
    }
    public void SetSmithObject(SmithObject smithObject)
    {
        this.smithObject = smithObject;
    }
    public SmithObject GetSmithObject()
    {
        return smithObject;
    }
    public void ClearSmithObject()
    {
        smithObject = null;
    }
    public bool HasSmithObject()
    {
        return smithObject != null;
    }
}
