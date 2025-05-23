using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        

            if (player.HasSmithObject())
            {
            // Player is carrying something
            player.GetSmithObject().DestroySelf();
            }
            else
            {
                // Player is not carrying anything
            }
        
        
    }
}
