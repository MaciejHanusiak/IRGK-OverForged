using UnityEngine;

[CreateAssetMenu(fileName = "ForgeingRecipe", menuName = "ScriptableObjects/ForgeingRecipe")]
public class ForgeingRecipeSO : ScriptableObject
{
    public SmithObjectSO input;
    public SmithObjectSO output;
    public float forgeingTimerMax;
}
