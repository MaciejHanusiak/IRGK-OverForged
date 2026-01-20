using UnityEngine;

public class BaseCounter : MonoBehaviour, ISmithObjectParent
{
    [SerializeField] private Transform counterTopPoint;


    protected SmithObject smithObject;
    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }
    public virtual void Interact(PlayerOne player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }
    public virtual void InteractAlternate(Player player)
    {
        Debug.LogError("BaseCounter.InteractAlternate();");
    }
    public virtual void InteractAlternate(PlayerOne player)
    {
        Debug.LogError("BaseCounter.InteractAlternate();");
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
    public string GetObjectParentName()
    {
        return this.name;
    }
    public virtual SmithObjectSO GetOutputSmithObjectSO() => null;
    
}
