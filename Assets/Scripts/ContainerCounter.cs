using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private SmithObjectSO smithObjectSO;
   


    public override void Interact(Player player)
    {
        if (!HasSmithObject())
        {
            // Create new smith object if none exist
            Transform smithObjectTransform = Instantiate(smithObjectSO.prefab);
            smithObjectTransform.GetComponent<SmithObject>().SetSmithObjectParent(player);
            smithObjectTransform.localPosition = Vector2.zero;
        }
    }

   
}
