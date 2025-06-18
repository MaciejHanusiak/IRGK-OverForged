using System;
using UnityEngine;

public class WorkBenchCounter : BaseCounter
{
    [SerializeField] private SmithObjectSO weaponStandObjectSO;
    public override void Interact(Player player)
    {
        if (!HasSmithObject())
        {
            // There is no smith object, player has or hasn't something.
            SmithObject.SpawnSmithObject(weaponStandObjectSO, this);
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
                player.GetSmithObject().SetSmithObjectParent(this);
            }
        }
    }
}
