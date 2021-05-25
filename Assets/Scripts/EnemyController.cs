using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class EnemyController : MonoBehaviour
{
	/*
		План на контроллер:
		 ЕСТЬ! если он видит- он в стрессе
		если он слышит- он в стрессе
		 ЕСТЬ! если он в стрессе и видит- он бежит
		если он в стрессе и слишит- он бежит к точке, которую слышал
		 ЕСТЬ если он в стрессе, рядом и может атаковать- он атакует
		 ЕСТЬесли может атаковать во время бега- атакует
		если он бегит и видит- он стреляет
		если он видит и расстояние больше (условно) 30 метров- он прячется и стреляет
		если рядом есть оружие(условно в 3-х шагах)- он может бросить оружие в игрока, перед этим как-бы издавая боевой клич
	*/

	//public FirstPersonController controler;
	public float playerHealt=100;  //собственно, и сказать тут ещё нечего

	public int lenght=0;	//это счётчик листа, который постоянно передвигается и считает врагов
	public Transform player;	//это чтобы двигаться
	public Transform camer;		//это чтобы считать поподания
	public List<GameObject> enemyList;//обьявил Лист
	public Enemy enemy;		//выдернутый скрипт,через который будет происодить вся дичь с ботом
	private RaycastHit hit;		// это чтоы считать взгляд врага и попадания
	public float memberTime=1f;		// это задаёт время, через которое бот забудет игрока
	public LayerMask EnemySee;
	
	void Start()
	{
		
	}

	void Update()
	{
		if(playerHealt>0)
		{
			if(lenght<enemyList.Count)//проверка счётчика листа
			{
				enemy=enemyList[lenght].GetComponent<Enemy>();//выдрали скрипт из текущего обьекта листа
				if(enemy.stress==true)
				{	
					Debug.DrawLine (enemy.seePoint.transform.position, camer.transform.position,Color.red);//рисуем линию чтобы видеть линию взора
					if(Physics.Linecast(enemy.seePoint.transform.position, camer.transform.position, out hit, ~EnemySee))//проверяем есть ли что-нибудь на линии обзора
					{
						if(hit.collider.tag=="Player")//если на линиии обзора только игрок
       					{
	       					enemy.timeOfMemberIsOn=false;
	       					enemy.member=true;//ставим отметку в боте, что бот плмнит игрока
	       					StartCoroutine(enemy.Run(player));//запускаем коротину бега

		       				if((enemyList[lenght].transform.position - player.position).sqrMagnitude<=10)//если бот близко,чтобы бить, то
		        			{
		        				//Debug.Log((enemyList[lenght].transform.position - player.position).sqrMagnitude);
		        				if(enemy.attack!=true)
		        				{
		        					StartCoroutine(enemy.AttackStart());
		        				}
		        			}
		        			else
		        			{
		        				if(enemy.attack!=true)
		        				{
		        					StartCoroutine(enemy.AttackStop());
		        				}
		        			}
	        			}
        				else//если не видно игрока,
        				{
        					if(enemy.member==true)// но бот помнит про игрока
        					{
        					if(enemy.timeOfMemberIsOn==false)
        					{
        						enemy.timeOfMemberIsOn=true;//запускаем время, через которое бот забудет игрока
        						enemy.timeOfMember= memberTime;//задаём время, через которое бот забудет игрока
        					}
       							StartCoroutine(enemy.Run(player));//бежим за игроком, пока не забудем его
        					}
        				}        				
        			}

        			if((enemyList[lenght].transform.position - player.position).sqrMagnitude<=6)//если бот достатчно близко, чтоб стоять, то
        			{
        				enemy.run=false;
        			}
        			else
        			{
        				if((enemyList[lenght].transform.position - player.position).sqrMagnitude>=8)//если игрок немного отдалился
        				{
        					enemy.run=true;
        				}
        			}
				}
				else
				{	
					if(Physics.Linecast(enemy.seePoint.transform.position, camer.transform.position, out hit))//проверяем есть ли что-нибудь на линии обзора
					{
						if(hit.collider.tag=="Player")//если на линиии обзора только игрок
       					{
       						enemy.stress=true;
        				}
        			}
				}
				lenght++;//в последнюю очередь делается
			}
			if(lenght>=enemyList.Count)//обнуление счётчика листа если счётчик дошёл до максимума
			{
				lenght=0;
			}
		}
		else
		{
			StartCoroutine(PlayerDead());
		}
	}
    	private IEnumerator PlayerDead()	//не, ну тут всё понятно. СМЭРТ
    	{
    		//yield return new WaitForSeconds();
    		Destroy(gameObject.GetComponent<FirstPersonController>());
    		Destroy(gameObject.GetComponent<CharacterController>());
    		GetComponent<CapsuleCollider>().enabled=true;
    		GetComponent<Rigidbody>().isKinematic=false;
    		GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * Random.Range(-5f, 5f), ForceMode.Impulse);    		
    		GetComponent<Rigidbody>().AddRelativeForce(Vector3.left * Random.Range(-5f, 5f), ForceMode.Impulse);
    		Destroy(this);
    		yield break;
    	}
}
