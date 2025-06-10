using UnityEngine;

[CreateAssetMenu(fileName = "OverCuttingRecipe", menuName = "ScriptableObjects/OverCuttingRecipe")]
public class OverCuttingRecipeSO : ScriptableObject
{
    public SmithObjectSO input;
    public SmithObjectSO output;
    public float overCuttingTimeMax;
}