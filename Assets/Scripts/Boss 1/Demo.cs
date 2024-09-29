using UnityEngine;

public class Demo : MonoBehaviour
{
    public bool isDemo;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StartPhase2()
    {
        BossStateController.instance.health = 20;
    }

    public void SetDemo(bool isDemo)
    {
        this.isDemo = isDemo;
    }
}
