using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MultiCenterRevealController : MonoBehaviour
{
    public GameObject[] targets = new GameObject[4];
    public float radius = 1.0f;
    public float revealAmount = 1.0f;

    public Material material;

    // Assign your camera here or use Camera.main by default
    public Camera cam;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        if (cam == null) return;

        material.SetFloat("_Radius", radius);
        material.SetFloat("_RevealAmount", revealAmount);

        Vector4[] centers = new Vector4[4];

        for (int i = 0; i < 4; i++)
        {
            if (i < targets.Length && targets[i] != null)
            {
                Vector3 viewportPos = cam.WorldToViewportPoint(targets[i].transform.position);

                // Clamp to [0,1] range or set to zero if behind camera
                if (viewportPos.z > 0) // in front of camera
                    centers[i] = new Vector4(viewportPos.x, viewportPos.y, 0, 0);
                else
                    centers[i] = Vector4.zero;
            }
            else
            {
                centers[i] = Vector4.zero;
            }
        }

        material.SetVector("_Center0", centers[0]);
        material.SetVector("_Center1", centers[1]);
        material.SetVector("_Center2", centers[2]);
        material.SetVector("_Center3", centers[3]);
    }
}
