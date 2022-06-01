using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Inventory")]
public class InventorySO : ScriptableObject
{
    public List<WeaponSO> Weapons;
}
