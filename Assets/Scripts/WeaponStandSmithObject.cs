using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStandSmithObject : SmithObject
{
    public event EventHandler<OnWeaponPartAddedEventArgs> OnWeaponPartAdded;
    public class OnWeaponPartAddedEventArgs : EventArgs
    {
        public SmithObjectSO SmithObjectSO;
    }
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

            Analytics.Instance.PlayerAddWeaponPart(smithObjectSO.objectName, LevelStats.Instance.gold, LevelTime.Instance.timeRemaining);
            OnWeaponPartAdded?.Invoke(this, new OnWeaponPartAddedEventArgs()
            {
                SmithObjectSO = smithObjectSO
            });
            return true;
        }
    }

    public List<SmithObjectSO> GetSmithObjectSOList()
    {
        return smithObjectSOList;
    }

}
