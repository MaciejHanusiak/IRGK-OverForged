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
            if (smithObjectParent != null && smithObjectParent.GetObjectParentName() != "FinishedWeapons")
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
    public void DestroySelf()
    {
        smithObjectParent.ClearSmithObject();
        Destroy(gameObject);
    }
    public static SmithObject SpawnSmithObject(SmithObjectSO smithObjectSO, ISmithObjectParent smithObjectParent)
    {
        Transform smithObjectTransform = Instantiate(smithObjectSO.prefab); // Create new smith object
        SmithObject smithObject = smithObjectTransform.GetComponent<SmithObject>();

        smithObject.SetSmithObjectParent(smithObjectParent); // Set this object to transform to parent

        return smithObject;
    }

    public bool TryGetWeaponStand(out WeaponStandSmithObject weaponStandSmithObject)
    {
        if (this is WeaponStandSmithObject)
        {
            weaponStandSmithObject = this as WeaponStandSmithObject;
            return true;
        }
        else
        {
            weaponStandSmithObject = null;
            return false;
        }
    }
}
