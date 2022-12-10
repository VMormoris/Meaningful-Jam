using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//-26.56505f
public class PlayerMovement : MonoBehaviour
{
    public Tilemap SlideMap;
    public Tilemap ObstaclesMap;
    public Tilemap WalkMap;
    public Tilemap ItemsMap;
    public Tilemap DeathMap;
    public Tilemap SlopesMap;
    public Tilemap ElevatedMap;

    public GameObject BloodSplatter;

    public float Speed = 5.0f;
    public Rigidbody2D rb;
    public Animator animator;

    public Vector2 mDir;
    private bool mSliding;
    private bool mHasCollided = false;
    private bool mElevating = false;

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

        if (IsOnSlope())
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45.0f));
        else
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));

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
        Vector3 dir = new Vector3(mDir.x, mDir.y);
        if ((!Colliding() || mElevating) && !mIsDead)
        {
            Vector2 offset = transform.TransformDirection(dir * Speed * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + offset);
            Vector3Int pos = ElevatedMap.WorldToCell(rb.position);
            if (ElevatedMap.HasTile(pos))
                mElevating = false;
        }
    }

    private bool Colliding()
    {
        Vector3 dir = new Vector3(mDir.x, mDir.y);
        Vector3Int pos = SlideMap.WorldToCell(transform.position);
        Vector3Int NextPos = ObstaclesMap.WorldToCell(transform.position + dir * 0.5f);
        Vector3 DownX = new Vector3(mDir.x, -1.0f).normalized;
        Vector3Int DownNext = SlopesMap.WorldToCell(transform.position + DownX * 0.5f);
        return ObstaclesMap.HasTile(NextPos) || 
            (SlideMap.HasTile(pos) && ElevatedMap.HasTile(NextPos) && !SlopesMap.HasTile(pos)) ||
            (ElevatedMap.HasTile(pos) && SlideMap.HasTile(NextPos) && !SlopesMap.HasTile(DownNext)) ||
            (mDir.y != 0.0f && SlopesMap.HasTile(NextPos));
    }

    private bool IsTouchingDeathTraps()
    {
        Vector3 direction = new Vector3(mDir.x, mDir.y, 0.0f).normalized;
        Vector3Int pos = DeathMap.WorldToCell(transform.position + direction * 0.5f);
        return DeathMap.HasTile(pos);
    }

    private bool IsOnSlope()
    {
        Vector3Int pos = SlopesMap.WorldToCell(transform.position);
        mElevating = true;
        return SlopesMap.HasTile(pos) && mDir.x != 0.0f;
    }

    private bool CanWalk()
    {
        Vector3Int pos = WalkMap.WorldToCell(transform.position);
        return WalkMap.HasTile(pos);
    }
}
