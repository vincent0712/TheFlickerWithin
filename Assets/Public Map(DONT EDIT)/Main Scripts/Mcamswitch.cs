using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Mcamswitch : MonoBehaviour
{
    // Cameras
    public GameObject videocamera;
    public Camera playerCamera;
    public GameObject flashlight;
    public MeshRenderer monster;
    public SkinnedMeshRenderer monsterrenderer;
    
    public bool camison = false;
    public float battery = 100f;
    public bool isCameraUnlocked;
    public GameObject camerahud;
    public Image[] batteryBars;



    // Transforms
    public Transform camStartingPoint;
    public Transform camTargetPoint;

    // Post-Processing
    public Volume vol;
    private UnityEngine.Rendering.Universal.LensDistortion lensDistortion;
    private UnityEngine.Rendering.Universal.Vignette vignette;
    private UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments;
    private UnityEngine.Rendering.Universal.LiftGammaGain liftGammaGain;
    private UnityEngine.Rendering.Universal.FilmGrain filmgrain;

    // Stored Default Values
    private float defaultLensDistortion;
    private float defaultVignetteIntensity;
    private float defaultSaturation;
    private float defaultContrast;
    private Vector4 defaultGamma;
    private Vector4 defaultGain;

    // Night Vision Effects
    public AudioSource nightVisionToggleSound;
    private bool isNightVision = false;
    public Light nightvisionlight;
    private bool canuse = true;
    private Onscreentext onScreenText;

    // Movement and Timing
    public float moveTime = 1f;
    private bool isCamAtTarget = false;
    private bool isTransitioning = false;

    void Start()
    {
        onScreenText = FindObjectOfType<Onscreentext>();
        // Initialize camera position
        videocamera.transform.position = camStartingPoint.position;
        StartCoroutine(BatteryDrainLoop());

        DisableNightVision();


        // Retrieve and store default post-processing values
        if (vol != null && vol.profile != null)
        {
            if (vol.profile.TryGet(out lensDistortion))
                defaultLensDistortion = lensDistortion.intensity.value;
            lensDistortion.active = false;

            if (vol.profile.TryGet(out vignette))
                defaultVignetteIntensity = vignette.intensity.value;



            if (vol.profile.TryGet(out colorAdjustments))
            {
                defaultSaturation = colorAdjustments.saturation.value;
                defaultContrast = colorAdjustments.contrast.value;
            }

            if (vol.profile.TryGet(out liftGammaGain))
            {
                defaultGamma = liftGammaGain.gamma.value;
                defaultGain = liftGammaGain.gain.value;
            }
        }
    }
    void UpdateBatteryMeter()
    {
        int activeBars = Mathf.Clamp(Mathf.CeilToInt((battery - 10f) / 22.5f), 0, 4); // Adjust so 0 bars means 10%

        for (int i = 0; i < batteryBars.Length; i++)
        {
            batteryBars[i].enabled = i < activeBars; // Enable bars based on battery level
        }
    }

    void Update()
    {
        UpdateBatteryMeter();
        if (battery > 0 && !canuse)
        {
            canuse = true;
        }
        if (battery <= 0)
        {
            battery = 0;
            canuse = false; // Prevent using camera when battery is 0

            if (camison && !isTransitioning)
            {
                StartCoroutine(SwapCameraWithEffects()); // Force camera down
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && !isTransitioning && isCameraUnlocked && canuse)
        {
            
            StartCoroutine(SwapCameraWithEffects());
        }
        else if (Input.GetKeyDown(KeyCode.Q) && !isTransitioning && isCameraUnlocked && !canuse)
        {
            onScreenText.ShowText("Out Of Battery!", 2f);
        }
    }

    private IEnumerator BatteryDrainLoop()
    {
        while (true) // Run forever
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            if (camison && battery > 0)
            {
                battery -= 1;
                

                if (battery <= 0)
                {
                    battery = 0;
                    canuse = false;
                    StartCoroutine(SwapCameraWithEffects()); // Force camera down immediately
                }
            }
        }
    }

    private IEnumerator SwapCameraWithEffects()
    {
        isTransitioning = true;
        DisableNightVision();

        yield return StartCoroutine(MoveCamera());

        if (isCamAtTarget && battery > 0)
        {
            EnableNightVision();
            FindObjectOfType<PaintingToggle>().TogglePaintings(true);
            
            camison = true;
        }
        else
        {
            camison = false;
            canuse = battery > 0; // Only allow usage if battery is not empty
        }

        isTransitioning = false;
    }




    private IEnumerator MoveCamera()
    {
        Transform parentCamera = videocamera.transform.parent; // Get the main player's camera
        Vector3 start = isCamAtTarget ? camTargetPoint.localPosition : camStartingPoint.localPosition;
        Vector3 end = isCamAtTarget ? camStartingPoint.localPosition : camTargetPoint.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / moveTime;
            videocamera.transform.localPosition = Vector3.Lerp(start, end, progress); // Move locally
            yield return null;
        }

        videocamera.transform.localPosition = end; // Set final position
        isCamAtTarget = !isCamAtTarget;
    }



    private void EnableNightVision()
    {
        isNightVision = true;
        videocamera.SetActive(false);
        
        monsterrenderer.enabled = isNightVision;
        camerahud.SetActive(isNightVision);






        if (nightvisionlight != null)
            nightvisionlight.intensity = 1.75f;

        if (vol != null)
        {
            flashlight.SetActive(false);

            if (colorAdjustments != null)
            {
                colorAdjustments.active = true;
                liftGammaGain.active = true;

                if (vol.profile.TryGet(out filmgrain))
                {
                    filmgrain.intensity.value = 1f;
                    lensDistortion.active = true;
                    



                }

                

            }
        }

        if (nightVisionToggleSound)
        {
            nightVisionToggleSound.Play();
        }
    }


    private void DisableNightVision()
    {
        FindObjectOfType<PaintingToggle>().TogglePaintings(false);
        
        videocamera.SetActive(true);
        isNightVision = false;
        
        monsterrenderer.enabled = isNightVision;
        camerahud.SetActive(isNightVision);



        if (nightvisionlight != null)
            nightvisionlight.intensity = 0f;

        if (vol != null)
        {
            flashlight.SetActive(true);

            if (colorAdjustments != null)
            {
                colorAdjustments.active = false;
                liftGammaGain.active = false;
                if (vol.profile.TryGet(out filmgrain))
                {

                    filmgrain.intensity.value = 0.8f;
                    lensDistortion.active = false;
                    

                }
            }
        }
    }

}
