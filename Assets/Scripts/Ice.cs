using UnityEngine;

public class Ice : MonoBehaviour
{
    public GameObject IceShatter;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 8)
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        GameObject shatter = Instantiate(IceShatter, transform.position, Quaternion.identity);
        Destroy(shatter, 2);
        Destroy(gameObject);
    }
}
