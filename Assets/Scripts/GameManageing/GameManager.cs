using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    static bool isMultiplayerSelected = false;

    public void Update()
    {
        Debug.Log(isMultiplayerSelected);
    }

    public void SelectMultiPlayer()
    {
        isMultiplayerSelected = true;
    }
    public void UnselectMultiPlayer()
    {
        isMultiplayerSelected = false;
    }
}
