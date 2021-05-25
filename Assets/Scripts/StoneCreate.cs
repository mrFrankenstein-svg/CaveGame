using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneCreate : MonoBehaviour
{
	public GameObject[] AllRocks;
	public GameObject[] Items;
	public GameObject flor;
	private GameObject Instantiat;
	public GameObject navMesh;
	public OffMeshLink[] off;    
	//public GameObject[] end;
    void Start()
    {
    	
    	flor.transform.localScale= new Vector3 (1,Random.Range(0.3f, 1f),1); // делаем нужный пол

		AllRocks = GameObject.FindGameObjectsWithTag("EPoints");    //ищем все точки для камней

		StartCoroutine("Kamni"); //запускаем коротину

    }

    private IEnumerator Kamni() 
    {
    	for (int i = AllRocks.Length; i > 0; i--)  
		{
		 Ray ray =new Ray (AllRocks[(i-1)].transform.position,AllRocks[(i-1)].transform.forward);	// стреляем лучом чтобы найти точки для камней
		 RaycastHit hit;
		 if(Physics.Raycast(ray,out hit))
		  {
		 	Instantiat= GameObject.Instantiate(Items[Random.Range(0,(Items.Length-1))],hit.point+ray.direction*0.1f,Quaternion.Euler(Random.Range(0,10),Random.Range(0,360),Random.Range(0,10))) as GameObject;
		 	Instantiat.transform.localScale= Instantiat.transform.localScale*5;	//ставим размер камней
		 	Instantiat.isStatic=true;	//включаем им статику
		  }
		  Destroy(AllRocks[(i-1)]);
		}
    	navMesh.GetComponent<NavMeshSurface>().BuildNavMesh();	// делаем меш для NPC

    	StartCoroutine("Jamping"); //запускаем коротину
    	yield return null;
    }


    private IEnumerator Jamping()
    {
    	AllRocks = GameObject.FindGameObjectsWithTag("Walkable");		// ищет все обьекты для прыжка через WALKABLE

    	foreach (GameObject obj in AllRocks)
    	{

			off= obj.GetComponents<OffMeshLink>();		//выдераем скрипт из обьектов с прыжком

			for  (int i=off.Length-1; i>-1; i--)		//применяем для всех точек
			{
				Ray ray =new Ray (off[i].endTransform.position, off[i].endTransform.forward);		//стреляем райкастом чтобы понять куда ставить точки для прыжка
				off[i].endTransform.parent=null;
				RaycastHit hit;
		    	if(Physics.Raycast(ray,out hit))
				{
					off[i].endTransform.position= hit.point;		//	ставим точки для прыжка
				}
			}
		 
	    }
		yield return null;
    }
}
