using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class BossLevelManager : MonoBehaviour
{
    public static BossLevelManager instance;
    [SerializeField] private BossStateController boss;
    [SerializeField] GameObject spawnManager;
    [SerializeField] GameObject _3DBackground;
    private Phase currentPhase = Phase.AttackPhase1;
    private float timer = 3;

    internal enum Phase
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
        CameraManager.instance.DisableViewSwitch();
    }

    private void Update()
    {
        if (currentPhase == Phase.SummonPhase)
        {
            CameraManager.instance.allowViewModeChange = false;
            CameraManager.instance.Set3DView();

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                spawnManager.SetActive(true);
            }
         } 
        else if (currentPhase == Phase.AttackPhase1 || currentPhase == Phase.AttackPhase2)
         {
             CameraManager.instance.Set2DView();
         }

        _3DBackground.SetActive(!Camera.main.orthographic);
    }

    internal void setPhase(int phaseNum)
    {
       switch(phaseNum)
        {
            case 1: currentPhase = Phase.AttackPhase1; break;
            case 2: currentPhase = Phase.SummonPhase; break;
            case 3: currentPhase = Phase.AttackPhase2; break;
            default: break;  
        }
    }

    internal Phase getcurrentPhase()
    {
        return currentPhase;
    }
}
