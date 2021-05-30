using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CaveCreator : MonoBehaviour
{
	public int misTry;		//счётчик неудачных установок
	public bool creationFinished;		//индикатор завершения созданя
	//public bool fullingFinished;		// индикатор гаполнния пещеры обьектами
	public bool Occlusion;				// выключатель оклюжигна на время разработки игры

	private int state;	//счётчик состояния скрипта
	public NavMeshSurface navMesh; //
	public GameObject[] caveSpavnPoint;	//хранилище найденых точек установки частей пещеры
	public GameObject[] spavnPoint;		//хронилище найденых точек установки всякого в пещере
	public GameObject[] spawnObjects;	//устанавливаемые обьекты
	public GameObject[] wallSpawnObjects; //такие же обьекты для стен (светильиники ьам всякие, корни....)
	public GameObject[] florSpawnObjects; //такие же обьекты для пола (светильиники ьам всякие, корни....)
	public GameObject[] spawnRums;	//обьект для выбора окончаний тонелей (комнаты или тупики)
	public GameObject[] spawnedObj;	//установленные обьекты
	public GameObject spawn;	//обьект для выбора рондомного кусочка
	public int caveLong;	// задаём длинну пещеры


	public int numberOfWallSpawnPoints;
	public int I=0;	//счётчик длинны пещеры
	GameObject Instantiat;	//надо подумать как без этого

	Vector3 startPointPos;		//запомнить стартовую позицию
	Quaternion startPointRot;		//и направление
	
	bool Started;		//для гизмо для инспектора. Удалить по окончании!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

	//чтобы не создавать новые переменные во время работы кода
	//создаём переменные, в кторые будут сохранятся переменные колайдёра
	//для проверки столкновений с другими коллайдёрами
	Vector3 positionOfCave;		//сохраняем позицию
	Vector3 sizeOfCave;			//и размер

    void Start()
    {
    	navMesh=GameObject.Find("NavMesh").GetComponent<NavMeshSurface>();	// ищем НавМеш
        spawnedObj= new GameObject[(caveLong+10)];	//устанавливаем длинну массива
        Started = true;		//это штука для работы гизмо в инспекторе. Удалить!!!!!!!!!!!!!!!!!!!!!!!!!
        startPointPos=GameObject.FindWithTag("CaveSpawnPoint").transform.position;		//запоминаем стартовую позицию
        startPointRot=GameObject.FindWithTag("CaveSpawnPoint").transform.rotation;		//и направление обьекта, из которого будет начинаться пещера
        state=1;
    }

    void Update()
    {
    	switch(state)
    	{
    		case 1:
			    	if(misTry>25)		//если ошибок больше 25
			    	{
			    		misTry=0;		//обнуляем счётчик ошибок
			    		GameObject star = new GameObject();		//создаём новый обьект в сцене. Да, от этого все эти непонятные "New Game Objeсt". Больше не использую такую нострукцию
			    		star.transform.position=startPointPos;		//задаём обьекту нужную позицию
			    		star.transform.rotation=startPointRot;		//и напавление
			    		star.tag = "CaveSpawnPoint";		//и нужный таг тоже

			    		for(int i=0; i<spawnedObj.Length-1; i++)		//циклом удаляем все уже созданные обьекты
			    		{
			    			Destroy(spawnedObj[i]);
			    		}

			    		for(int i=0; i<caveSpavnPoint.Length; i++)		//циклом удаляем все точки спавна
			    		{
			    			//Debug.Log(i);
			    			Destroy(caveSpavnPoint[i]);    				
			    		}


			    		spawnedObj= new GameObject[caveLong+10];		//обуляем массив созданных обьектов чтоб наверняка
			    		I=0;		//ну и обнуляем счётчик длинны пещеры
			    	}
			    	else		//если ошибок не больше 25, то
			    	{
			    		caveSpavnPoint = GameObject.FindGameObjectsWithTag("CaveSpawnPoint");	//ищем все точки для спавна кусочков пещер
				    	if(caveSpavnPoint.Length >0)	//если ещё надо ставить куски то...
				    	{
				    		spawn=caveSpavnPoint[Random.Range(0,(caveSpavnPoint.Length-1))];	//берём случайный кусочек

						    if(caveSpavnPoint.Length +I <= caveLong)		//если не достаточно проходов и не нужно ставить комнаты
						    {
					    		Instantiat=GameObject.Instantiate(spawnObjects[Random.Range(0,spawnObjects.Length)],spawn.transform.position,spawn.transform.rotation) as GameObject;	//ставим кусочек
						    }
						    else		//если достаточно проходов и нужно ставить комнаты
						    {
						    	Instantiat=GameObject.Instantiate(spawnRums[Random.Range(0,spawnRums.Length)],spawn.transform.position,spawn.transform.rotation) as GameObject;	//ставим комнаты
						    }



				//Это проверка и надо она только сейчас (24.05.2021 22:17)
						    if(Instantiat.GetComponent<Renderer>())
						    {
						   		var bounds = Instantiat.GetComponent<Renderer>().bounds;		//штука для нахождения центра и длинны
						    	Debug.Log(bounds);
						    	/*
						    	var Cube = new GameObject();
            					Cube.transform.position =bounds.center;		//эта хрень ставит пустышку в центре
            					*/
							}


						    /*Следуюцая часть нужна для того, чтобы переименовать устанавливаемый обьект.
							Возможно это не нужно будет для готового варианта, но пока пусть будет*/
							//////////////////////////////////////////////////////////////////////////

						    spawnedObj[I]=Instantiat;		//записываем установленный кусочек в массив
						    string nam=I.ToString();		//переводим цифры в буквы
						    spawnedObj[I].name= nam+ " " +spawnedObj[I].name ;		//переименовываем установленный кусочек в номер

							//////////////////////////////////////////////////////////////////////////


					    
					        positionOfCave=spawnedObj[I].transform.Find("box").transform.position;		//из только что установленного кусочка выдераем обьект, который размерами как комната, только это простой квадрат
					        sizeOfCave=spawnedObj[I].transform.Find("box").GetComponent<Collider>().bounds.size;		//выдераем из квадрата его размер в мировых кординатах

					        spawnedObj[I].SetActive(false);		//основной же обьект скрываем на время проверки
					        Collider[] colliders = Physics.OverlapBox(positionOfCave, sizeOfCave/2);		//собственно, делаем проверку

										/*for(int i=0; i<colliders.Length; i++)		
								    		{   		
						        				Debug.Log((i+1)+" "+colliders[i]);		
								    		}*/


					        if(colliders.Length>1)		//если проверка показала, что мы чего-то касаемся, то...
					        {
					        	misTry++;		//+1 в ошибки

					        	//и ругаемся в консоль

					        	Debug.Log("..................................." + misTry + " errors. Now with:");

					        	for(int i=0; i<colliders.Length; i++)		//и перечисляем всё, до чего касаемся
								    		{   		
						        				Debug.Log(colliders[i]);		
								    		}

					        	Destroy(spawnedObj[I]);		//удаляем установленный обьект
					        }
					        else		//если проверка показала, что мы ничего не касаемся, то...
					        {
					        	misTry=0;		//обнуляем счётчик ошибок
					        	spawnedObj[I].SetActive(true);		//включаем наш главный обьект
					        	spawnedObj[I].tag="forOccluding";		        
					        	Destroy(spawnedObj[I].transform.Find("box").gameObject);		//удаляем уже ненужный квадрат
					        	Destroy(spawn);	//удаляем точку спавна
					        	CreateObjectsOnSurface(spawnedObj[I],florSpawnObjects,numberOfWallSpawnPoints);		//Добавляем к обьекту скрипт, который делает точки на стенах
					        	I++;	//прибавляем 1 к счётчику
					        }
					    }
					    else //если больше ничего ставить не надо
					    {
					    	state++;		//переключаем кейс
					    }
					}
    		break;
    		case 2:
    				Spawn("CaveWallPoint",wallSpawnObjects,true,false);
					/*wallSpawnPoint = GameObject.FindGameObjectsWithTag("CaveWallPoint");	//ищем точки установки в стенах
					if(wallSpawnPoint.Length >0)	//если ещё надо ставить куски то...
			    	{
			    		spawn=wallSpawnPoint[Random.Range(0,(wallSpawnPoint.Length-1))];	//берём случайный кусочек
			    		Instantiat=GameObject.Instantiate(wallSpawnObjects[Random.Range(0,wallSpawnObjects.Length)],spawn.transform.position,spawn.transform.rotation)
			    		 as GameObject;	//ставим кусочек
			    		Instantiat.transform.parent = spawn.transform.parent;
			    		Instantiat.transform.Rotate(Random.Range(-30,30),Random.Range(-30,30),Random.Range(-30,30), Space.Self);
			    		Destroy(spawn);	//удаляем точку спавна
			    	}
			    	else
			    	{
			    		state=3;
			    	}*/
    		break;
    		case 3:
    				Spawn("CaveFlorPoint",florSpawnObjects, false,true);
    		break;
    		case 4:
    				if(Occlusion==true)
    				{
    					End();
    				}
    					NavMesh();
    					state++;
    		break;
    		default:

    		break;
    	}

    }




    void Spawn(string tag, GameObject[] objects, bool scale, bool rotate)
    {
    	spavnPoint = GameObject.FindGameObjectsWithTag(tag);	//ищем точки установки в стенах
		if(spavnPoint.Length >0)	//если ещё надо ставить куски то...
			  {
			  	spawn=spavnPoint[Random.Range(0,(spavnPoint.Length-1))];	//берём случайный кусочек
			  	Instantiat=GameObject.Instantiate(objects[Random.Range(0,objects.Length)],spawn.transform.position,spawn.transform.rotation)
			  	as GameObject;	//ставим кусочек
			  	if(scale ==true)
			  	{
			  		if(Instantiat.tag != "Light")
			  		{
			  			Instantiat.transform.localScale= Instantiat.transform.localScale*5;
			  		}
			  	}
			  	Instantiat.transform.parent = spawn.transform.parent;
			  	if(rotate==true)
			  	{
			  		Instantiat.transform.Rotate(Random.Range(-30,30),Random.Range(0,360),Random.Range(-30,30), Space.Self);
			  	}
			  	else
			  	{
			  		Instantiat.transform.Rotate(Random.Range(-30,30),Random.Range(-30,30),Random.Range(-30,30), Space.Self);
			  	}

			  	Destroy(spawn);	//удаляем точку спавна
			  }
			   else
			  {
			  	state++;
			  }
    }


    void CreateObjectsOnSurface( GameObject processedObject, GameObject[] objectsToAdd, int numberOfObjectsToAdd)
    {
  		processedObject.AddComponent<SpawningOnTheSurface>();		//добавляем отекту скрипт
  		processedObject.GetComponent<SpawningOnTheSurface>().numberOfSpawnPoints=numberOfObjectsToAdd;		//задаём значение в скрипте
  		processedObject.GetComponent<SpawningOnTheSurface>().оbjectsForAddOnSurface=objectsToAdd;		//передаём обьекты в скрипт
    }



    void End()
    {
    	for(int i=0; i<spawnedObj.Length; i++)
    	{
    		if(spawnedObj[i]!=null && !(spawnedObj[i].GetComponent<OcclusionGameObject2>()) )
    		{
    			spawnedObj[i].AddComponent<OcclusionGameObject2>() ;
    		}
    		//Debug.Log("Ячейка № " + i + ", обьект с именем: " + spawnedObj[i]);
    	}
    	Started=false;
    }




    void NavMesh()
    {
    	navMesh.GetComponent<NavMeshSurface>().BuildNavMesh();	// делаем меш для NPC
    }




    void OnDrawGizmos()		//эта штака нужна только для гизмо в инспекторе. Удалить!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        Gizmos.color = Color.red;
        if (Started)
            Gizmos.DrawWireCube(positionOfCave, sizeOfCave);
    }
}