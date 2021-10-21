using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitControl : MonoBehaviour
{
    const float speed = 3.0f;
    const int MAX_HP = 100;
    const int DROP_HP = 4;

    public bool bMoveable = false;
    public ParticleSystem fxParticle;
    public GameObject hpBar;
    public Button revive;


    GameManager gm;
    Vector3 targetPos;
    Vector3 orgPos;

    float timeToDest;
    float elapsed;

    float elapsedDrop;
    int currentHP;
    int maxHP;

    bool bMoving;

    private void Start()
    {
        //sm = GameObject.Find("GameManager").GetComponent<SocketModule>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        orgPos = transform.position;
        targetPos = orgPos;
        timeToDest = 0;
        bMoving = false;

        maxHP = MAX_HP;
        currentHP = maxHP;
        elapsedDrop = Time.time;
    }

    private void Update()
    {
        // SetHP
        if(Time.time > elapsedDrop + 1.0f && currentHP > 0)
        {
            elapsedDrop = Time.time;
            currentHP -= DROP_HP;
            SetHP(currentHP);
        }

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

    public void DropHP(int drop)
    {
        currentHP -= drop;
        SetHP(currentHP);
    }

    public void StartFX()
    {
        if(currentHP > 0)
        {
            fxParticle.Play();
        }
    }

    public int GetHP()
    {
        return currentHP;
    }

    public void SetHP(int hp)
    {
        hp = Mathf.Clamp(hp, 0, maxHP);
        currentHP = hp;
        float ratio = (float)currentHP / maxHP;
        hpBar.transform.localScale = new Vector3(ratio, 1.0f, 1.0f);
    }

    public void SetColor(Color col)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr)
        {
            sr.color = col;
        }
    }

    public void Revive()
    {
        currentHP = maxHP;
        hpBar.transform.localScale = Vector3.one;
        elapsedDrop = Time.time;
    }


}
