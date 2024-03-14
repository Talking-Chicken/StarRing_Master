using MeadowGames.UINodeConnect4;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(MindPalaceNode))]
public class MPNodeShaderControl : MonoBehaviour
{
    MindPalaceNode _node;

    Image image;
    Material _mat;

    Vector2 lastPos;

    public float speedRatio;
    public float maxMoveSpeed;
    float randomSeed;
    [Range(0.01f, 10)] public float glitchSpeed;

    private float glitchSpeedInfluencer = 1;
    public MindPalaceNode MindPalaceNode
    {
        get
        {
            if (!_node)
                _node = GetComponent<MindPalaceNode>();
            return _node;
        }
        set => _node = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        randomSeed = Random.value;
        _mat = Instantiate(image.material);
    }

    // Update is called once per frame
    void Update()
    {

        bool onDragging = MindPalaceNode.GetOnDragging();

        if (onDragging)
        {
            Vector2 pos = transform.localPosition;

            float speed = (pos - lastPos).magnitude * Time.deltaTime;
            //Debug.Log(speed);
            speed = Mathf.Min(maxMoveSpeed, speed * speedRatio);

            Vector2 direction = (pos - lastPos).normalized;
            //Debug.Log(direction);

            _mat.SetFloat("_MovingSpeed", speed);
            _mat.SetVector("_MovingDirection", direction);

            lastPos = pos;
        }

        bool onReacting = MindPalaceNode.Reacting;
        _mat.SetFloat("_DoGlitch", (onReacting) ? 1 : 0);
        _mat.SetFloat("_DoSquareGlitchInside", (onReacting && MindPalaceNode.ReactingProgress > .5f) ? 1 : 0);
        if (onReacting)
        {
            glitchSpeedInfluencer = Mathf.Lerp(.1f, 1f, MindPalaceNode.ReactingProgress);
            float timeSeed = Mathf.Round(Time.time * glitchSpeed * glitchSpeedInfluencer);
            _mat.SetFloat("_RandomSeed", timeSeed + randomSeed);
        }

        image.material = _mat;
    }
}
