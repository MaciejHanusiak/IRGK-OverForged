using UnityEngine;

public class ClearCounter : MonoBehaviour, ISmithObjectParent
{
    [SerializeField] private SmithObjectSO smithObjectSO;
    [SerializeField] private Transform counterTopPoint;


    private SmithObject smithObject;
 
    public void Interact(Player player)
    {

        if (smithObject == null)
        {
            // Create new smith object if none exist
            Transform smithObjectTransform = Instantiate(smithObjectSO.prefab, counterTopPoint);
            smithObjectTransform.GetComponent<SmithObject>().SetSmithObjectParent(this);
            smithObjectTransform.localPosition = Vector2.zero;
        }
        else
        {
            // Give the object to player
            smithObject.SetSmithObjectParent(player);
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
