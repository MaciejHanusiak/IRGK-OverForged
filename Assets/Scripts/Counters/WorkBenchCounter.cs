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
            if (player.HasSmithObject())
            {
                if (player.GetSmithObject().TryGetWeaponStand(out WeaponStandSmithObject weaponStandSmithObject))
                {
                    player.GetSmithObject().SetSmithObjectParent(this);
                }
            }
        }
        else
        {
            // There is smith object

            if (player.HasSmithObject())
            {
                // Player carrying an object
               if (GetSmithObject().TryGetWeaponStand(out WeaponStandSmithObject weaponStandSmithObject))
               {
                    if (weaponStandSmithObject.TryAddWeaponPart(player.GetSmithObject().GetSmithObjectSO()))
                    {
                        player.GetSmithObject().DestroySelf();
                    }
               }
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
        if (!HasSmithObject())
        {
            // There is no smith object, player has or hasn't something.
            SmithObject.SpawnSmithObject(weaponStandObjectSO, this);

            Analytics.Instance.PlayerCreateWeaponStand(LevelStats.Instance.gold, LevelTime.Instance.timeRemaining);
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
                
            }
        }
    }
}
