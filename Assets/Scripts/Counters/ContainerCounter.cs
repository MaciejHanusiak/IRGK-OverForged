using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private SmithObjectSO smithObjectSO;
    




    public override void Interact(Player player)
    {
        if (!player.HasSmithObject())
        {
            // Player is not carrying anything
            SmithObject.SpawnSmithObject(smithObjectSO, player);
            Debug.Log(smithObjectSO.name);
            
        }
        else
        {
            // Player already has an smith object
        }
    }

   
}
