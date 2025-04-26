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
        this.clearCounter = clearCounter;
    }

    public ClearCounter GetClearCounter()
    {
        return clearCounter;
    }
}
