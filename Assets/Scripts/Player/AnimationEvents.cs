using UnityEngine;

namespace Player
{
    public class AnimationEvents : MonoBehaviour
    {
        [SerializeField] private AttackZone attackZone;

        private void PhysicAttack()
        {
            attackZone.Attack();
        }

        //1.��������� �� ������� ���������� �����
        //2.�������� ������ ������
        //3.������� ������ ������ � ���������� �����

        //4.������ ���� ����� �� AttackZone � OnTriggerEnter`� ������ ���� ������ ��������� ��� ���������
    }
}