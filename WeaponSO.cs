using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Weapon")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public enum WeaponType{Melee, HandGun, Rifle};
    public WeaponType weaponType;
    public Image iconImage;
    public GameObject model;

    public void Attack(){
        Debug.Log(name + ": isAttacking");
    }
}

