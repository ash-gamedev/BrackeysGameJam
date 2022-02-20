using System.Collections;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();

        // flip to target player
        if(player.transform.position.x < transform.position.x)
            transform.localScale = new Vector2(-1f, 1f);
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