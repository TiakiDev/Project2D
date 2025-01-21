using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack instance;
    
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackBox;
    [SerializeField] private float attackCooldown = 1f;

    public bool canAttack;
    public bool isAttacking = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        canAttack = false;
        attackBox.gameObject.SetActive(false);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        
        if (context.performed && canAttack && !isAttacking && !IsPointerOverUIObject())
        {
            isAttacking = true; // Ustaw flagę
            canAttack = false;  // Blokada ataków
            StartCoroutine(AttackCoroutine());
        }
    }
    
    private IEnumerator AttackCoroutine()
    {
        animator.SetBool("isAttacking", true);
        attackBox.gameObject.SetActive(true);
    
        yield return new WaitForSeconds(attackCooldown);
    
        animator.SetBool("isAttacking", false);
        attackBox.gameObject.SetActive(false);
        canAttack = true;  // Odblokowanie ataków
        isAttacking = false; // Flaga resetowana po zakończeniu korutyny
    }


    private bool IsPointerOverUIObject()
    {
        if (!EventSystem.current) return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }
}