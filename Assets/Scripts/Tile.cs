using System.Collections;
using System.Collections.Generic;
using System.Xml.Xsl;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Material firstMaterial;
    private Material secondMaterial;

    private Quaternion currentRotation;
    // Start is called before the first frame update
    void Start()
    {
        currentRotation = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        StartCoroutine(LoopRotation(45, false));
    }

    IEnumerator LoopRotation(float angle, bool FirstMat)
    {
        var rot = 0f;
        const float dir = 1f;
        const float rotSpeed = 180.0f;
        const float rotSpeed1 = 90.0f;
        var startAngle = angle;
        var assigned = false;


        if (FirstMat)
        {
            while (rot < angle)
            {
                var step = Time.deltaTime * rotSpeed1;
                gameObject.GetComponent<Transform>().Rotate(new Vector3(0, 2, 0) * step * dir);
                if(rot >= (startAngle - 2) && assigned == false)
                {
                    ApplyFirstMaterial();
                    assigned = true;
                }

                rot += (1 * dir); 
                yield return null;
            }
        }
        else
        {
            while (angle > 0)
            {
                float step = Time.deltaTime * rotSpeed1;    
                gameObject.GetComponent<Transform>().Rotate(new Vector3(0,2,0) * step * dir);  
                angle -= (1 * step * dir);
                yield return null;
            }
        }

        gameObject.GetComponent<Transform>().rotation = currentRotation;

        if(!FirstMat)
        {
            ApplySecondMaterial();
        }
    }

    public void SetFirstMaterial(Material mat, string texturePath)
    {
        firstMaterial = mat;
        firstMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
    }

    public void SetSecondMaterial(Material mat, string texturePath)
    {
        secondMaterial = mat;
        secondMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
    }

    public void ApplyFirstMaterial()
    {
        gameObject.GetComponent<Renderer>().material = firstMaterial;
    }

    public void ApplySecondMaterial()
    {
        gameObject.GetComponent<Renderer>().material = secondMaterial;
    }
}
