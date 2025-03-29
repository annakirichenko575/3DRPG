using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Player {
    public class ManaSpend : MonoBehaviour {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private UnityEngine.UI.Image manaBar;
        [SerializeField] private UnityEngine.UI.Image _cooldownIcon;

        public static float mana;

        public static float maxMana = 100f;

        void Start()
        {
            mana = maxMana;
            manaBar.fillAmount = mana/maxMana;
            _cooldownIcon.fillAmount = 0f;
        }

        void Update()
        {
            if (playerInput.IsMagicAttack()){
                mana -= 20f;
                manaBar.fillAmount = mana/maxMana;
            }

            _cooldownIcon.fillAmount = playerInput._currentMagicCooldown/playerInput._maxMagicCooldown;
        }

        void OnTriggerEnter(Collider other) {
            if (other.tag == "mana") {
                mana += 10f;
                if (mana >= maxMana)
                    mana = maxMana;
                manaBar.fillAmount = mana/maxMana;
            }
        }
    }
}

