using System;
using Unity.VisualScripting;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour
{
    public static SoundtrackManager Instance;
    private PlayerController playerController;
    [SerializeField] private MusicLayer uncoverLayer;
    [SerializeField] private MusicLayer combatLayer;
    [SerializeField] private MusicLayer searchLayer;
    [SerializeField] private MusicLayer combatBgLayer;
    [SerializeField] private MusicLayer playerInSightLayer;
    [SerializeField] private MusicLayer warcallLayer;
    [SerializeField] private MusicLayer silentKillLayer;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        if (playerController.Hidden == false) uncoverLayer.Activate();
        uncoverLayer.Tick(Time.deltaTime);
        combatLayer.Tick(Time.deltaTime);
        searchLayer.Tick(Time.deltaTime);
        combatBgLayer.Tick(Time.deltaTime);
        playerInSightLayer.Tick(Time.deltaTime);
        warcallLayer.Tick(Time.deltaTime);
        silentKillLayer.Tick(Time.deltaTime);
    }
    public void ReportAgitation(float agitation, AgitationState agitationState, EnemyPerceptionState enemyPerceptionState, EnemyState enemyState = EnemyState.Alive)
    {
        if (agitationState == AgitationState.Alarmed)
        {
            if (enemyPerceptionState == EnemyPerceptionState.PlayerInSight) combatLayer.Activate();
            if (enemyPerceptionState != EnemyPerceptionState.Idle) searchLayer.Activate();
            combatBgLayer.Activate();
        }
        else
        {
            if (enemyPerceptionState == EnemyPerceptionState.PlayerInSight) playerInSightLayer.Activate();
        }
        if (enemyState == EnemyState.Dead)
            if (combatLayer.IsActive())
            {
                warcallLayer.Activate();
            }
            else
            {
                silentKillLayer.Activate();
            }
    }
}
[System.Serializable]
public class MusicLayer
{
    public AudioSource source;

    public float activeVolume = 1f;
    public float inactiveVolume = 0f;

    public float fadeInSpeed = 2f;
    public float fadeOutSpeed = 1f;

    public float holdTime = 5f;

    private float timer;
    private bool active;

    public void Activate()
    {
        active = true;
        timer = holdTime;
        if (!source.loop) source.Play();
    }

    public void Deactivate()
    {
        active = false;
    }

    public void Tick(float dt)
    {

        timer -= dt;


        float target = (timer > 0f) ? activeVolume : inactiveVolume;
        float speed = (timer > 0f) ? fadeInSpeed : fadeOutSpeed;

        source.volume = Mathf.Lerp(source.volume, target, dt * speed);
        if (source.volume < .01f) active = false;
    }
    public bool IsActive()
    {
        return active;
    }
}
