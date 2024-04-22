using System.Collections;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Controls the player's attack actions.
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    // References to other components
    /// <summary>
    /// Reference to the sword range controller.
    /// </summary>
    public SwordRangeController swordRangeController;

    public SwordData currentSword;

    public UnityEvent attackEvent;

    // Private fields
    private bool _isAttacked = false;

    private Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();

    }

    /// <summary>
    /// Initiates an attack with the equipped sword.
    /// </summary>
    public void Attack()
    {
        // Check if an attack is currently in progress
        if (!_isAttacked)
        {
            // Start the attack coroutine
            StartCoroutine(AttackIEnumerator());
        }
    }

    // Coroutine for handling attack cooldown
    private IEnumerator AttackIEnumerator()
    {
        _anim.SetBool("isAttacking", true);
        _isAttacked = true;

        //swordRangeController.TriggerEnemyDamage(currentSword.damage);

        attackEvent.Invoke();
        Debug.Log("Test!");
        // Wait for the attack cooldown before allowing another attack
        yield return new WaitForSeconds(currentSword.attackSpeed);
        _anim.SetBool("isAttacking", false);
        _isAttacked = false;
    }
}
