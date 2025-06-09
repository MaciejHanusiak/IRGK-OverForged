using UnityEngine;

[CreateAssetMenu(fileName = "AnvilForgeingRecipe", menuName = "ScriptableObjects/AnvilForgeingRecipe")]
public class AnvilForgeingRecipeSO : ScriptableObject
{
    public SmithObjectSO input;
    public SmithObjectSO output;
    public float anvilForgeingProgressMax;
}
