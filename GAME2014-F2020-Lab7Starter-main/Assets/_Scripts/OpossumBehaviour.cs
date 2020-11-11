using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumBehaviour : MonoBehaviour
{
    public float runForce;
    public bool isGroundAhead;
    public Transform lookAheadPoint;
    public Transform lookBelowPoint;
    public Rigidbody2D rigidbody2D;
    public LayerMask colGroundLayer;
    public LayerMask colWallLayer;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
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
        isGroundAhead = Physics2D.Linecast(transform.position, lookBelowPoint.position, colGroundLayer);
        Debug.DrawLine(transform.position, lookBelowPoint.position, Color.green);
    }

    private void _LookInFront()
    {
        if (isGroundAhead = Physics2D.Linecast(transform.position, lookAheadPoint.position, colWallLayer))
            _FlipX();
        Debug.DrawLine(transform.position, lookAheadPoint.position, Color.green);
    }

    private void _FlipX()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
    }
}
