using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCParticles : MonoBehaviour
{
    public ParticleSystem particleEffect;

    public Material dedgeMaterial;

    float krillTimer = 0.5f;

    private List<Material> material = new List<Material>();
    private List<Color> originalColor = new List<Color>();

    private Vector3 ogSize;

    void Start()
    {
        ogSize = transform.localScale;

        // Use a unique material instance to avoid affecting others
        foreach (Transform _child in transform)
        {
            foreach (Transform childObj in _child)
            {
                Renderer renderer = childObj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material mat = renderer.material;
                    material.Add(mat);
                    originalColor.Add(mat.color);
                    Debug.Log(mat.color);
                }
            }
        }
    }

    public void StartFade()
    {
        // Instantiate the particle effect at the object's position
        Instantiate(particleEffect, transform.position, Quaternion.identity);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(krillTimer);

        foreach (Transform _child in transform)
        {
            foreach (Transform childObj in _child)
            {
                Renderer renderer = childObj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = dedgeMaterial;
                }
            }
        }

        // float timer = 0f;
        // while (timer < krillTimer)
        // {
        //     for (int i = 0; i < material.Count; ++i)
        //     {
        //         float r = Mathf.Lerp(originalColor[i].r, 1f, timer / krillTimer);
        //         float g = Mathf.Lerp(originalColor[i].g, 1f, timer / krillTimer);
        //         float b = Mathf.Lerp(originalColor[i].b, 1f, timer / krillTimer);
        //         material[i].color = new Color(r, g, b, originalColor[i].a);
        //         timer += Time.deltaTime;
        //     }
        //     float size = timer / krillTimer;
        //     transform.localScale = new Vector3(Mathf.Lerp(ogSize.x, 0f, size), Mathf.Lerp(ogSize.y, 0f, size), Mathf.Lerp(ogSize.z, 0f, size));
        //     yield return null;
        // }

        // // Ensure it's fully transparent
        // for (int i = 0; i < material.Count; ++i)
        // {
        //     material[i].color = new Color(1f, 1f, 1f, 1f);
        // }

        // // Disable or destroy the object
        // gameObject.SetActive(false); // or Destroy(gameObject);
    }
}
