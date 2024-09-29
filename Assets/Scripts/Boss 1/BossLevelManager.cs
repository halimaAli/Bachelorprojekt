using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class BossLevelManager : MonoBehaviour
{
    public static BossLevelManager instance;
    [SerializeField] private BossStateController boss;
    [SerializeField] private GameObject player;

    [Header("Player Won Components")]
    [SerializeField] private MovingWall door;
    [SerializeField] private GameObject secondRoom;
    private EndingDialogue endingDialogue;

    [Header("Screens Components")]
    [SerializeField] Image gameWonScreen;
    [SerializeField] TMP_Text gameWontext;

    [Header("Summon Phase Components")]
    [SerializeField] GameObject spawnManager;
    [SerializeField] GameObject _3DBackground;
    [SerializeField] private DisableCollider platform;

    public Phase currentPhase = Phase.AttackPhase1;
    private float timer = 3;

    [SerializeField] LoadingScene sceneLoader;
    [SerializeField] private float fadeSpeed;


    public enum Phase
    {
        AttackPhase1,
        SummonPhase,
        AttackPhase2
    } 

    private void Start()
    {
        if (boss == null)
        {
            boss = BossStateController.instance;
        }
        if (instance == null) { instance = this; }

        spawnManager.SetActive(false);
       // CameraManager.instance.DisableViewSwitch();
        endingDialogue = GetComponent<EndingDialogue>();
    }

    private void Update()
    {
        if (currentPhase == Phase.SummonPhase)
        {
            if (Camera.main.orthographic)
            {
                CameraManager.instance.Set3DView();
            }

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                spawnManager.SetActive(true);
            }
        } 
        else if (currentPhase == Phase.AttackPhase1 || currentPhase == Phase.AttackPhase2)
        {
            
            if (!Camera.main.orthographic) CameraManager.instance.Set2DView();
        }

        _3DBackground.SetActive(!Camera.main.orthographic);
        
    }

    public void setPhase(int phaseNum)
    {
       switch(phaseNum)
       {
            case 1: currentPhase = Phase.AttackPhase1; break;
            case 2: currentPhase = Phase.SummonPhase; break;
            case 3: currentPhase = Phase.AttackPhase2; break;
            default: break;  
       }

        if (currentPhase == Phase.AttackPhase2)
        {
            boss.EndSummonPhase();
            platform.DisableColliders();
        }
    }

    internal Phase getcurrentPhase()
    {
        return currentPhase;
    }

    public void ShowGameWonScreen()
    {
        PlayerController.instance.active = false;
        endingDialogue.isPlayerClose = true;
    }

    public void Result(bool won)
    {
        if (won)
        {
            secondRoom.SetActive(true);
            door.canOpen = true;
        } 
        else
        {
            boss.Won();
            PlayerController.instance.active = false;
        }
    }
}
