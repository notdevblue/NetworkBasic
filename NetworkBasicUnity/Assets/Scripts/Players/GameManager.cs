using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public GameObject prefabUnit;
    public GameObject mainChar;


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(!EventSystem.current.IsPointerOverGameObject()) // UI 위에 있을 시 true
            {
                Vector3 targetPos;

                targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mainChar.GetComponent<UnitControl>().SetTargetPos(targetPos);
            }
        }
    }
}
