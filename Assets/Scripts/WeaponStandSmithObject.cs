using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStandSmithObject : SmithObject
{

    [SerializeField] private List<SmithObjectSO> validSmithObjectSOList;
    private List<SmithObjectSO> smithObjectSOList;

    public void Awake()
    {
        smithObjectSOList = new List<SmithObjectSO>();
    }

    public bool TryAddWeaponPart(SmithObjectSO smithObjectSO)
    {
        if (!validSmithObjectSOList.Contains(smithObjectSO))
        {
            // Not valid weapon parts
            return false;
        }
        if (smithObjectSOList.Contains(smithObjectSO))
        {
            // Weapon stand already has this object
            return false;
        }
        else
        {
            smithObjectSOList.Add(smithObjectSO);
            return true;
        }
    }

    public List<SmithObjectSO> GetSmithObjectSOList()
    {
        return smithObjectSOList;
    }
}
