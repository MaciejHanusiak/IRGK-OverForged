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
    public bool isMultiplayerSelected = false;

    public void Update()
    {
        
    }

    public void SelectMultiPlayer()
    {
        isMultiplayerSelected = true;
        Debug.Log(isMultiplayerSelected);
    }
    public void UnselectMultiPlayer()
    {
        isMultiplayerSelected = false;
        Debug.Log(isMultiplayerSelected);
    }
}
