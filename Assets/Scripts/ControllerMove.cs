using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ControllerMove : MonoBehaviour
{
    public float Speed = 0.0f;
    public float JumpSpeed = 0.0f;
    public float Gravity = 0.0f;
    public Animator anim;
    public AudioClip[] FootstepSounds;

    [SerializeField] private float overallSpeed;
    [SerializeField] private bool jump;

    private Vector3 moveDirection;
    public CharacterController _char;
    public AudioSource Audio;


    private void Start()
    {
        //_char = GetComponent<CharacterController>();
        //Audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
    	overallSpeed = _char.velocity.magnitude;		//вычесляем общую склрость
    	anim.SetFloat("Speed", ((overallSpeed/Speed)*100) );		//задаём скорость анимации бега

        if(_char.isGrounded)		// если игрок на земле
        {
        	if(anim.GetBool("Jump") && jump)		//включена анимация прыжка и прыжок активен
        	{
        		anim.SetBool("Jump",false);		//выключаем анимацию
        		jump=false;		//выключаем прыжок
        	}
        	if(Input.GetKey(KeyCode.LeftShift))		//если нажата кнопка шифт
        	{
	            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")) * (Speed*1.4f);		//скорость в 1.4 раза выше
	        }
	        else		//если шифт не нажат
	        {
	        	moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")) * Speed;		//обычная скорость
	        }
            moveDirection = transform.TransformDirection(moveDirection);		//вектор3 переводится в мирвые кординаты

            if(overallSpeed>0 && ( Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical")!=0 ))		//если скорость больше 0 и нажата какая-нибудь кнопка
            {
            	anim.SetBool("Run",true);		//анимация бека вкл
            }
            else		//если нет
            {
            	anim.SetBool("Run",false);		//бег выкл
            }

            if(Input.GetKey(KeyCode.Space))//если нажат прыжок
            {
            	jump=true;		//прыжок вкл
                moveDirection.y += JumpSpeed;		//скорость назначена
            }
        }
        else		//если игрок в полёте
        {
            moveDirection.y -= Gravity * Time.deltaTime;		// игрок опускается на землю
            if(overallSpeed>0 && jump)		//если игрок двигается в полёте и включён прыжок
            {
            	anim.SetBool("Run",false);		//выключаем бег
            	anim.SetBool("Jump",true);		//включаем прыжок
            }
        }
        _char.Move(moveDirection * Time.deltaTime);		//двигаем карактер контроллер со сглаживанием
    }


    public void Step()		//эта функция запустается Аниматором в определённый момент анимации
    						//и делает эта функция звуки шагов, когда нога ступает примерно на землю
    {    	
    	if (!_char.isGrounded)
            {
                return;
            }
            int n = ((Random.Range(1, FootstepSounds.Length))-1);		
            //Audio.clip = FootstepSounds[n];
            Audio.PlayOneShot(FootstepSounds[n]);
    }
}
