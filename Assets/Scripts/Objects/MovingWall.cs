using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public bool canOpen;
    private bool close;
    [SerializeField] Transform target;
    private Vector3 originalPos;
    [SerializeField] private AudioClip cageSoundClip;
    [SerializeField] private float speed;

    [SerializeField] private Transform player;
    [SerializeField] private Type type;

    private enum Type
    {
        Door,
        Wall
    }

    void Start()
    {
        canOpen = false;
        close = false;
        originalPos = gameObject.transform.localPosition;
    }

    void Update()
    {
        if (canOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
        } 

        if (close)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(originalPos.x, transform.position.y, transform.position.z), Time.deltaTime * speed);
        }
    }

    public void Move()
    {
        close = false;
        canOpen = true;
        SoundFXManager.instance.PlaySoundFXClip(cageSoundClip, transform, 1, false);
    }


    private void OnCollisionExit(Collision collision)
    {
        if (type == Type.Wall)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                player.SetParent(null);
            }
        }
           
    }

    public void MoveBack()
    {
        canOpen = false;
        close = true;
        SoundFXManager.instance.PlaySoundFXClip(cageSoundClip, transform, 1, false);
    }
}
