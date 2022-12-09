using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public Tilemap SlideMap;
    public Tilemap ObstaclesMap;
    public Tilemap WalkMap;
    public Tilemap ItemsMap;
    public Tilemap DeathMap;

    public GameObject BloodSplatter;

    public float Speed = 5.0f;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 mDir;
    private bool mSliding;
    private bool mHasCollided = false;

    private int mItems = 0;
    public bool mIsDead = false;

    // Update is called once per frame
    void Update()
    {
        if (mIsDead)
            return;

        if(IsTouchingDeathTraps())
        {
            mIsDead = true;
            Debug.Log("Dying");
            Instantiate(BloodSplatter, transform);
        }

        Vector3Int pos = ItemsMap.WorldToCell(transform.position);
        if (ItemsMap.HasTile(pos))
        {
            mItems++;
            ItemsMap.SetTile(pos, null);
        }
        if (CanWalk())
        {
            mDir.y = 0.0f;
            mDir.x = Input.GetAxisRaw("Horizontal");
            if (mDir.x == 0.0f)
                mDir.y = Input.GetAxisRaw("Vertical");
            mSliding = false;
        }
        else if(mDir.x != 0.0f || mDir.y != 0)
            mSliding = true;

        if (!mHasCollided && Colliding())
        {
            mSliding = false;
            mDir = new Vector2(0.0f, 0.0f);
            mHasCollided = true;
        }
        else if(mHasCollided)
        {
            mDir.y = 0.0f;
            mDir.x = Input.GetAxisRaw("Horizontal");
            if (mDir.x == 0.0f)
                mDir.y = Input.GetAxisRaw("Vertical");
            mSliding = !CanWalk();
            if(mDir.x != 0.0f || mDir.y != 0.0f)
                mHasCollided = false;
        }

        animator.SetFloat("horizontal", mDir.x);
        animator.SetFloat("vertical", mDir.y);
        animator.SetFloat("Speed", mDir.sqrMagnitude);
        animator.SetBool("Sliding", mSliding);
    }

    private void FixedUpdate()
    {
        if(!Colliding() && !mIsDead)
            rb.MovePosition(rb.position + mDir * Speed * Time.fixedDeltaTime);
    }

    private bool Colliding()
    {
        Vector3 direction = new Vector3(mDir.x, mDir.y, 0.0f).normalized;
        Vector3Int pos = ObstaclesMap.WorldToCell(transform.position + direction * 0.5f);
        return ObstaclesMap.HasTile(pos);
    }

    private bool IsTouchingDeathTraps()
    {
        Vector3 direction = new Vector3(mDir.x, mDir.y, 0.0f).normalized;
        Vector3Int pos = DeathMap.WorldToCell(transform.position + direction * 0.5f);
        return DeathMap.HasTile(pos);
    }

    private bool CanWalk()
    {
        Vector3Int pos = WalkMap.WorldToCell(transform.position);
        return WalkMap.HasTile(pos);
    }
}
