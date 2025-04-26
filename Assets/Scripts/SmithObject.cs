using UnityEngine;

public class SmithObject : MonoBehaviour
{
    [SerializeField] private SmithObjectSO smithObjectSO;

    public SmithObjectSO GetSmithObjectSO()
    {
        return smithObjectSO;
    }
}
