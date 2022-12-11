using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movable : MonoBehaviour
{
    public Tilemap SlopeMap;
    public Tilemap DeathTrapMap;

    public GameObject BreakingSnowball;

    public float AngularVelocity = 25.0f;
    public float Speed = 10.0f;

    private Vector2 mDir;
    private bool mMoving = false;
    
    // Update is called once per frame
    void Update()
    {
        if (IsTouchingDeathTrap())
        {
            Instantiate(BreakingSnowball, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
            Vector3Int pos = DeathTrapMap.WorldToCell(transform.position);
            DeathTrapMap.SetTile(pos, null);
            Destroy(gameObject);
        }

        Move();
    }

    private bool IsOnSlope()
    {
        Vector3Int down = SlopeMap.WorldToCell(transform.position + new Vector3(0.0f, -0.5001f));
        return SlopeMap.HasTile(down);
    }

    private void Move()
    {
        if (!mMoving)
            return;

        //Direction picking
        float angle = -45.0f;
        if (IsOnSlope())
            angle = 45.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, IsOnSlope() ? angle : 0.0f);

        //Move
        Vector3 dir = new Vector3(mDir.x, mDir.y);
        Vector3 offset = transform.TransformDirection(dir * 0.64f);
        transform.position += offset * Speed * Time.deltaTime;

        //Rotate child
        Transform child = transform.GetChild(0);
        child.rotation = Quaternion.Euler(0.0f, 0.0f, child.rotation.eulerAngles.z + AngularVelocity * Time.deltaTime);
        child.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

    private bool IsTouchingDeathTrap()
    {
        Vector3Int pos = DeathTrapMap.WorldToCell(transform.position);
        return DeathTrapMap.HasTile(pos);
    }

    public void Push(Vector2 dir)
    {
        mMoving = true;
        mDir = dir;
    }

}
