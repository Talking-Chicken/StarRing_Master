using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Highlighters;

[RequireComponent (typeof(Highlighter))]
public class AddOutline : MonoBehaviour
{
    public bool enableHighlight = true;
    public Color OutlineColor = Color.white;
    public float OutlineThickness = 5;

    private Highlighter m_highlighter;


    void Start()
    {
        m_highlighter = GetComponent<Highlighter>();
        m_highlighter.Setup();
        SetHighlight();
        m_highlighter.enabled = enableHighlight;
    }

    private void Update()
    {
        SetHighlight();
        m_highlighter.enabled = enableHighlight;
    }
    private void SetHighlight()
    {
        m_highlighter.Settings.UseMeshOutline = true;
        m_highlighter.Settings.MeshOutlineFront.Color = OutlineColor;
        m_highlighter.Settings.MeshOutlineBack.Color = OutlineColor;
        m_highlighter.Settings.MeshOutlineThickness = OutlineThickness * transform.localScale.x / 5000;
        m_highlighter.Settings.MeshOutlineAdaptiveThickness = 1;
    }
}
