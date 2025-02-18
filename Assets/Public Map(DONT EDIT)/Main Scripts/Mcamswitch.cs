using System.Collections;
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

    // Movement and Timing
    public float moveTime = 1f;
    private bool isCamAtTarget = false;
    private bool isTransitioning = false;

    void Start()
    {
        // Initialize camera position
        videocamera.transform.position = camStartingPoint.position;
        DisableNightVision();

        // Retrieve and store default post-processing values
        if (vol != null && vol.profile != null)
        {
            if (vol.profile.TryGet(out lensDistortion))
                defaultLensDistortion = lensDistortion.intensity.value;


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
        if (Input.GetKeyDown(KeyCode.Q) && !isTransitioning)
        {
            StartCoroutine(SwapCameraWithEffects());
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
        }

        isTransitioning = false;
    }

    private IEnumerator MoveCamera()
    {
        Vector3 start = isCamAtTarget ? camTargetPoint.position : camStartingPoint.position;
        Vector3 end = isCamAtTarget ? camStartingPoint.position : camTargetPoint.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / moveTime;
            videocamera.transform.position = Vector3.Lerp(start, end, progress);
            yield return null;
        }

        videocamera.transform.position = end;
        isCamAtTarget = !isCamAtTarget;
    }

    private void EnableNightVision()
    {
        isNightVision = true;
        videocamera.SetActive(false);
        monster.enabled = !isNightVision;

        if (nightvisionlight != null)
            nightvisionlight.intensity = 15f;

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
        videocamera.SetActive(true);
        isNightVision = false;
        monster.enabled = !isNightVision;

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
                }
            }
        }
    }

}
