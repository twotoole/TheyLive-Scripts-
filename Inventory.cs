using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventorySO _inventory;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(_inventory.Weapons);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
