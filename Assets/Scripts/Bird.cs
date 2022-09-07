using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public Rigidbody Rb;
    public GameObject Feathers;
    public GameObject FeatherExplosion;
    public AudioSource Slingshot;
    public AudioSource SlingshotRelease;
    public AudioSource Flying;
    public AudioSource BirdCollision;
    public float ReleaseTime = 0.5f;
    public float DestructionTime = 5f;
    private bool _isPressed;
    private bool _isFired;

    [Header("Display Controls")]
    [SerializeField]
    [Range(10, 100)]
    private int LinePoints = 25;
    [SerializeField]
    [Range(0.01f, 0.25f)]
    private float TimeBetweenPoints = 0.1f;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private SpringJoint springJoint;

    [SerializeField] private Transform slingShot;
    [SerializeField] private Transform hook;
    private void Start()
    {
        foreach (Transform GO in slingShot.GetComponentsInChildren<Transform>(true))
        {
            Debug.Log(GO.name);
            if (GO.name == "Hook")
            {
                hook = GO;
            }
        }
    }

    void FixedUpdate()
    {
        if (_isPressed && !_isFired && !GameManager.Instance.IsLevelCleared)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 6.5f));
            DrawProjection(worldPosition);
            //next lines are to make sure you can grab the bird
            if (worldPosition.y >= 0.2f && worldPosition.y <= 8f)
            {
                Rb.position = worldPosition;
            }
        }
    }


    private void DrawProjection(Vector3 ReleasePosition)
    {
        
        if (hook != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;
            Vector3 startPosition = ReleasePosition;
            Vector3 _direction = hook.transform.position - startPosition;
            //Vector3 startVelocity = springJoint.spring * _direction / Rb.mass;
            Vector3 startVelocity = Mathf.Sqrt(springJoint.spring) * _direction/ Rb.mass;

            int i = 0;
            lineRenderer.SetPosition(i, startPosition);
            for (float time = 0; time < LinePoints; time += TimeBetweenPoints)
            {
                i++;
                Vector3 point = startPosition + time * startVelocity;
                point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time) - springJoint.damper * time;

                lineRenderer.SetPosition(i, point);

                Vector3 lastPosition = lineRenderer.GetPosition(i - 1);

                if (Physics.Raycast(lastPosition,
                    (point - lastPosition).normalized,
                    out RaycastHit hit,
                    (point - lastPosition).magnitude,
                    GrenadeCollisionMask))
                {
                    lineRenderer.SetPosition(i, hit.point);
                    lineRenderer.positionCount = i + 1;
                    return;
                }
            }
        }
    }


    void OnMouseDown()
    {
        if (_isFired || GameManager.Instance.IsLevelCleared)
        {
            return;
        }

        _isPressed = true;
        Rb.isKinematic = true;
        Slingshot.Play();
    }

    void OnMouseUp()
    {
        if (_isFired || GameManager.Instance.IsLevelCleared)
        {
            return;
        }

        _isPressed = false;
        Rb.isKinematic = false;

        GameManager.Instance.ActiveTurn = true;

        GetComponent<TrailRenderer>().enabled = true;
        _isFired = true;
        SlingshotRelease.Play();
        Flying.Play();
        StartCoroutine(Release());
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<TrailRenderer>().enabled = false;
        if (!collision.collider.CompareTag("Ground"))
        {
            GameObject feathers = Instantiate(Feathers, transform.position, Quaternion.identity);
            Destroy(feathers, 2);
            if (!BirdCollision.isPlaying)
            {
                BirdCollision.Play();
            }
            GameManager.Instance.AddScore(Random.Range(5, 25) * 10, transform.position, Color.white);
        }
    }

    IEnumerator Release()
    {
        yield return new WaitForSeconds(ReleaseTime);

        Destroy(GetComponent<SpringJoint>());
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(DestructionTime);

        GameManager.Instance.SetNewBird();
        GameManager.Instance.BirdDestroy.Play();
        Instantiate(FeatherExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
