using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningOnTheSurface : MonoBehaviour
{
	public int numberOfSpawnPoints;		//Количество точек для спавна
	public GameObject[] оbjectsForAddOnSurface;		//то, что будет вставлятся
	private GameObject Instantiat;		//переменная, в которую будет устанавливаться обьект. Это чтобы не создавалось 10000000000 пустышек

  void Start()
    {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;

        						for (int i = 0; i < numberOfSpawnPoints; i++)
						        {
						        	//пока что это всё костыль для проверки 
						        	/*
						            GameObject wallPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
						            wallPoint.transform.localScale = Vector3.one * 0.3f;
						            Destroy(wallPoint.GetComponent<SphereCollider>());
						            wallPoint.transform.position = getRandomWorldPointOnMesh(mesh, transform);
						            wallPoint.transform.parent = gameObject.transform;
						            */
						            ////////////////////////////////////////////
						             Instantiat=GameObject.Instantiate(оbjectsForAddOnSurface[Random.Range(0,оbjectsForAddOnSurface.Length)],
						             gameObject.transform.position,gameObject.transform.rotation);
						             Instantiat.transform.position = getRandomWorldPointOnMesh(mesh, transform);
						        }
    }


  private Vector3 getRandomWorldPointOnMesh(Mesh mesh, Transform relativeTransform)
							    {
							        Vector3[] vertices = mesh.vertices;
							        int[] tris = mesh.triangles;

							        int randTriangle = 3 * Random.Range(0, tris.Length / 3);

							        Vector3 v0 = vertices[tris[randTriangle]];
							        Vector3 v1 = vertices[tris[randTriangle + 1]];
							        Vector3 v2 = vertices[tris[randTriangle + 2]];

							        Vector3 randOneToTwo = Vector3.Lerp(v1, v2, Random.Range(0f, 1f));
							        Vector3 randVec = Vector3.Lerp(v0, randOneToTwo, Random.Range(0f, 1f));

							        return relativeTransform.TransformPoint(randVec);
							    }

private Vector3 getPerpendicularPointOnMesh(Mesh mesh, Transform relativeTransform)
								{
									
								}

}
