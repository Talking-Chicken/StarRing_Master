using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTransform : MonoBehaviour
{
    public bool DoDistortion = true;
    public bool DoTransform = false;
    [SerializeField] private Material TransformMaterial;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    private float BlenderShapeKey = 0;
    [SerializeField] private float ChangeSpeed = 1;
    private int ChangeDirection = 1;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        StartCoroutine(EnableDistortion());
        DoTransform = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (DoTransform)
        {
            InvokeRepeating("TransformWeapon", 0, 0.1f);
        }
    }

    private void TransformWeapon()
    {
        if (meshRenderer.GetBlendShapeWeight(0) < 0 )
        {
            DoTransform = false;
            ChangeDirection *= -1;
            meshRenderer.SetBlendShapeWeight(0, 0);
            CancelInvoke("TransformWeapon");
        }
        if (meshRenderer.GetBlendShapeWeight(0) > 100)
        {
            DoTransform = false;
            ChangeDirection *= -1;
            meshRenderer.SetBlendShapeWeight(0, 100);
            CancelInvoke("TransformWeapon");
        }
        BlenderShapeKey += ChangeSpeed * ChangeDirection;
        meshRenderer.SetBlendShapeWeight(0, BlenderShapeKey);

    }
    IEnumerator EnableDistortion()
    {
        if (DoDistortion)
        {
            TransformMaterial.SetInt("_TurnOnDistortion", 1);
            float a = Random.Range(0.1f, 0.4f);
            TransformMaterial.SetFloat("_DistortionAmount", a);
            int x = Random.Range(10, 500);
            int y = Random.Range(10, 500);
            Vector2 RandomSeed = new Vector2(x, y);
            TransformMaterial.SetVector("_RandomSeed", RandomSeed);
            yield return new WaitForSeconds(.1f);
            TransformMaterial.SetInt("_TurnOnDistortion", 0);
        }
        else
        {
            TransformMaterial.SetInt("_TurnOnDistortion", 0);
        }

        float waittime = Random.Range(0.1f, 0.5f);
        yield return new WaitForSeconds(waittime);
        StartCoroutine(EnableDistortion());
    }
}
