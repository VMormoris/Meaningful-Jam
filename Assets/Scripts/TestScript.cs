using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TestScript : MonoBehaviour
{
    public Tilemap WalkMap;
    public Tilemap SlideMap;
    public Tilemap ObstacleMap;
    public Tilemap SlopeMap;
    public Tilemap ElevatedMap;

    public Animator animator;

    public float Speed = 5.0f;
  
    public Vector2 mDir;
    private Vector3Int mTarget;
    private bool mMoving = false;
    private bool mSliding = false;
    private bool mHasCollided = false;

    // Update is called once per frame
    void Update()
    {
        if (CanWalk() && !mMoving)
        {
            mDir.y = 0.0f;
            mDir.x = Input.GetAxisRaw("Horizontal");
            if (mDir.x == 0.0f)
                mDir.y = Input.GetAxisRaw("Vertical");

            if (mDir.sqrMagnitude > 0.0f)
            {

                Vector3 dir = new Vector3(mDir.x, mDir.y);
                mTarget = WalkMap.WorldToCell(transform.position + dir);
                mMoving = true;
            }
        }
        else if ((mDir.x != 0.0f || mDir.y != 0.0f) && !CanWalk())
            mSliding = true;

        if(!mHasCollided && Colliding())
        {
            mTarget = SlideMap.WorldToCell(transform.position);
            mDir = new Vector2(0.0f, 0.0f);

            mSliding = false;
            mHasCollided = true;
            mMoving = false;
        }
        else if(mHasCollided)
        {
            mDir.y = 0.0f;
            mDir.x = Input.GetAxisRaw("Horizontal");
            if (mDir.x == 0.0f)
                mDir.y = Input.GetAxisRaw("Vertical");

            if(mDir.sqrMagnitude > 0.0f)
            {
                mHasCollided = false;
                mMoving = true;
                mSliding = !CanWalk();
            }
        }

        Move();
        animator.SetFloat("horizontal", mDir.x);
        animator.SetFloat("vertical", mDir.y);
        animator.SetFloat("Speed", mDir.sqrMagnitude);
        animator.SetBool("Sliding", mSliding);
    }

    private bool CanWalk()
    {
        Vector3Int pos = WalkMap.WorldToCell(transform.position);
        return WalkMap.HasTile(pos);
    }

    private bool Colliding()
    {
        Vector3 dir = new Vector3(mDir.x, mDir.y);
        Vector3Int pos = SlideMap.WorldToCell(transform.position);
        Vector3Int NextPos = ObstacleMap.WorldToCell(transform.position + dir * 0.5f);
        Vector3 DownX = new Vector3(mDir.x, -1.0f).normalized;
        Vector3Int DownNext = SlopeMap.WorldToCell(transform.position + DownX * 0.5f);
        return ObstacleMap.HasTile(NextPos) ||
            (SlideMap.HasTile(pos) && ElevatedMap.HasTile(NextPos) && !SlopeMap.HasTile(pos)) ||
            (ElevatedMap.HasTile(pos) && SlideMap.HasTile(NextPos) && !SlopeMap.HasTile(DownNext)) ||
            (mDir.y != 0.0f && SlopeMap.HasTile(NextPos));
    }

    private void Move()
    {
        Vector3 target = WalkMap.GetCellCenterWorld(mTarget);
        Vector3 dir = new Vector3(mDir.x, mDir.y);
        if (mMoving)
        {
            Vector2 offset = transform.TransformDirection(dir * Speed * Time.deltaTime);
            transform.position += new Vector3(offset.x, offset.y);
        }

        float dist = Vector3.Distance(target, transform.position);
        if (dist <= 0.05f)
        {
            transform.position = target;
            mTarget = WalkMap.WorldToCell(transform.position);

            if (!SlideMap.HasTile(mTarget) && !ElevatedMap.HasTile(mTarget))
                mMoving = false;
            else
                mTarget = SlideMap.WorldToCell(transform.position + dir);

            Vector3Int pos = WalkMap.WorldToCell(transform.position);
            if (WalkMap.HasTile(pos))
            {
                mSliding = false;
                if(!WalkMap.HasTile(mTarget))
                    mDir = new Vector2(0.0f, 0.0f);
            }
        }
    }
}
