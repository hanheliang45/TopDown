using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigController : MonoBehaviour
{
    [SerializeField] private Rig rig;
    [SerializeField] private float rigIncreaseSpeed = 4f;
    
    [SerializeField] private TwoBoneIKConstraint leftHandIK;

    private bool _shouldIncreaseWeight;
    private bool _shouldIncreaseWeightForLeftHandIK;
    
    void Awake()
    {
        _shouldIncreaseWeight = false;
        // Prioritize();
    }

    void Update()
    {
        if (_shouldIncreaseWeight)
        {
            rig.weight += rigIncreaseSpeed * Time.deltaTime;
        }
        if (rig.weight >= 1.0f)
        {
            _shouldIncreaseWeight = false;
        }

        if (_shouldIncreaseWeightForLeftHandIK)
        {
            leftHandIK.weight += rigIncreaseSpeed * Time.deltaTime;
        }
        if (leftHandIK.weight >= 1.0f)
        {
            _shouldIncreaseWeightForLeftHandIK = false;
        }
    }

    public void Deprioritize()
    {
        rig.weight = 0.15f;
    }

    public void Prioritize()
    {
        Debug.Log("Prioritize");
        _shouldIncreaseWeight = true;
    }

    public void DeprioritizeLeftHandIK()
    {
        this.Deprioritize();
        leftHandIK.weight = 0.15f;
    }
    
    public void PrioritizeLeftHandIK()
    {
        Debug.Log("PrioritizeLeftHandIK");
        this.Prioritize();
        _shouldIncreaseWeightForLeftHandIK = true;
    }
}
