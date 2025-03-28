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

        public static float mana;

        public static float maxMana = 100f;

        void Start()
        {
            mana = maxMana;
            manaBar.fillAmount = mana/maxMana;
        }

        void Update()
        {
            if (playerInput.IsMagicAttack()){
                mana -= 20f;
                manaBar.fillAmount = mana/maxMana;
            }
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

