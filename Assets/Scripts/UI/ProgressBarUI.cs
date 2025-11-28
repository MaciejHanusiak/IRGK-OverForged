using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    [SerializeField] private Image redBarImage;
    private IHasProgress hasProgress;


    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError("GameObject" + hasProgressGameObject + " does not have a component that implements IHasProgress!");
        }
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        barImage.fillAmount = 0f;

        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        if (e.forgeState == ForgeCounter.State.Forged || e.sawState == SawCounter.State.Cutted)
        {
            redBarImage.gameObject.SetActive(true);
            redBarImage.fillAmount = e.progressNormalized;

        }
        else
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
