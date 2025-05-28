using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewElement", menuName = "Boss/Element")]
public class ElementConfig : ScriptableObject
{
    public string elementName;
    public int bonusDamage;

    public AudioClip soundEffect;
    public ParticleSystem particleEffectPrefab;

    public Color effectColor = Color.white;
}
