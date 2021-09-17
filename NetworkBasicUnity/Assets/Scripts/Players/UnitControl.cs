using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControl : MonoBehaviour
{
    const float speed = 3.0f;

    public bool bMoveable = false;

    GameManager gm;
    Vector3 targetPos;
    Vector3 orgPos;

    float timeToDest;
    float elapsed;

    bool bMoving;

    private void Start()
    {
        //sm = GameObject.Find("GameManager").GetComponent<SocketModule>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        orgPos = transform.position;
        targetPos = orgPos;
        timeToDest = 0;
        bMoving = false;
    }

    private void Update()
    {
        if(bMoving)
        {
            elapsed += Time.deltaTime;
            if(elapsed >= timeToDest)
            {
                elapsed = timeToDest;
                transform.position = targetPos;
                bMoving = false;
            }
            else
            {
                Vector3 newPos = Vector3.Lerp(orgPos, targetPos, elapsed / timeToDest);
                transform.position = newPos;
            }
        }
    }

    public void SetTargetPos(Vector3 pos)
    {
        orgPos = transform.position;

        targetPos = pos;
        targetPos.z = orgPos.z;
        timeToDest = Vector3.Distance(orgPos, targetPos) / speed; // S = vt 공식
        elapsed = 0;
        bMoving = true;
    }

    public void SetColor(Color col)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr)
        {
            sr.color = col;
        }
    }

}
