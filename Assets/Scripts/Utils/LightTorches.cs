using UnityEngine;

public class LightTorches : MonoBehaviour, IDataPersistence
{
    private Animator _animator;
    private bool lighted = false;

    [SerializeField] private string id;
    [SerializeField] private AudioClip torchIgniteSoundClip;
    [SerializeField] private GameObject _light;

  [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!lighted)
        {
            _animator.SetBool("on", true);
            SoundFXManager.instance.PlaySoundFXClip(torchIgniteSoundClip, transform, 1, false);
            _light.SetActive(true);
            lighted = true;
        }
    }

    public void LoadData(GameData data)
    {
        data.spawnPointReached.TryGetValue(id, out lighted);
        if (lighted)
        {
            _animator.SetBool("on", true);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.spawnPointReached.ContainsKey(id))
        {
            data.spawnPointReached.Remove(id);
        }
        data.spawnPointReached.Add(id, lighted);
    }
}
