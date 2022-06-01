using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeMaterial : MonoBehaviour
{

    
    public PlayerInput playerInput;
    private InputAction toggleGlassesAction;

    public bool glassesOn = false;

    public twoTextureSO tex;

    Renderer _renderer;
    Material[] _materials;

    void Awake(){
        
        _renderer = gameObject.GetComponent<MeshRenderer> ();
        _materials = _renderer.materials;

        if(glassesOn == false){
        _materials[0] = tex.adMaterial;
        _renderer.materials = _materials;
        }
        else if(glassesOn == true){
        _materials[0] = tex.ObeyMaterial;
        _renderer.materials = _materials;
        }
        

                toggleGlassesAction = playerInput.actions["GlassesToggle"];
                toggleGlassesAction.performed+= _ => Change();
    }



    public void Change(){
        Debug.Log("change: " + glassesOn);
        glassesOn = !glassesOn;

        if(glassesOn == false){
        _materials[0] = tex.adMaterial;
        _renderer.materials = _materials;
        }
        else if(glassesOn == true){
        _materials[0] = tex.ObeyMaterial;
        _renderer.materials = _materials;
        }

    }
}
