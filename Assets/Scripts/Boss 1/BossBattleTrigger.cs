using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleTrigger : MonoBehaviour
{
    [SerializeField] GameObject boss;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            BossStateController bossCtrl = boss.GetComponent<BossStateController>();
            
        }
    }
}
