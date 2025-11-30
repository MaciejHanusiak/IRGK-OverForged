using UnityEngine;
//using static Player;

public class SelectedUI : MonoBehaviour
{
    [SerializeField] private Transform selectedPanel;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        // Tutaj reagujemy!
        if (e.selectedCounter != null)
        {
            Debug.Log(e.selectedCounter.ToString());
            selectedPanel.gameObject.SetActive(true);
        }
        else
        {
            selectedPanel.gameObject.SetActive(false);
        }
    }

}
