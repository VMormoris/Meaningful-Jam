using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TestScript : MonoBehaviour
{
    public TileBase Hole;
    
    public Tilemap WalkMap;
    public Tilemap SlideMap;
    public Tilemap ObstacleMap;
    public Tilemap SlopeMap;
    public Tilemap ElevatedMap;
    public Tilemap BreakableMap;
    public Tilemap DeathMap;
    public Tilemap CrackedMap;
    public Tilemap HoleMap;

    public GameObject BreakingSnowball;
    public GameObject BloodSplatter;
    public GameObject UICanvas;
    
    public Animator animator;

    public float Speed = 5.0f;
    public float FallingSpeed = 0.1f;

    public Transform Movables;

    public Vector2 mPrevDir = new Vector2(0.0f, 1.0f);
    public Vector2 mDir;
    public Vector3Int mTarget;
    public bool mMoving = false;
    public bool mSliding = false;
    public bool mHasCollided = false;
    public bool mSlopeUp = false;
    public bool mSlopeDown = false;
    public bool mCanBreak = false;
    public bool mIsDead = false;

    // Update is called once per frame
    void Update()
    {
        if (IsTouchingDeathTrap())
        {
            if(!mIsDead)
                Instantiate(BloodSplatter, transform);
            mIsDead = true;
            UICanvas.GetComponent<PauseMenu>().DeathMenu();
            return;
        }
        else if (mIsDead)
        {
            transform.localScale -= new Vector3(1.0f, 1.0f) * FallingSpeed * Time.deltaTime;
            if (transform.localScale.x <= 0.0f)
                gameObject.SetActive(false);
            UICanvas.GetComponent<PauseMenu>().DeathMenu();
            return;
        }


        float angle = -45.0f;
        if ((mDir.x > 0.0f && mSlopeUp) || (mDir.x < 0.0f && mSlopeDown))
            angle = 45.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, IsOnSlope() ? angle : 0.0f);

        if (CanWalk() && !mMoving)
        {
            if(mDir.sqrMagnitude > 0.0f)
                mPrevDir = mDir;
            mDir.y = 0.0f;
            mDir.x = Input.GetAxisRaw("Horizontal");
            if (mDir.x == 0.0f)
                mDir.y = Input.GetAxisRaw("Vertical");
            mCanBreak = false;
            if (mDir.sqrMagnitude > 0.0f)
            {
                Vector3 dir = new Vector3(mDir.x, mDir.y);
                if (!Colliding())
                {
                    mTarget = WalkMap.WorldToCell(transform.position + dir);
                    mMoving = true;
                }
            }
        }
        else if ((mDir.x != 0.0f || mDir.y != 0.0f) && !CanWalk())
            mSliding = true;

        if(Colliding() && !mHasCollided)
        {
            mTarget = SlideMap.WorldToCell(transform.position);
            mPrevDir = mDir;
            mDir = new Vector2(0.0f, 0.0f);

            mSliding = false;
            mHasCollided = true;
            mMoving = false;
        }
        else if(mHasCollided)
        {
            if (mDir.sqrMagnitude > 0.0f)
                mPrevDir = mDir;
            mDir.y = 0.0f;
            mDir.x = Input.GetAxisRaw("Horizontal");
            if (mDir.x == 0.0f)
                mDir.y = Input.GetAxisRaw("Vertical");

            mCanBreak = false;
            if (mDir.sqrMagnitude > 0.0f && !Colliding())
            {
                mHasCollided = false;
                mMoving = true;
                mSliding = !CanWalk();
            }
        }

        PushInRange();
        Move();

        animator.SetFloat("prevHorizontal", mPrevDir.x);
        animator.SetFloat("prevVertical", mPrevDir.y);
        animator.SetFloat("horizontal", mDir.x);
        animator.SetFloat("vertical", mDir.y);
        animator.SetFloat("Speed", mDir.sqrMagnitude);
        animator.SetBool("Sliding", mSliding);
    }

    private void Move()
    {
        float speed = Speed * (mCanBreak ? 1.5f : 1.0f);
        Vector3 target = WalkMap.GetCellCenterWorld(mTarget);
        Vector3 dir = new Vector3(mDir.x, mDir.y);
        if (mMoving)
        {
            Vector2 offset = transform.TransformDirection(dir * speed * Time.deltaTime);
            transform.position += new Vector3(offset.x, offset.y);
            if(mCanBreak && !IsOnSlope())
            {
                int y = (int)transform.position.y;
                transform.position = new Vector3(transform.position.x, y + (y <= 0 ? -0.5f : 0.5f));

                Vector3Int pos = BreakableMap.WorldToCell(transform.position);
                if(BreakableMap.HasTile(pos))
                {
                    BreakableMap.SetTile(pos, null);
                    Instantiate(BreakingSnowball, BreakableMap.GetCellCenterWorld(pos), Quaternion.Euler(0.0f, 0.0f, 0.0f));
                }
            }
        }

        float dist = Vector3.Distance(target, transform.position);
        if (dist <= 0.0899999999999f)
        {
            transform.position = target;
            Vector3Int pos = WalkMap.WorldToCell(transform.position);

            if (HoleMap.HasTile(pos))
                mIsDead = true;

            if (WalkMap.HasTile(mTarget) || CrackedMap.HasTile(mTarget))
            {
                mTarget = WalkMap.WorldToCell(transform.position);
                mMoving = false;
                Vector3Int prev = CrackedMap.WorldToCell(transform.position - dir);
                if(CrackedMap.HasTile(prev) && dir.sqrMagnitude > 0.0f)
                {
                    CrackedMap.SetTile(prev, null);
                    HoleMap.SetTile(prev, Hole);
                }
            }
            else
                mTarget = SlideMap.WorldToCell(transform.position + dir);

            Vector3Int DownNext = mTarget + new Vector3Int(0, -1);
            if (SlopeMap.HasTile(mTarget))
                mTarget += new Vector3Int((int)mDir.x, 1);
            else if (ElevatedMap.HasTile(pos) && SlideMap.HasTile(mTarget) && SlopeMap.HasTile(DownNext))
                mTarget += new Vector3Int((int)mDir.x, -1);

            if (WalkMap.HasTile(pos) || CrackedMap.HasTile(pos))
            {
                mSliding = false;
                if (!WalkMap.HasTile(mTarget) && !CrackedMap.HasTile(mTarget))
                {
                    mPrevDir = mDir;
                    mDir = new Vector2(0.0f, 0.0f);
                }
            }

            mSlopeUp = false;
            mSlopeDown = false;
        }

        if(!mMoving)
        {
            int y = (int)transform.position.y;
            transform.position = new Vector3(transform.position.x, y + (y <= 0 ? -0.5f : 0.5f));
        }
    }

    private bool IsOnSlope()
    {
        Vector3Int pos = SlopeMap.WorldToCell(transform.position);
        Vector3 dir = new Vector3(mDir.x, mDir.y);
        Vector3 offset = transform.TransformDirection(dir * 0.64f);
        Vector3Int aux = SlopeMap.WorldToCell(transform.position - offset);
        Vector3Int prev = ElevatedMap.WorldToCell(transform.position - dir);
        Vector3Int Down = SlopeMap.WorldToCell(transform.position + new Vector3(0.0f, -0.5001f));

        if ((SlopeMap.HasTile(pos) || SlopeMap.HasTile(aux)) && mDir.x != 0.0f && !mSlopeDown)
        {
            mSlopeUp = true;
            return true;
        }
        else if((ElevatedMap.HasTile(prev) && SlopeMap.HasTile(Down)) && mDir.x != 0.0f)
        {
            mSlopeDown = true;
            mCanBreak = true;
            return true;
        }
        else
            return false;
    }

    private bool Colliding()
    {
        Vector3 dir = new Vector3(mDir.x, mDir.y);
        Vector3Int pos = SlideMap.WorldToCell(transform.position);
        Vector3Int NextPos = ObstacleMap.WorldToCell(transform.position + dir * 0.5001f);
        Vector3 DownX = new Vector3(mDir.x, -1.0f).normalized;
        Vector3Int DownNext = SlopeMap.WorldToCell(transform.position + DownX);
        return ObstacleMap.HasTile(NextPos) ||
            (SlideMap.HasTile(pos) && ElevatedMap.HasTile(NextPos) && !SlopeMap.HasTile(pos) && !mSlopeUp) ||
            (ElevatedMap.HasTile(pos) && SlideMap.HasTile(NextPos) && !SlopeMap.HasTile(DownNext)) ||
            (mDir.y != 0.0f && SlopeMap.HasTile(NextPos)) ||
            (BreakableMap.HasTile(NextPos) && !mCanBreak);
    }

    private bool CanWalk()
    {
        Vector3Int pos = WalkMap.WorldToCell(transform.position);
        return WalkMap.HasTile(pos) || CrackedMap.HasTile(pos);
    }

    private bool IsTouchingDeathTrap()
    {
        Vector3 direction = new Vector3(mDir.x, mDir.y, 0.0f).normalized;
        Vector3Int pos = DeathMap.WorldToCell(transform.position + direction * 0.5f);
        return DeathMap.HasTile(pos);
    }

    private void PushInRange()
    {
        const float MinDistance = 1.0f;//1^2
        foreach(Transform child in Movables)
        {
            if(QuickDistance(child.position, transform.position) <= MinDistance)
            {
                Movable movable = child.GetComponent<Movable>();
                movable.Push(mDir);
            }
        }
    }

    private float QuickDistance(Vector2 a, Vector2 b)
    {
        float x = a.x - b.x;
        float y = a.y - b.y;
        return x * x + y * y;
    }

}
