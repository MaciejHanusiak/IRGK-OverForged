using UnityEngine;

public class SmithObject : MonoBehaviour
{
    [SerializeField] private SmithObjectSO smithObjectSO;

    private ISmithObjectParent smithObjectParent;
    public SmithObjectSO GetSmithObjectSO()
    {
        return smithObjectSO;
    }

    public void SetSmithObjectParent(ISmithObjectParent smithObjectParent)
    {
        this.smithObjectParent?.ClearSmithObject();

        this.smithObjectParent = smithObjectParent;

        if (smithObjectParent.HasSmithObject())
        {
            Debug.LogError("IKitchenObjectParent already has a SmithObject!");
        }
        smithObjectParent.SetSmithObject(this);
        transform.parent = smithObjectParent.GetSmithObjectFollowTransform();
        transform.localPosition = Vector2.zero;
    }

    public ISmithObjectParent GetSmithObjectParent()
    {
        return smithObjectParent;
    }
}
