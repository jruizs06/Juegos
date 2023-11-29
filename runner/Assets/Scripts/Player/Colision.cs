using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Colision : MonoBehaviour
{
    public GameObject thePlayer;
    void OnTriggerEnter(){
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        thePlayer.GetComponent<PlayerMove>().enabled = false;
        PlayerMove.canMove=false;
    }
}
