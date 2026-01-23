using UnityEngine;
using System;

public class BaseCounter : MonoBehaviour, ISmithObjectParent
{
    [SerializeField] private Transform counterTopPoint;


    protected SmithObject smithObject;
    // Fired whenever counter state changes in a way that UI may need to refresh
    // (e.g. smithObject set/cleared, weapon stand spawned/removed, etc.)
    public event EventHandler OnStateChanged;

    protected void NotifyStateChanged()
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }
    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    } public virtual void InteractAlternate(Player player)
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
        NotifyStateChanged();
    }
    public SmithObject GetSmithObject()
    {
        return smithObject;
    }
    public void ClearSmithObject()
    {
        smithObject = null;
        NotifyStateChanged();
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
