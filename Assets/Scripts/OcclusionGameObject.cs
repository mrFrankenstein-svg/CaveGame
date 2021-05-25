using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class OcclusionGameObject : MonoBehaviour
{
	private OcclusionCamera iocCam;
	private Vector3 hitPoint;
	private float shadowDistance;
	private int currentLayer;
	private bool realtimeShadows;
	private Renderer[] rs;
	private bool hidden;
	private bool sleeping;
	private RaycastHit h;
	private UnityEngine.Rendering.ShadowCastingMode[] originalShadowCastingMode;
	private int counter;
	private int frameInterval;
	private Ray r;
	private bool visible;
	private Vector3 p;

	void Awake () 
	{
		Init();
	}
	
	public void Init () 
	{
		try
		{
			iocCam =  Camera.main.GetComponent<OcclusionCamera>();
			currentLayer = gameObject.layer;
			//prevDist = 0f;
			//prevHitTime = Time.time;
			sleeping = true;
			h = new RaycastHit();
			this.enabled = true;
		}
		catch(Exception e)
		{
			this.enabled = false;
			Debug.Log(e.Message);
		}
	}


    void Start()
    {    	
    	realtimeShadows = iocCam.realtimeShadows;
        rs = GetComponentsInChildren<Renderer>(false).Where(x => x.gameObject.GetComponent<Light>() == null).ToArray();
		originalShadowCastingMode = new UnityEngine.Rendering.ShadowCastingMode[rs.Length];
		for(int i=0;i<rs.Length;i++)
		{
			originalShadowCastingMode[i] = rs[i].shadowCastingMode;
		}
		Initialize();
    }


	public void Initialize() 
	{
		if(iocCam.enabled == true)
		{
			HideAll();
		}
		else
		{
			this.enabled = false;
			ShowLod(1);
		}
	}

    void Update()
    {
        frameInterval = Time.frameCount % 4;
        if(frameInterval == 0)
		{
	        if(!hidden && Time.frameCount - counter > iocCam.hideDelay)
					{
						visible = rs[0].isVisible;					
						
						if(visible)		//стреляет дальше, чтобы выключить обьекты сзади
						{
							p = transform.localToWorldMatrix.MultiplyPoint(hitPoint);
							r = new Ray(p, iocCam.transform.position - p);
							if(Physics.Raycast(r, out h, iocCam.viewDistance))
							{
								if(!h.collider.CompareTag(iocCam.tag) && rs[0].enabled == true)
								{
									Hide();
								}
								else
								{
									counter = Time.frameCount;
								}
							}
						}
						else
						{
							Hide();
						}
					}
	    }
	    else if(realtimeShadows && frameInterval == 2)
		{
	//			distanceFromCam = Vector3.Distance(transform.position, iocCam.transform.position);
				if(hidden)
				{
					if(rs[0].enabled)
					{
						for(int i=0;i<rs.Length;i++)
						{
							rs[i].enabled = false;
							rs[i].shadowCastingMode = originalShadowCastingMode[i];
						}
					}
					else
					{
						if(!rs[0].enabled)
						{
							for(int i=0;i<rs.Length;i++)
							{
								rs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
								rs[i].enabled = true;
							}
						}
					}
				}
		}
		
	}	


    /*public void UpdateValues() 
    {
    	realtimeShadows = iocCam.realtimeShadows;
    }*/


    public void UnHide(RaycastHit h)
	{
		counter = Time.frameCount;
		hitPoint = transform.worldToLocalMatrix.MultiplyPoint(h.point);
		if(hidden)
		{
			hidden = false;
			ShowLod(h.distance);			
		}
	}



	public void ShowLod(float d)
	{
		int i = 0;
		if(rs[0].enabled)
		{
			for(i=0;i<rs.Length;i++)
			{
				rs[i].shadowCastingMode = originalShadowCastingMode[i];
			}
		}
		else
		{
			for(i=0;i<rs.Length;i++)
			{
				rs[i].enabled = true;
			}
		}
	}



	public void Hide()
	{
		int i = 0;
		hidden = true;
		if(realtimeShadows)
			{
				for(i=0;i<rs.Length;i++)
				{
					rs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
				}
			}
			else
			{
				for(i=0;i<rs.Length;i++)
				{
					if(rs[i].enabled != false)
					rs[i].enabled = false;
				}
			}
	}

	public void HideAll()
	{
		int i = 0;
		hidden = true;
		if(rs != null)
		{
			for(i=0;i<rs.Length;i++)
			{
				rs[i].enabled = false;
				if(realtimeShadows)
				{
					rs[i].shadowCastingMode = originalShadowCastingMode[i];
				}
			}
		}
	}
}
