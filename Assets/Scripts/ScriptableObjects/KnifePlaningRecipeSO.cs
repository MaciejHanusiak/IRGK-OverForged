using UnityEngine;

[CreateAssetMenu(fileName = "KnifePlaningRecipe", menuName = "ScriptableObjects/KnifePlaningRecipe")]
public class KnifePlaningRecipeSO : ScriptableObject
{
    public SmithObjectSO input;
    public SmithObjectSO output;
    public float knifePlaningProgressMax;
}
