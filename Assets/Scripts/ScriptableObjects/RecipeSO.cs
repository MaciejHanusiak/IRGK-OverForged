using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe")]
public class RecipeSO : ScriptableObject
{
    public List<SmithObjectSO> smithObjectSOList;
    public string recipeName;
    public float recipeTime = 30f; // Domyœlnie 30 sekund
    public int recipePrice;
    //public Sprite iconSprite;
    
    


}
