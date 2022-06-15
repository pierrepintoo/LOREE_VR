using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ManagerParticle : MonoBehaviour
{
    public VisualEffect PortalVfx;
    public string PortalVfxValue = "Arc";
    public VisualEffect AuroreVfx;
    public string AuroreVfxValue = "Arc";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int CenterX = Screen.width / 2;
        Vector3 mousePos = Input.mousePosition;
       
        if (PortalVfx != null)
        {

            float valuePortal = 0f;

            if(mousePos.x < CenterX)
            {
                float Current = Mathf.Clamp(mousePos.x, 0f, CenterX);
                Current = (3.14f / CenterX) * Current;
                AuroreVfx.SetFloat("Arc", Current);
                PortalVfx.SetFloat("Arc", 6.28f - Current);
                Debug.Log($"DISPLAY : {6.28f - Current}");
            }
            else
            {
                float Current = Mathf.Clamp(mousePos.x, CenterX, CenterX * 2);
                Current = (3.14f / CenterX) * Current;

                AuroreVfx.SetFloat("Arc",Current);
                PortalVfx.SetFloat("Arc", 3.14f - (Current / 2));
                Debug.Log($"DISPLAY : { 3.14f - (Current / 2)}");
            }

        }
    }
}
