using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{


    [SerializeField]
    private float speed = 2.0f;
    [SerializeField]
    private float turnSmoothTime = 0.1f;
    [SerializeField]
    private float rotationSpeed = 5f;
    [SerializeField]
    private float animationSmoothTime = 0.1f;



    private CharacterController controller;
    private Transform cam;
    private PlayerInput playerInput;
    public bool isAiming;

    private InputAction moveAction;
    private InputAction aimAction;
    private InputAction switchWeaponAction;
    private InputAction attackAction;

    public Animator animator;

    int moveXAnimationParameterId;
    int moveZAnimationParameterId;
    
    float turnSmoothVelocity;
    Vector2 currentAninmationBlendVector;
    Vector2 animationVelocity;

    public WeaponSO currentWeapon;
    public int currentWeaponType;
    public InventorySO inventory;
    private int currentWeaponIndex;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main.transform;

        moveAction = playerInput.actions["Move"];
        aimAction = playerInput.actions["Aim"];
        switchWeaponAction = playerInput.actions["SwitchWeapon"];
        attackAction = playerInput.actions["Attack"];

        switchWeaponAction.performed += _ => changeWeapon(switchWeaponAction.ReadValue<float>());

        Cursor.lockState = CursorLockMode.Locked;
        //Animations
        moveXAnimationParameterId = Animator.StringToHash("MoveX");
        moveZAnimationParameterId = Animator.StringToHash("MoveZ");        




    }

    void Start()
    {
    }




       void Update()
    {
        Aiming();
        Movement();
        Attack();
    }

    void Movement(){
         Vector2 input = moveAction.ReadValue<Vector2>();
        currentAninmationBlendVector = Vector2.SmoothDamp(currentAninmationBlendVector, input, ref animationVelocity, animationSmoothTime);
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z)*Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle =Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if(isAiming == false)
            {
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            }
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        animator.SetFloat(moveXAnimationParameterId, currentAninmationBlendVector.x);
        animator.SetFloat(moveZAnimationParameterId, currentAninmationBlendVector.y);
        
        if(isAiming == true)
        {
        //rotate player towards camera direction
        Quaternion targetRoatation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRoatation, rotationSpeed * Time.deltaTime);
        }
        
        animator.SetFloat("Speed", direction.magnitude);
    }

    private void Aiming()
    {
        aimAction.performed += _ => isAiming = true;
        aimAction.canceled += _ => isAiming = false;

        animator.SetInteger("WeaponType", currentWeaponType);
        animator.SetBool("IsAiming", isAiming);
    }

    private int weaponTypeToInt(WeaponSO _weaponSO){
        if(_weaponSO.weaponType == WeaponSO.WeaponType.Melee){
            return 0;
        }
        if(_weaponSO.weaponType ==  WeaponSO.WeaponType.HandGun){
            return 1;
        }
        if(_weaponSO.weaponType ==  WeaponSO.WeaponType.Rifle){
            return 2;
        }
        return 0;
    }

    private void Attack(){
        attackAction.performed += _ => currentWeapon.Attack();
        attackAction.canceled -= _ => currentWeapon.Attack();
    }

    private void changeWeapon(float _val){      
        
        int lastItem = inventory.Weapons.Count - 1;

        if(_val < 0)
        {
            if(currentWeaponIndex <= 0)
            {
                currentWeapon = inventory.Weapons[lastItem];
                setWeapon(inventory.Weapons[lastItem]);
                currentWeaponIndex = lastItem;
            }
            else
            {
                currentWeapon = inventory.Weapons[currentWeaponIndex-1];
                setWeapon(inventory.Weapons[currentWeaponIndex-1]);
            }
        }    
        if(_val > 0)
        {
            if(currentWeaponIndex >= lastItem)
            {
                currentWeapon = inventory.Weapons[0];
                setWeapon(inventory.Weapons[0]);
                currentWeaponIndex = 0;
            }
            else
            {
                currentWeapon = inventory.Weapons[currentWeaponIndex+1];
                setWeapon(inventory.Weapons[currentWeaponIndex+1]);
            }
        }
        Debug.Log(currentWeapon);
    }

    private void setWeapon(WeaponSO _weapon){
        for(int i=0;i<inventory.Weapons.Count;i++){
            if(_weapon == inventory.Weapons[i]){
                currentWeapon = inventory.Weapons[i];
                currentWeaponType = weaponTypeToInt(inventory.Weapons[i]);
                currentWeaponIndex = i;
            }
        }
    }

}


