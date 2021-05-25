using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class OcclusionGameObject2 : MonoBehaviour
{
	private OcclusionCamera iocCam;	
	public Renderer[] rs;
	private Light[] rs_Light;
	public float hideDelay;

	void Awake () 
	{
		Init();
	}
	public void Init () 
	{
		try
		{
			iocCam =  Camera.main.GetComponent<OcclusionCamera>();
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
    	hideDelay=iocCam.hideDelay;
    	rs = GetComponentsInChildren<Renderer>(false).Where(x => x.gameObject.GetComponent<Light>() == null).ToArray();
    	if(iocCam.light==true)
    	{
    		rs_Light=GetComponentsInChildren<Light>();
    	}
    	for(int i=0;i<rs.Length;i++)
			{
				if(rs[i]!= null)
				{
					rs[i].enabled = true;
				}
			}


		for(int i=0;i<rs_Light.Length;i++)
			{
				if(rs_Light[i]!= null)
				{
					rs_Light[i].enabled = true;
				}
			}
    }

    void Update()
    {
    	hideDelay-=Time.deltaTime;
    	if(hideDelay<=0)
    	{
			if(iocCam.light==true)
			{
				for(int i=0;i<rs_Light.Length;i++)
				{
					if(rs_Light[i]!= null)
					{
						rs_Light[i].enabled = false;
					}
				}
			}
    		for(int i=0;i<rs.Length;i++)
			{
				if(rs[i]!= null)
				{
					rs[i].enabled = false;
				}
			}
			Destroy(this);
    	}
    }

    public void UnHide(RaycastHit h)
	{

	}
}
