using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class PlayerMove : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float leftRightSpeed = 4f;
    static public bool canMove = true;
    public bool isJumping = false;
    public bool falling = false;
    public GameObject playerObject;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Muerte", false);
    }
    void Update()
    {

        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.World);
        if (canMove == true)
        {
            if (Input.GetKey(KeyCode.LeftArrow) && this.gameObject.transform.position.x > -3.5)
            {
                transform.Translate(Vector3.left * Time.deltaTime * leftRightSpeed);

            }
            if (Input.GetKey(KeyCode.RightArrow) && this.gameObject.transform.position.x < 3.5)
            {
                transform.Translate(Vector3.left * Time.deltaTime * -leftRightSpeed);
            }
            if(Input.GetKey(KeyCode.UpArrow))
            {
                if (isJumping == false)
                {
                    isJumping = true;
                    anim.SetBool("Salto", true);
                    //aqui iria la animacion de salto

                    //===========================
                    StartCoroutine(jumpSequence());
                }
            }
            if (isJumping == true)
            {
                if (falling == false)
                {
                    transform.Translate(Vector3.up * Time.deltaTime * 7, Space.World);
                }
                if (falling == true)
                {


                    transform.Translate(Vector3.up * Time.deltaTime * -7, Space.World);
                }

            }
        }
        else
        {
            anim.SetBool("Muerte", true);
        }

    }

    IEnumerator jumpSequence()
    {
        yield return new WaitForSeconds(0.8f);
        falling = true;
       
        isJumping = false;
        falling = false;
        anim.SetBool("TocoSuelo", true);
        anim.SetBool("Salto",false);
        yield return new WaitForSeconds(0.8f);
    }
}
