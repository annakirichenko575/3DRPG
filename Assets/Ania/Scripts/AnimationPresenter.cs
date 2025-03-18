using UnityEngine;

public class AnimationPresenter : MonoBehaviour
{
    private const string SadIdleState = "SadIdle";
    private const string WalkState = "Walking";
    private const string RunState = "Run";
    private const string ToSadIdleName = "ToSadIdle";
    private const string ToWalkName = "ToWalk";
    private const string ToRunName = "ToRun";

    [SerializeField] private Movement movement;

    private Animator animator;
    private bool IsIdle, IsWalk, IsRun;

    private void Awake()
    {
        // �������� ��������� Animator
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
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
        }
        return false;
    }

    private void SetPlaying(string stateName)
    {
        IsIdle = false;
        IsWalk = false;
        IsRun = false;
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
        }
    }
}
