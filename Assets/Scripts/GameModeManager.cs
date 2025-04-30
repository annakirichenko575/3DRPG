/* part for better game and future UI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Combat,
    Peaceful
}

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; }

    public GameMode CurrentMode { get; private set; } = GameMode.Combat;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetMode(GameMode mode)
    {
        CurrentMode = mode;
    }
}

*/
using UnityEngine;

public enum GameMode
{
    Combat,
    Peaceful
}
//temporary part for debug
public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; }

    [SerializeField] private bool isPeacefulMode; 

    public GameMode CurrentMode { get; private set; } = GameMode.Combat;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        CurrentMode = isPeacefulMode ? GameMode.Peaceful : GameMode.Combat;
    }

    public void SetMode(GameMode mode)
    {
        CurrentMode = mode;
        isPeacefulMode = (mode == GameMode.Peaceful); 
    }

    public bool IsPeaceful() => CurrentMode == GameMode.Peaceful;
}
