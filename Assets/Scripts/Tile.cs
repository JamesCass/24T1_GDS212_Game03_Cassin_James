using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public AudioClip pressSound;

    private Material firstMaterial;
    private Material secondMaterial;

    private Quaternion currentRotation;

    [HideInInspector]
    public bool Revealed = false;
    private TileManager tileManager;
    private bool clicked = false;
    private int index;

    private bool isRotating = false;   

    private AudioSource audio;

    public void SetIntex(int id) { index = id; }
    public int GetIndex() { return index; }

    // Start is called before the first frame update
    void Start()
    {
        Revealed = false;
        clicked = false;
        tileManager = GameObject.Find("[TileManager]").GetComponent<TileManager>();
        currentRotation = gameObject.transform.rotation;

        audio = GetComponent<AudioSource>();
        audio.clip = pressSound;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        if (clicked == false && !isRotating)
        {
            StopAllCoroutines();
            isRotating = true;
            tileManager.CurrentPuzzleState = TileManager.PuzzleState.PuzzleRotating;
            if (GameSettings.Instance.IsAudioMutedPermanently() == false)
            {
                audio.Play();
            }
            StartCoroutine(LoopRotation(45, false));
            clicked = true;

           
        }
        
    }

    public void FlipBack()
    {
        if (gameObject.activeSelf)
        {
            tileManager.CurrentPuzzleState = TileManager.PuzzleState.PuzzleRotating;
            Revealed = false;
            if (GameSettings.Instance.IsAudioMutedPermanently() == false)
            {
                audio.Play();
            }
            StartCoroutine(LoopRotation(90, true));
        }
    }

    IEnumerator LoopRotation(float angle, bool FirstMat)
    {
        var rot = 0f;
        const float dir = 1f;
        const float rotSpeed = 180.0f;
        const float rotSpeed1 = 180.0f;
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
            clicked = false;
            isRotating = false;
            yield return null;
        }

        gameObject.GetComponent<Transform>().rotation = currentRotation;

        if(!FirstMat)
        {
            Revealed = true;
            ApplySecondMaterial();
            tileManager.CheckTile();
        }

        else
        {
            tileManager.PuzzleRevealedNumber = TileManager.RevealedState.NoRevealed;
            tileManager.CurrentPuzzleState = TileManager.PuzzleState.CanRotate;
        }

        clicked = false;
        yield return null;
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

    public void Deactivate()
    {
        StartCoroutine(DeactivateCoroutine());
    }

    private IEnumerator DeactivateCoroutine()
    {
        Revealed = false;

        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
