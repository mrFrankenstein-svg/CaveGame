using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionCamera : MonoBehaviour
{
	//public LayerMask layerMsk;
	public int samples;
	public float raysFov;
	public float viewDistance;
	public int hideDelay;
	public bool realtimeShadows;
	public bool light;
	public int caseSwitch;

	private RaycastHit hit;
	private Ray r;
	//private int layerMask;
	private OcclusionGameObject2 iocComp;	
	private int haltonIndex;
	//public float haltonIndexPASS;
	private float[] hx;
	private float[] hy;
	private int pixels;
	private Camera cam;
	private Camera rayCaster;

	void Awake () 
	{
		cam = GetComponent<Camera>();
		hit = new RaycastHit();
	//	haltonIndexPASS = 0.04f;
		viewDistance=cam.farClipPlane;
		hideDelay=3;
	}

	void Start ()
	{
		pixels = Mathf.FloorToInt(Screen.width * Screen.height / 4f);
		hx = new float[pixels];
		hy = new float[pixels];
		for(int i=0; i < pixels; i++)
		{
			hx[i] = HaltonSequence(i, 2);
			hy[i] = HaltonSequence(i, 3);
		}
		rayCaster =cam;
	}

    void Update()
    {
    	for(int k=0; k <= samples; k++)
		{
			r = rayCaster.ViewportPointToRay(new Vector3(hx[haltonIndex], hy[haltonIndex], 0f));	
			haltonIndex++;
			if(haltonIndex >= pixels) haltonIndex = 0;
			if(Physics.Raycast(r, out hit, viewDistance))
			{
				Unhide(hit.transform, hit);
			}		
			//Debug.DrawRay(r.origin, hit.point ,Color.green, 0.01f);
		}
    }

    private void Unhide(Transform t, RaycastHit hit)
    {

		if(t.parent != null)
		{
			Unhide(t.parent, hit);
		}
		else if(t.GetComponent<OcclusionGameObject2>()==null && t.parent == null)
		{
			t.gameObject.AddComponent<OcclusionGameObject2>() ;
		}
		else if(t.GetComponent<OcclusionGameObject2>()!=null && t.parent == null)
		{
			t.GetComponent<OcclusionGameObject2>().hideDelay=hideDelay;
		}
	}
	private float HaltonSequence(int index, int b)
	{
		float res = 0f;
		float f = 1f / b;
		int i = index;
		while(i > 0)
		{
			res = res + f * (i % b);
			i = Mathf.FloorToInt(i/b);
			f = f / b;
		}
		return res;
	}
}
