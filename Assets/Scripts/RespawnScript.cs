using UnityEngine;

public class Respawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter(Collision collision)
    {
        GameObject player = collision.gameObject;
        player.transform.position = new Vector3(0, 0, 0);
    }
}
