using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class Fluffy : MonoBehaviour
{
    [Header("Shell Info")]
    [SerializeField] private Renderer fluffyRenderer;
    [SerializeField] private List<FluffyInfo> layers = new List<FluffyInfo>();

    [Space]
    [Header("Shell Variables")]
    [SerializeField, Range(1,100)] private int shellNumber = 16;
    [SerializeField] private float shellGap = 5f;

    [Space]
    [Header("Strand Variables")]
    [SerializeField] private Color colorTint;
    [SerializeField] private float strandDensity = 300;
    [SerializeField, Range(0, 1)] private float baseThickness = 1;
    [SerializeField, Range(0, 1)] private float tipThickness = 0;
    [SerializeField, Range(0, 1)] private float baseDarkness = 0.5f;

    [Space]
    [Header("Physics")]
    [SerializeField, Range(0.1f, 5f)] private float swayPower = 0.1f;
    [SerializeField] private float swayAmount = 1;
    [SerializeField, Range(0, 50)] private float gravityPower = 0;
    [SerializeField, Range(0, 0.5f)] private float randomHairDisplacement = 0.5f;
    
    private Material[] originalMaterials;

    //The main called upon function to create all the renderer materials/
    public void MakeFluffy()
    {
        if (fluffyRenderer == null) return;

        //Setup lists
        CleanSlate();
        originalMaterials = fluffyRenderer.sharedMaterials;
        Material[] newMaterials = new Material[shellNumber];
        layers = new List<FluffyInfo>(shellNumber);

        //Add new materials and property-blocks to each layer
        for (int i = 0; i < shellNumber; i++)
        {
            Material mat = new Material(originalMaterials[0]);
            newMaterials[i] = mat;
            layers.Add(new FluffyInfo(mat, new MaterialPropertyBlock()));
            UpdateShaderLayer(i);
        }
        fluffyRenderer.sharedMaterials = newMaterials;
        OnValidate();
    }

    //Update each material using property-block
    void UpdateShaderLayer(int i)
    {
        var layer = layers[i];
        if (layer.mpb == null) layer.mpb = new MaterialPropertyBlock();

        layer.mpb.SetFloat("_StrandHeight", (i * (shellGap + 1)) / 1000f);
        layer.mpb.SetFloat("_Cutoff", (float)i / shellNumber);
        layer.mpb.SetFloat("_AlphaClip", i==0?0:1);
        layer.mpb.SetFloat("_StrandDensity", strandDensity);
        layer.mpb.SetFloat("_BaseThickness", baseThickness);
        layer.mpb.SetFloat("_TipThickness", tipThickness);
        layer.mpb.SetFloat("_BaseDarkness", baseDarkness);
        layer.mpb.SetFloat("_SwayPower", swayPower);
        layer.mpb.SetFloat("_SwayAmount", swayAmount);
        layer.mpb.SetFloat("_GravityPower", gravityPower);
        layer.mpb.SetFloat("_RandomHairDisplacement", randomHairDisplacement);
        layer.mpb.SetColor("_BaseColor", colorTint);

        fluffyRenderer.SetPropertyBlock(layer.mpb,i);
    }

    //Delete and redo the layers and property blocks
    public void CleanSlate()
    {
        if (layers == null || layers.Count == 0) return;
        if (fluffyRenderer != null && originalMaterials != null)
            fluffyRenderer.sharedMaterials = originalMaterials;
        layers.Clear();
    }

    //Dynamically change each layer when the variables are changed
    private void OnValidate()
    {
        if (layers == null || layers.Count == 0) return;

        for (int i = 0; i < layers.Count; i++)
            UpdateShaderLayer(i);
    }
}

[Serializable]
public struct FluffyInfo
{
    public Material material;
    public MaterialPropertyBlock mpb;

    public FluffyInfo(Material material, MaterialPropertyBlock mpb)
    {
        this.material = material;
        this.mpb = mpb;
    }
}

[CustomEditor(typeof(Fluffy))]
[CanEditMultipleObjects]
public class LookAtPointEditor : Editor
{
    private Fluffy fluffy;

    private void OnEnable()
    {
        fluffy = (Fluffy)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Make Layers"))
        {
            fluffy.MakeFluffy();
        }

        if (GUILayout.Button("Clean Slate"))
        {
            fluffy.CleanSlate();
        }
    }
}
