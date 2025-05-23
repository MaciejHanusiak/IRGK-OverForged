using UnityEngine;

[CreateAssetMenu(fileName = "BurningRecipe", menuName = "ScriptableObjects/BurningRecipe")]
public class BurningRecipeSO : ScriptableObject
{
    public SmithObjectSO input;
    public SmithObjectSO output;
    public float burningTimerMax;
}
