using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    Animator animator;

    int comboIndex = 0;

    bool canQueueNextAttack = false;
    bool nextAttackQueued = false;

    bool wasIdle = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        bool isIdle = animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");

        if (isIdle && !wasIdle && comboIndex > 0)
        {
            ResetCombo();
        }

        wasIdle = isIdle;
    }

    public void OnAttack(InputValue value)
    {
        Debug.Log("Attack Pressed");

        if (comboIndex == 0)
        {
            comboIndex = 1;

            animator.SetInteger("ComboIndex", 1);
            animator.SetTrigger("Attack");

            Debug.Log("ComboIndex = 1");
        }
        else if (canQueueNextAttack &&
                 !nextAttackQueued &&
                 comboIndex < 4)
        {
            comboIndex++;

            animator.SetInteger("ComboIndex", comboIndex);

            nextAttackQueued = true;

            Debug.Log("ComboIndex = " + comboIndex);
        }
    }

    public void OpenComboWindow()
    {
        Debug.Log("OpenCW");

        canQueueNextAttack = true;

        // Allow only one queue per attack
        nextAttackQueued = false;
    }

    public void CloseComboWindow()
    {
        Debug.Log("CloseCW");

        canQueueNextAttack = false;
    }

    public void ResetCombo()
    {
        Debug.Log("ResetCombo");

        comboIndex = 0;

        animator.SetInteger("ComboIndex", 0);

        canQueueNextAttack = false;
        nextAttackQueued = false;
    }
}