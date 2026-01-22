using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void SelectMulti()
    {
        GameManager.Instance.SelectMultiPlayer();
    }

    public void SelectSingle()
    {
        GameManager.Instance.UnselectMultiPlayer();
    }
}
