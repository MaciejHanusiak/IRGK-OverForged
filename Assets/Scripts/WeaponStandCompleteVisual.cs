using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;


public class WeaponStandCompleteVisual : MonoBehaviour
{
    [Serializable] public struct SmithObjectSO_GameObject
    {
        public SmithObjectSO smithObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private WeaponStandSmithObject weaponStandSmithObject;
    [SerializeField] private List<SmithObjectSO_GameObject> smithObjectSO_GameObjectList;
    

    private void Start()
    {
        weaponStandSmithObject.OnWeaponPartAdded += WeaponStandSmithObject_OnWeaponPartAdded;

        foreach (SmithObjectSO_GameObject smithObjectSO_GameObject in smithObjectSO_GameObjectList)
        {

            smithObjectSO_GameObject.gameObject.SetActive(false);
            
        }
    }

    private void WeaponStandSmithObject_OnWeaponPartAdded(object sender, WeaponStandSmithObject.OnWeaponPartAddedEventArgs e)
    {
        foreach (SmithObjectSO_GameObject smithObjectSO_GameObject in smithObjectSO_GameObjectList)
        {
           if (smithObjectSO_GameObject.smithObjectSO == e.SmithObjectSO)
           {
                smithObjectSO_GameObject.gameObject.SetActive(true);
           }
        }
    }
}
