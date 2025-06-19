using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe")]
public class RecipeSO : ScriptableObject
{
    public List<SmithObjectSO> smithObjectSOList;
    public string recipeName;
}
