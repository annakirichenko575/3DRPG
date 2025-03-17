using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPresenter : MonoBehaviour
{
    [SerializeField] private Movement movement;

    private Animator animator;

    private void Awake()
    {
        // �������� ��������� Animator
        animator = GetComponent<Animator>();
    }

    private string currentState = "Idle"; // ������� ��������� ��������

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
        if (IsPlaying("SadIdle")) // ���������, �� ������������� �� ��� ��� ��������
            return;

        Debug.Log("ToSadIdle");

        ResetAllTriggers(); // ���������� ��� ��������
        animator.SetTrigger("ToSadIdle"); // ���������� ������� ��� Sad Idle
    }

    // ����� ��� ������������ �� �������� "Walk"
    public void PlayWalk()
    {
        if (IsPlaying("Walking")) // ���������, �� ������������� �� ��� ��� ��������
            return;

        ResetAllTriggers(); // ���������� ��� ��������
        animator.SetTrigger("ToWalk"); // ���������� ������� ��� Walk
    }

    // ����� ��� ������������ �� �������� "Run"
    public void PlayRun()
    {
        if (IsPlaying("Run")) // ���������, �� ������������� �� ��� ��� ��������
            return;

        ResetAllTriggers(); // ���������� ��� ��������
        animator.SetTrigger("ToRun"); // ���������� ������� ��� Run
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

    // ����� ��� ��������, ������������� �� ��������� ��������
    private bool IsPlaying(string stateName)
    {
        // �������� ���������� � ������� ��������� ���������
        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // ���������, ��������� �� ��� ��������� � ������������� �� ��������
        return currentStateInfo.IsName(stateName);
    }
}
