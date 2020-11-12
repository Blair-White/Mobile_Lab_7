using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum RampDirection { UP, DOWN, NONE }
public class OpossumBehaviour : MonoBehaviour
{
    public float runForce;
    public bool isGroundAhead;
    public Transform lookAheadPoint;
    public Transform lookBelowPoint;
    public Rigidbody2D rigidbody2D;
    public LayerMask colGroundLayer;
    public LayerMask colWallLayer;
    public bool onRamp;
    public RampDirection rampDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rampDirection = RampDirection.NONE;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _LookInFront();
        _LookAhead();
        _Move();
    }

    private void _Move()
    {
        if(isGroundAhead)
        {
            rigidbody2D.AddForce(Vector2.left * runForce * Time.deltaTime * transform.localScale.x);
            if(onRamp)
            {
                
                if(rampDirection == RampDirection.UP)
                {

                    rigidbody2D.AddForce(Vector2.up * runForce * 0.5f * Time.deltaTime);
                }
                else
                {
                    rigidbody2D.AddForce(Vector2.down * runForce * 0.25f * Time.deltaTime);
                }
                StartCoroutine(Rotate());
            }
            else
            {
                StartCoroutine(Normalize());
            }
            
            rigidbody2D.velocity *= 0.90f;
        }
        else 
        {
            _FlipX();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platforms"))
        {
            isGroundAhead = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platforms"))
        {
            isGroundAhead = false;
        }
    }

    private void _LookAhead()
    {
        var groundHit = Physics2D.Linecast(transform.position, lookBelowPoint.position, colGroundLayer);
        if (groundHit)
        {
            if(groundHit.collider.CompareTag("Ramps"))
            {
                onRamp = true;
                
            }

            if(groundHit.collider.CompareTag("Platforms"))
            {
                onRamp = false;
            }
            isGroundAhead = true;
        }
        else
        {
            isGroundAhead = false;
        }
        Debug.DrawLine(transform.position, lookBelowPoint.position, Color.green);
    }

    private void _LookInFront()
    {
        var wallHit = Physics2D.Linecast(transform.position, lookAheadPoint.position, colWallLayer);
        if (wallHit)
        {
            if(!wallHit.collider.CompareTag("Ramps"))
            {
                if (!onRamp)
                { 
                       _FlipX();
                }
                rampDirection = RampDirection.DOWN;
            } 
        }
        else
        {
            rampDirection = RampDirection.UP;
        }
            
        Debug.DrawLine(transform.position, lookAheadPoint.position, Color.green);
    }

    private void _FlipX()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
    }

    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, -26.0f);
    }
    IEnumerator Normalize()
    {
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
}
