using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject visualGameObject;
    
    private Player player;


    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    public void Bind(Player player)
    {
        // odpinamy starego (jeœli by³)
        if (this.player != null)
            this.player.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;

        this.player = player;

        if (this.player != null)
            this.player.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;

        Hide();
    }
    private void OnDestroy()
    {
        if (player != null)
            player.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;
    }

    private void Show()
    {
        visualGameObject.SetActive(true);
    }

    private void Hide()
    {
        visualGameObject.SetActive(false);
    }
}
