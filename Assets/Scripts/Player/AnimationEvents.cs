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

        //1.подпишись на событие магической атаки
        //2.Заспавнь маджик буллет
        //3.Направь маджик буллет в ближайшего врага

        //4.Скрипт пули похож на AttackZone в OnTriggerEnter`е только пуля должна исчезнуть при попадании
    }
}