using System.Collections;
using UnityEngine;


public class PowerUp : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    PlayerHealth playerHealth;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;

    public float rotateSpeed = 50f;
    public float maxDistance = 0.8f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerShooting = FindObjectOfType<PlayerShooting>();
        Destroy(this.gameObject, 15);
    }
    private void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    IEnumerator Appear()
    {
        Vector3 normalPosition = transform.localPosition;
        Vector3 maxPosition = new Vector3(transform.localPosition.x, normalPosition.y + maxDistance, transform.localPosition.z);
        Vector3 minPosition = new Vector3(transform.localPosition.x, normalPosition.y - 5, transform.localPosition.z);
        float time = 0.0f;

        transform.position = minPosition;
        while (time < 0.4f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, maxPosition, time / 0.4f);
            time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        time = 0;

        while (time < 0.4f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, normalPosition, time / 0.4f);
            time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = normalPosition;
    }

    private void OnEnable()
    {
        StartCoroutine(Appear());
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + maxDistance, transform.position.z), Color.red);
    }

    IEnumerator Aquired(System.Action onCompleted)
    {
        rotateSpeed = 500f;
        float time = 0f;


        Color temp = _spriteRenderer.color;

        while (time < 2)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, 0.8f, 0), time / 2);
            transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale + new Vector3(1, 1, 1), time / 2);

            Color _tempColor = _spriteRenderer.color;
            GetComponent<SpriteRenderer>().color = new Color(_tempColor.r, _tempColor.g, _tempColor.b, Mathf.Lerp(temp.a, 0, time / 1));
            time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        onCompleted?.Invoke();
    }

    void DestroyObject()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" || other.isTrigger)
            return;

        if (this.gameObject.CompareTag("PowerHeal") && !playerHealth.IsInHealBuff)
            playerHealth.BeginHealPlayer();
        else if (this.gameObject.CompareTag("PowerHeal") && playerHealth.IsInHealBuff)
            return;

        if (this.gameObject.CompareTag("SpeedBoost") && !playerMovement.IsInSpeedBuff)
            playerMovement.BeginSpeedBoost();
        else if (this.gameObject.CompareTag("SpeedBoost") && playerMovement.IsInSpeedBuff)
            return;
        
        if (this.gameObject.CompareTag("AttackBoost") && !playerShooting.IsInAttackBuff)
            playerShooting.BeginAttackBoost();
        else if (this.gameObject.CompareTag("AttackBoost") && playerShooting.IsInAttackBuff)
            return;
        
        StartCoroutine(Aquired(DestroyObject));
    }    
}
