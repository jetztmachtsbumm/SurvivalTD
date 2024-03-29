using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }

    [SerializeField] private float averageTimeBetweenPhases = 60 * 5;

    private GamePhase currentGamePhase;
    private float defensePhaseTimer;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("It seems like there is more than one GameManager object active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;

        currentGamePhase = GamePhase.FARMING_PHASE;
        defensePhaseTimer = averageTimeBetweenPhases;
    }

    private void Update()
    {
        if (currentGamePhase == GamePhase.DEFENSE_PHASE) return;

        defensePhaseTimer -= Time.deltaTime;

        if(defensePhaseTimer <= 0)
        {
            SetCurrentGamePhase(GamePhase.DEFENSE_PHASE);
            BuildMode.Instance.ToggleBuildingSelectUI(false);
            BuildMode.Instance.SetInBuildMode(false);
            WaveManager.Instance.SetupNextWave();
            defensePhaseTimer = averageTimeBetweenPhases;
        }
    }

    public GamePhase GetCurrentGamePhase()
    {
        return currentGamePhase;
    }

    public void SetCurrentGamePhase(GamePhase gamePhase)
    {
        currentGamePhase = gamePhase;
    }

    public enum GamePhase
    {
        FARMING_PHASE,
        DEFENSE_PHASE
    }

}
