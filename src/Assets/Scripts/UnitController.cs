using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that has methods to move and rotate unit
public class UnitController : MonoBehaviour
{
    [SerializeField]
    Transform weaponController;
    Vector3 weaponOriginalPos;

    //Unit max speed
    [SerializeField]
    float maxSpeed = 5f;

    //How fast it lerp to maxspeed or changes direction, 0 means infinit
    [SerializeField]
    float acelaration = 1f;
    [SerializeField]
    float desaceleration = 2f;
    [SerializeField]
    float pivotSpeed = 5f;

    Vector3 direction = Vector3.zero;
    Rigidbody rb;
    Vector3 velocity = Vector3.zero;

    Vector3 origin = Vector3.zero;
    Vector3 forward = Vector3.forward;
    Vector3 right = Vector3.right;
    Vector3 upward = Vector3.up;

    [SerializeField]
    float crawlDif = 2f;
    [SerializeField]
    float crawlSpeed = 4f;
    float crawlInterpolation = 0f;

    [SerializeField]
    float jumpPower = 10f;

    [SerializeField]
    Light lantern;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(weaponController)
        weaponOriginalPos = weaponController.transform.localPosition;
    }

    public void SetMovDir(Vector3 dir)
    {
        velocity = dir * maxSpeed * (2f - 1.5f * GetCrawlInterpol()) /2f;
        //CheckCollisions(dir);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        transform.rotation = Quaternion.LookRotation(forward, upward);
    }

    public void LookAt(Vector3 target)
    {
        origin = weaponController.position;
        forward = Vector3.ProjectOnPlane((target - origin), Vector3.up).normalized;
        right = Vector3.Cross(forward, -Vector3.up);
        upward = Vector3.Cross(forward, right);

        Debug.DrawRay(origin, upward, Color.green); //up
        Debug.DrawRay(origin, forward, Color.blue); // forward
        Debug.DrawRay(origin, right, Color.red); // right

        transform.rotation = Quaternion.LookRotation(forward, upward);
    }

    public void LookAt(Transform target)
    {
        
    }

    public void LanternOn(bool on)
    {
        if (lantern != null)
            lantern.enabled = on;
    }

    public bool LanternIsOn()
    {
        if (lantern != null)
            return lantern.enabled;
        else
            return false;
    }

    public bool TryJump()
    {
        if (IsGrounded())
        {
            rb.velocity += Vector3.up * jumpPower;
            return true;
        }
        return false;
    }

    public void Crawl(bool on)
    {
        if(on)
            crawlInterpolation += Time.deltaTime * crawlSpeed;
        else
            crawlInterpolation -= Time.deltaTime * crawlSpeed;
        crawlInterpolation = Mathf.Clamp01(crawlInterpolation);

        var interpol = Mathf.SmoothStep(0, 1, crawlInterpolation);
        Vector3 dif = Vector3.down * interpol * crawlDif;

        weaponController.localPosition = weaponOriginalPos + dif;
    }

    public float GetCrawlInterpol()
    {
        return Mathf.SmoothStep(0, 1, crawlInterpolation);
    }

    public bool IsGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        return Physics.Raycast(ray,0.15f);
    }

    //U: Check if collision is with static object and avoids going through it
    private void CheckCollisions(Vector3 dir)
    {
        int i = 0;
        Collider col;
        RaycastHit raycastHit = new RaycastHit();
        Physics.SphereCast(transform.position + Vector3.up * 5f,
            0.7f,
            Vector3.down * 10f,
            out raycastHit,
            10f,
            Layers.PropsLayer(),
            QueryTriggerInteraction.Collide);

        col = raycastHit.collider;

        while (Physics.SphereCast(transform.position + Vector3.up * 5f,
            0.7f,
            Vector3.down * 10f,
            out raycastHit,
            10f,
            Layers.PropsLayer(),
            QueryTriggerInteraction.Collide)
            && i < 500 && raycastHit.collider == col)
        {
            var goBackDir = (transform.position - raycastHit.point).normalized;
            transform.position -= goBackDir * 0.01f;
            i++;
        }
    }
}
