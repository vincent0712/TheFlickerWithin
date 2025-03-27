using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering.Universal;

public class Mcamswitch : MonoBehaviour
{
    // Cameras
    public GameObject videocamera;
    public Camera playerCamera;
    public GameObject flashlight;
    public MeshRenderer monster;
    public SkinnedMeshRenderer monsterrenderer;
    public TextMeshProUGUI batterytext;
    public bool camison = false;
    public float battery = 100f;
    public bool isCameraUnlocked;



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

    // Movement and Timing
    public float moveTime = 1f;
    private bool isCamAtTarget = false;
    private bool isTransitioning = false;

    void Start()
    {

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isTransitioning && isCameraUnlocked && canuse)
        {
            StartCoroutine(SwapCameraWithEffects());
            
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
                batterytext.text = "Battery: " + battery;
                
            }
            else
            {
                
            }
        }
    }



    private IEnumerator SwapCameraWithEffects()
    {
        isTransitioning = true;

        DisableNightVision();
        
        yield return StartCoroutine(MoveCamera());

        if (isCamAtTarget)
        {
            
            EnableNightVision();
            FindObjectOfType<PaintingToggle>().TogglePaintings(true);
            batterytext.enabled = true;
            camison = true;
            
        }
        else
        {
            camison = false;
            
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
        batterytext.enabled = isNightVision;
        monsterrenderer.enabled = isNightVision;
        




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
        batterytext.enabled = false;
        videocamera.SetActive(true);
        isNightVision = false;
        batterytext.enabled = isNightVision;
        monsterrenderer.enabled = isNightVision;
        


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
