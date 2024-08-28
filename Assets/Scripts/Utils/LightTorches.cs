using UnityEngine;

public class LightTorches : MonoBehaviour, IDataPersistence
{
    private Animator _animator;
    private bool lighted = false;

    [SerializeField] private string id;

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
        _animator.SetBool("on", true);
        lighted = true;
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
