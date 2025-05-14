using System.Collections;
using UnityEngine;
using Player;

public class BossWeaponController : MonoBehaviour
{
    [Header("Weapon Objects")]
    [SerializeField] private GameObject hammer;
    [SerializeField] private GameObject staff;

    [Header("Effect Spawn Sockets")]
    [SerializeField] private Transform hammerSocket;
    [SerializeField] private Transform staffSocket;

    [SerializeField] private ElementConfig[] elements;

    private GameObject activeWeapon;
    private Transform activeSocket;
    private ElementConfig currentElement;

    public ElementConfig CurrentElement => currentElement;
    public string CurrentWeapon => activeWeapon?.name;

    public void InitializeWeapon()
    {
        bool useHammer = Random.value > 0.5f;

        hammer.SetActive(useHammer);
        staff.SetActive(!useHammer);
        activeWeapon = useHammer ? hammer : staff;
        activeSocket = useHammer ? hammerSocket : staffSocket;

        currentElement = elements[Random.Range(0, elements.Length)];
    }

    public void PlayElementEffect()
    {
        if (currentElement == null) return;

        if (currentElement.soundEffect != null)
        {
            var audio = GetComponent<AudioSource>();
            if (audio == null)
                audio = gameObject.AddComponent<AudioSource>();

            audio.clip = currentElement.soundEffect;
            audio.loop = true;
            audio.Play();
        }

        if (currentElement.particleEffectPrefab != null && activeSocket != null)
        {
            var instance = Instantiate(currentElement.particleEffectPrefab, activeSocket.position, activeSocket.rotation, activeSocket);
            Destroy(instance.gameObject, 2f); 
        }
    }

    public int GetElementalBonusDamage() => currentElement.bonusDamage;
}

