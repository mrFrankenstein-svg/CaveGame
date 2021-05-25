using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	public NavMeshAgent agent;
	public bool stress;
	public float hp;
	public bool jump;
	public bool attack=false;
	public bool run=true;
	public Animator anim;
	public GameObject seePoint;
	public EnemyController player;

	public bool member=false;
	public bool timeOfMemberIsOn=false;
	public float timeOfMember;
    void Start()
    {
    	agent= GetComponent<NavMeshAgent>();//хватаем агента в переменную
        player=GameObject.FindGameObjectWithTag("Player").GetComponent<EnemyController>();
        player.enemyList.Add(gameObject);//добавляем этого бота в список в контроллере
    }

    void Update()
    {
        if(timeOfMemberIsOn==true)//если включено, бот считает время, через которое забудет игрока
        {
        	Debug.Log("I member you ");//пишем в лог, что этот процесс запущен

        	timeOfMember -= Time.deltaTime; //Вычитаем из  по секунде
			if(timeOfMember <= 0)//Время вышло 
			{
				timeOfMemberIsOn=false;
				StartCoroutine(Member());
			}
        }
        anim.SetFloat("HP", hp);
        
    }

    public void Hit()		//эта функция запустается Аниматором в определённый момент анимации
    {
    	//Debug.Log("HIT");
    	//player.playerHealt-=7;
    }

    	public IEnumerator Run(Transform target)//то, что заставляет бота бежать к игроку
    	{
    		if(run==true)
    		{
    			agent.isStopped = false;//запускаем агента, если он был до этого остановлен
    			agent.SetDestination(target.position);//ставим цель для агента
       			anim.SetBool("Run",true);//запускаем анимацию
    		}
    		else
    		{
    			agent.isStopped = true;//тормозим агента
    			anim.SetBool("Run",false);//запускаем анимацию
    		}
              Debug.Log("I see you");//пишем в лог, что видим игрока. Надо будет потом удалить
    		yield break;
    	}
    	public IEnumerator Member()//это выключатель памяти для бота
    	{
    		  Debug.Log("I forgot you.");//пишем в лог
    		anim.SetBool("Run",false);//убираем анимаци
    		anim.SetBool("Attack",false);//тоже убираем анимации
    		member=false;//убираем проверку, пмнит ли бот игрока (теперь не помнит. Это надо для контроллера)
    		agent.isStopped = true;//тормозим агента
    		yield break;
    	}
    	public IEnumerator AttackStart()
    	{
    		attack=true;
    		anim.SetBool("Attack",true);
        	//run=false;
        	yield return new WaitForSeconds(0.30f);
    		attack=false;
    		yield break;
    	}
    	public IEnumerator AttackStop()
    	{
    		anim.SetBool("Attack",false);
    		//run=true;
    		//attack=false;
    		yield break;
    	}
}
