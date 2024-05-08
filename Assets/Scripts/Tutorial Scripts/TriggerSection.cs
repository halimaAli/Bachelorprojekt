using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSection : MonoBehaviour
{

    [SerializeField] private TutorialLevelManager.Section section;

    private void OnTriggerEnter(Collider other)
    {
        TutorialLevelManager.instance.SetSection(section);
    }
}
