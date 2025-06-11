using UnityEngine;
using UnityEngine.Splines;

public class SplineFollowerEmir : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float speed = 1f;

    private float totalDistanceMoved = 0f;
    public bool isFollowing = false;
    private Vector3 lastSplinePosition;
    private Vector3 startPosition = Vector3.zero;

    public bool lookAt = false;

    public ParticleSystem particleSysts;

    void Start()
    {
        if (particleSysts) particleSysts.gameObject.SetActive(false);

        if (splineContainer != null)
        {
            startPosition = transform.position;
            lastSplinePosition = splineContainer.EvaluatePosition(0f);

            if (lookAt) euler = transform.eulerAngles;
        }
    }

    private Vector3 euler = Vector3.zero;

    void Update()
    {
        if (isFollowing && splineContainer != null)
        {
            float splineLength = splineContainer.CalculateLength();

            totalDistanceMoved += speed * Time.deltaTime;

            // Wrap around for looping
            if (totalDistanceMoved > splineLength)
                totalDistanceMoved -= splineLength;

            float t = Mathf.Clamp01(totalDistanceMoved / splineLength);

            Vector3 currentSplinePos = splineContainer.EvaluatePosition(t);
            Vector3 delta = currentSplinePos - lastSplinePosition;

            if (lookAt)
            {
                var targetRot = Quaternion.LookRotation((currentSplinePos - lastSplinePosition).normalized);
                transform.localRotation = Quaternion.Lerp(transform.rotation, targetRot, 15f * Time.deltaTime);
            }

            transform.position += delta;
            lastSplinePosition = currentSplinePos;
        }
    }

    public void FollowSpline()
    {
        if (splineContainer == null) return;

        if (particleSysts) particleSysts.gameObject.SetActive(true);

        totalDistanceMoved = 0f;
        lastSplinePosition = splineContainer.EvaluatePosition(0f);
        startPosition = transform.position;
        isFollowing = true;
    }

    public void ResetAndStop()
    {
        if (particleSysts) particleSysts.gameObject.SetActive(false);

        isFollowing = false;
        totalDistanceMoved = 0f;

        if (startPosition == Vector3.zero) startPosition = transform.position;

        if (euler == Vector3.zero && lookAt) euler = transform.eulerAngles;

        transform.position = startPosition;
        lastSplinePosition = splineContainer.EvaluatePosition(0f);

        if (lookAt) transform.eulerAngles = euler;
    }
}
