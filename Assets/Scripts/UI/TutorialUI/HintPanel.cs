using UnityEngine;

public class HintPanel : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            this.gameObject.SetActive(false);
        }
    }
}
