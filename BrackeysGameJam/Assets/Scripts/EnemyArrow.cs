using System.Collections;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    AudioPlayer audioPlayer;
    Player player;
    

    void Start()
    {
        player = FindObjectOfType<Player>();

        audioPlayer = FindObjectOfType<AudioPlayer>();
        audioPlayer.PlaySoundEffect(Enum.SoundEffects.EnemyProjectile);
    }

    void Update()
    {
        MoveTowards(player.transform.position);
    }

    private void MoveTowards(Vector2 target)
    {
        // move
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // todo: rotate to look at player
        transform.right = target - new Vector2(transform.position.x, transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Kill Player");
            Destroy(gameObject);
            player.Die();
        }
    }
}