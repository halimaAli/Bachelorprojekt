using UnityEngine;
using UnityEngine.UI;

public class SwitchTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private Image switchTimerBar;
    [SerializeField] private float max3DViewTime = 10f;
    [SerializeField] private float coolDownTimer; //has to be proportional to the countdown

    internal bool is3DViewActive;
    internal bool isOnCoolDown;
    private float timer;
    private bool active;

    void Awake()
    {
        timer = max3DViewTime;
        active = true;
        UpdateTimerUI();
    }

    void Update()
    {
        if (!active)
        {
            return;
        }

        if (is3DViewActive && !isOnCoolDown) // in 3D View
        {
            Timer();
        }
        else if (isOnCoolDown) // in 2D view, cooldown
        {
            StartCoolDown();
        }
        else
        {
            CameraManager.instance.allowViewModeChange = true; // in 2D view, after cooldown
        }
    }

    private void Timer()
    {
        timer -= Time.deltaTime;
        UpdateTimerUI();

        if (timer <= 0)
        {
            timer = 0;
            is3DViewActive = false;
            CameraManager.instance.Set2DView();
            StartCoolDown();
        }
    }

    private void StartCoolDown()
    {
        if (!isOnCoolDown)
        {
            coolDownTimer = max3DViewTime - timer;
            timer = 0;
            isOnCoolDown = true;
        }

        CameraManager.instance.allowViewModeChange = false;
        timer += Time.deltaTime;
        UpdateTimerUI();

        if (timer >= coolDownTimer)
        {
            timer = max3DViewTime;
            isOnCoolDown = false;
        }
    }

    private void UpdateTimerUI()
    {
        float fillAmount;

        if (isOnCoolDown)
        {
            fillAmount = timer / coolDownTimer;
        }
        else
        {
            fillAmount = timer / max3DViewTime;
        }

        switchTimerBar.fillAmount = fillAmount;
    }

    public void TurnTimerOff()
    {
        active = false;
    }

    public void TurnTimerOn()
    {
        active = true;
    }
}
