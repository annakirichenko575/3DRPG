using UnityEngine;

namespace Player
{
    public class AnimationEvents : MonoBehaviour
    {
        [SerializeField] private AttackZone attackZone;
        [SerializeField] private MagicAttacker magicAttacker;

        private void PhysicAttack()
        {
            attackZone.Attack();
        }

        private void MagicAttack()
        {
            magicAttacker.Attack();
        }

    }
}