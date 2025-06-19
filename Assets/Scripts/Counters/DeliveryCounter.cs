using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    [SerializeField] private BaseCounter baseCounterForFinishedWeapons;
    [SerializeField] private Transform counterTopPoint3;
    public override void Interact(Player player)
    {
        if (player.HasSmithObject())
        {
            if (player.GetSmithObject().TryGetWeaponStand(out WeaponStandSmithObject weaponStandSmithObject))
            {
                DeliveryManager.Instance.DeliverRecipe(weaponStandSmithObject);

                player.GetSmithObject().SetSmithObjectParent(baseCounterForFinishedWeapons);
                baseCounterForFinishedWeapons.GetSmithObject().gameObject.SetActive(false);

                
            }
        }
    }
}
