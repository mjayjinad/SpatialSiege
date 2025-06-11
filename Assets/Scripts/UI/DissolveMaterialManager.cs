using System.Collections;
using UnityEngine;

public class DissolveMaterialManager : MonoBehaviour
{
    [SerializeField] private float transitionDuration = 2f;

    private Material material;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        //StartMaterialDissolve(false);
        GameEventManager.Instance.OnWaveEndedEventForMaterial += StartMaterialDissolve;
        GameEventManager.Instance.OnEnemyReachedTargetPosEvent += StartMaterialDissolve;
    }

    public void StartMaterialDissolve(bool value)
    {
        StartCoroutine(DissolveTransition(value));
    }

    private IEnumerator DissolveTransition(bool isReversing)
    {
        float timeElapsed = 0f;
        float startValue = isReversing ? 1f : 0f;  // Start at 1 if reversing, else 0
        float endValue = isReversing ? 0f : 1f;    // End at 0 if reversing, else 1

        while (timeElapsed < transitionDuration)
        {
            timeElapsed += Time.deltaTime;
            float lerpFactor = timeElapsed / transitionDuration;

            // Lerp between startValue and endValue
            float dissolveValue = Mathf.Lerp(startValue, endValue, lerpFactor);

            // Set the lerp factor of the shader
            material.SetFloat("_Dissolve", dissolveValue);

            yield return null;
        }

        // Ensure the final texture is fully transitioned
        material.SetFloat("_Dissolve", endValue);
    }
}
