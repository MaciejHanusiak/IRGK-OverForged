using UnityEngine;

public interface ISmithObjectParent 
{

    public Transform GetSmithObjectFollowTransform();
    public void SetSmithObject(SmithObject smithObject);
    public SmithObject GetSmithObject();
    public void ClearSmithObject();
    public bool HasSmithObject();
}
