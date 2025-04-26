using UnityEngine;

public class SmithObject : MonoBehaviour
{
    [SerializeField] private SmithObjectSO smithObjectSO;

    private ClearCounter clearCounter;
    public SmithObjectSO GetSmithObjectSO()
    {
        return smithObjectSO;
    }

    public void SetClearCounter(ClearCounter clearCounter)
    {
        if (this.clearCounter != null)
        {
            this.clearCounter.ClearSmithObject();
        }
        this.clearCounter = clearCounter;

        if (clearCounter.HasSmithObject())
        {
            Debug.LogError("Counter already has a SmithObject!");
        }
        clearCounter.SetSmithObject(this);
        transform.parent = clearCounter.GetSmithObjectFollowTransform();
        transform.localPosition = Vector2.zero;
    }

    public ClearCounter GetClearCounter()
    {
        return clearCounter;
    }
}
