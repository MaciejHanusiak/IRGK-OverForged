using UnityEngine;

[CreateAssetMenu(fileName = "CuttingRecipe", menuName = "ScriptableObjects/CuttingRecipe")]
public class CuttingRecipeSO : ScriptableObject
{
    public SmithObjectSO input;
    public SmithObjectSO output;
    public float cuttingTimerMax;
}




