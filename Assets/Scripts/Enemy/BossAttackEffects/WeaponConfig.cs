using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Boss/Weapon")]
public class WeaponConfig : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab; 
}

