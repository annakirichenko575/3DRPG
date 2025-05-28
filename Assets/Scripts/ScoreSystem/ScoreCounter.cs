using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Enemy
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        private int score;

        void Update()
        {
            score = EnemyDeathCounter.enemiesKilled * 10;
            _scoreText.text = score.ToString();
        }
    }
}

