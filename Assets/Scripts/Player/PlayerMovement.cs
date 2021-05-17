using System.Collections;
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;

    int floorMask;
    float camRayLength = 100f;

    public GameObject nonRotatingChild;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    private void Update()
    {
        nonRotatingChild.transform.rotation = Quaternion.Euler(0, gameObject.transform.rotation.y * -1.0f, 0);
    }

    public void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }

    public void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    public void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }

    #region Boost

    [Header("Boost Properties")]
    public float speedBoostMultiplier = 1.3f;
    public float speedBoostDuration = 10f;

    public bool IsInSpeedBuff;

    public void BeginSpeedBoost()
    {
        StartCoroutine(SpeedBoost());
    }

    public void StopSpeedBoost()
    {
        StopCoroutine(SpeedBoost());
    }

    IEnumerator SpeedBoost()
    {
        Debug.Log("Boosted");
        IsInSpeedBuff = true;

        float normalSpeed = speed;
        speed *= speedBoostMultiplier;

        yield return new WaitForSeconds(speedBoostDuration);

        speed = normalSpeed;
        IsInSpeedBuff = false;
    }

    #endregion

}
