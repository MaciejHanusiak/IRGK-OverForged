using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private SmithObjectSO smithObjectSO;
   


    public override void Interact(Player player)
    {
        if (!player.HasSmithObject())
        {
            // Player is not carrying anything
            Transform smithObjectTransform = Instantiate(smithObjectSO.prefab);
            smithObjectTransform.GetComponent<SmithObject>().SetSmithObjectParent(player);           
        }
        else
        {
            // Player already has an smith object
        }
    }

   
}
