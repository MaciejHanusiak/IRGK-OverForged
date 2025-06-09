using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private AnvilCounter anvilCounter;
    [SerializeField] private Image barImage;

    private void Start()
    {
        anvilCounter.OnProgressChanged += AnvilCounter_OnProgressChanged;
        barImage.fillAmount = 0f;

        Hide();
    }

    private void AnvilCounter_OnProgressChanged(object sender, AnvilCounter.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
