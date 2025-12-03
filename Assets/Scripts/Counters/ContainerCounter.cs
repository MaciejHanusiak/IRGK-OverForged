using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] public SmithObjectSO smithObjectSO;
    private int priceForMaterial = 0;


    public override void Interact(Player player)
    {
        if (!player.HasSmithObject())
        {
            // Player is not carrying anything
            switch (smithObjectSO.name)
            {
                case "WoodenLog":
                    priceForMaterial = 1;
                    break;
                case "CopperOre":
                    priceForMaterial = 2;
                    break;
                case "IronOre":
                    priceForMaterial = 3;
                    break;
            }

            if (LevelStats.Instance.SpendGold(priceForMaterial))
            {
                SmithObject.SpawnSmithObject(smithObjectSO, player);
                Analytics.Instance.PlayerBuySomething(smithObjectSO.name, LevelStats.Instance.gold, LevelTime.Instance.timeRemaining);

            }

        }
        else
        {
            // Player already has an smith object
        }
    }

    public override SmithObjectSO GetOutputSmithObjectSO() => smithObjectSO;

}
