using UnityEngine;
using System.Collections;

public class AnimationPresenter : MonoBehaviour
{
    private const string SadIdleState = "SadIdle";
    private const string WalkState = "Walking";
    private const string RunState = "Run";
    private const string PhysicAttackState = "PhysicAttack"; // ����� ��������� �����
    private const string ToSadIdleName = "ToSadIdle";
    private const string ToWalkName = "ToWalk";
    private const string ToRunName = "ToRun";
    private const string ToPhysicAttackName = "ToPhysicAttack"; // ����� ������� ��� �����

    [SerializeField] private Movement movement;

    private Animator animator;
    private bool IsIdle, IsWalk, IsRun, IsPhysicAttack;

    private void Awake()
    {
        // �������� ��������� Animator
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsPhysicAttack) // ���� ����� �������, ���������� ������ ��������
            return;

        if (movement.IsIdle())
        {
            PlaySadIdle(); // �������� �������� Sad Idle
        }
        else
        {
            if (movement.IsRun())
            {
                PlayRun(); // �������� �������� Run
            }
            else
            {
                PlayWalk(); // �������� �������� Walk
            }
        }

        // �������� �� ������� ����� ������ ����
        if (Input.GetMouseButtonDown(0)) // ����� ������ ����
        {
            PlayPhysicAttack(); // �������� �������� �����
        }
    }

    // ����� ��� ������������ �� �������� "Sad Idle"
    public void PlaySadIdle()
    {     
        if (IsPlaying(SadIdleState)) // ���������, �� ������������� �� ��� ��� ��������
            return;
        
        ResetAllTriggers(); // ���������� ��� ��������
        animator.SetTrigger(ToSadIdleName); // ���������� ������� ��� Sad Idle
        SetPlaying(SadIdleState);
    }

    // ����� ��� ������������ �� �������� "Walk"
    public void PlayWalk()
    {
        if (IsPlaying(WalkState)) // ���������, �� ������������� �� ��� ��� ��������
            return;

        ResetAllTriggers(); // ���������� ��� ��������
        animator.SetTrigger(ToWalkName); // ���������� ������� ��� Walk
        SetPlaying(WalkState);
    }

    // ����� ��� ������������ �� �������� "Run"
    public void PlayRun()
    {
        if (IsPlaying(RunState)) // ���������, �� ������������� �� ��� ��� ��������
            return;

        ResetAllTriggers(); // ���������� ��� ��������
        animator.SetTrigger(ToRunName); // ���������� ������� ��� Run
        SetPlaying(RunState);
    }

    // ����� ��� ������������ �� �������� "Physic Attack"
    public void PlayPhysicAttack()
    {
        if (IsPlaying(PhysicAttackState)) // ���������, �� ������������� �� ��� ��� ��������
            return;

        ResetAllTriggers(); // ���������� ��� ��������
        animator.SetTrigger(ToPhysicAttackName); // ���������� ������� ��� Physic Attack
        SetPlaying(PhysicAttackState);

        // ��������� �������� ��� ���������� �����
        StartCoroutine(CompletePhysicAttack());
    }

    // ����� ��� ������ ���� ���������
    private void ResetAllTriggers()
    {
        foreach (var trigger in animator.parameters)
        {
            if (trigger.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(trigger.name); // ���������� ������ �������
            }
        }
    }

    private bool IsPlaying(string stateName)
    {
        switch (stateName)
        {
            case SadIdleState:
                return IsIdle;
            case WalkState:
                return IsWalk;
            case RunState:
                return IsRun;
            case PhysicAttackState:
                return IsPhysicAttack; // �������� ��� �����
        }
        return false;
    }

    private void SetPlaying(string stateName)
    {
        IsIdle = false;
        IsWalk = false;
        IsRun = false;
        IsPhysicAttack = false; // ���������� ���� �����
        switch (stateName)
        {
            case SadIdleState:
                IsIdle = true;
                break;
            case WalkState:
                IsWalk = true;
                break;
            case RunState:
                IsRun = true;
                break;
            case PhysicAttackState:
                IsPhysicAttack = true; // ������������� ���� �����
                break;
        }
    }

    // �������� ��� ���������� �����
    private IEnumerator CompletePhysicAttack()
    {
        // ���� ���������� �������� ����� (����� ����� ���������)
        yield return new WaitForSeconds(2f); // ������: 1 ������� �� �������� �����

        IsPhysicAttack = false; // ���������� ���� �����
    }
}
