using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public Tilemap SlideMap;
    public Tilemap ObstaclesMap;
    public Tilemap WalkMap;

    public float Speed = 5.0f;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 mDir;
    private bool mSliding;

    // Update is called once per frame
    void Update()
    {
        if (CanWalk())
        {
            mDir.y = 0.0f;
            mDir.x = Input.GetAxisRaw("Horizontal");
            if (mDir.x == 0.0f)
                mDir.y = Input.GetAxisRaw("Vertical");
            mSliding = false;
        }
        else if (mDir.x != 0.0f && mDir.y != 0.0f && Colliding())
        {
            mSliding = false;
            mDir = new Vector2(0.0f, 0.0f);
        }
        else
            mSliding = true;

        animator.SetFloat("horizontal", mDir.x);
        animator.SetFloat("vertical", mDir.y);
        animator.SetFloat("Speed", mDir.sqrMagnitude);
        animator.SetBool("Sliding", mSliding);
    }

    private void FixedUpdate()
    {
        if(!Colliding())
            rb.MovePosition(rb.position + mDir * Speed * Time.fixedDeltaTime);
    }

    private bool Colliding()
    {
        Vector3 direction = new Vector3(mDir.x, mDir.y, 0.0f).normalized;
        Vector3Int pos = SlideMap.WorldToCell(transform.position + direction * 0.5f);
        return ObstaclesMap.HasTile(pos);
    }

    private bool CanWalk()
    {
        Vector3Int pos = WalkMap.WorldToCell(transform.position);
        return WalkMap.HasTile(pos);
    }
}
