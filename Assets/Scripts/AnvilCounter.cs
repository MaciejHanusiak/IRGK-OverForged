using UnityEngine;

public class AnvilCounter : BaseCounter
{
    [SerializeField] private SmithObjectSO forgedSmithObjectSO;
    public override void Interact(Player player)
    {
        if (!HasSmithObject())
        {
            // There is no smith object

            if (player.HasSmithObject())
            {
                // Player is carrying something
                player.GetSmithObject().SetSmithObjectParent(this);
            }
            else
            {
                // Player is not carrying anything
            }
        }
        else
        {
            // There is smith object

            if (player.HasSmithObject())
            {
                // Player carrying an object
            }
            else
            {
                // Player is no carrying anything
                this.GetSmithObject().SetSmithObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasSmithObject())
        {
            // There is a smith object
            GetSmithObject().DestroySelf();
            SmithObject.SpawnSmithObject(forgedSmithObjectSO, this);
        }


    }
}
