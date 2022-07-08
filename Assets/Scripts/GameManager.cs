using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float playerSens = 1f;
    public int points = 0;
    public int maxTargets = 6;

    public Text sens;

    public GameObject targetPrefab;
    public GameObject mainTargetPrefab;
    public GameObject playerRef;

    public Text sensText;
    public Text pointText;
    public Text prevAdjusText;

    private Vector3 mainPos = new Vector3(0.23f, 0.77f, 8.29f);
    private Vector3 mainPosScreen;
    private Vector2 overshoot;
    private Vector3 targetPos;
    private Vector3 targetPosScreen;
    private GameObject targetRef;
    private GameObject mainRef;
    private float prevAdjus = 0f;
    private bool started = false;

    public bool targetMode = false;
    private bool targetModeOnce = false;

    public void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    public void Update()
    {
        // Shoot Target
        if (Input.GetMouseButtonDown(0) && started)
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
            if (Physics.Raycast(ray, out raycastHit, 100f, LayerMask.GetMask("Target")))
            {
                if (raycastHit.transform != null)
                {
                    ClickedGameObject(raycastHit.transform.gameObject);
                }

                return;
            }
            else if (targetMode)
            {
                targetPosScreen = Camera.main.WorldToScreenPoint(targetRef.transform.position);
                mainPosScreen = Camera.main.WorldToScreenPoint(mainPos);

                // overshoot based on crosshair pos
                overshoot = new Vector2((Screen.width / 2) - targetPosScreen.x, (Screen.height / 2) - targetPosScreen.y);
                points--;

                AdjustSens();
                UpdateStats();
                GenerateMainTarget();
                Destroy(targetRef);
            }
            else if (targetModeOnce){
                mainPosScreen = Camera.main.WorldToScreenPoint(targetPos);
                targetPosScreen = Camera.main.WorldToScreenPoint(mainPos);
                
                // overshoot based on crosshair pos
                overshoot = new Vector2((Screen.width / 2) - targetPosScreen.x, (Screen.height / 2) - targetPosScreen.y);
                points--;

                AdjustSens();
                UpdateStats();
                GenerateTarget();
                Destroy(mainRef);
                
            }

            
        }
    }

    private void ClickedGameObject(GameObject go)
    {

        
        if (go.tag == "MainTarget")
        {
            GenerateTarget();
        }
        else {
            GenerateMainTarget();
        }

        points++;
        UpdateStats();
        Destroy(go);
    }

    private void AdjustSens()
    {
        // avg x and y adjus, if adjus is negative, sens is too slow. Vice versa for positive.
        Debug.Log(targetPosScreen);
        Debug.Log(mainPosScreen);
        float adjustment = (overshoot.x / (targetPosScreen.x - mainPosScreen.x)) / 2f + (overshoot.y / (targetPosScreen.y - mainPosScreen.y)) / 2f;
        adjustment = Mathf.Clamp(adjustment * 0.01f, -0.1f, 0.1f);
        prevAdjus = adjustment;
        playerSens *= (1 - adjustment);
    }

    public void GenerateTarget()
    {
        // Generate some random spot infront of the player, generate after a target is destroyed
        float x, y, z;
        float px = playerRef.transform.position.x;
        float py = playerRef.transform.position.y;
        float pz = playerRef.transform.position.z;

        x = Random.Range(-7f, 7f) + px;
        y = Random.Range(-2f, 2f) + py;
        z = mainPos.z;

        targetRef = Instantiate(targetPrefab, new Vector3(x, y, z), Quaternion.identity);
        overshoot = Vector2.zero;
        targetPos = targetRef.transform.position;
        targetMode = true;
        targetModeOnce = true;
    }

    public void GenerateMainTarget()
    {
        mainRef = Instantiate(mainTargetPrefab, mainPos, Quaternion.identity);
        targetMode = false;
    }

    public void UpdateStats()
    {
        sensText.text = "Sens: " +  playerSens.ToString();
        pointText.text = "Points: " + points.ToString();
        prevAdjusText.text = "Adjusment: " + (prevAdjus * -100).ToString() + "%"; 
    }

    public void Go()
    {
        playerSens = float.Parse(sens.text);
        started = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}
