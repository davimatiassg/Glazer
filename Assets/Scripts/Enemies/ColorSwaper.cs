using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwaper : MonoBehaviour
{
    [SerializeField] private Renderer _CharRender;
    public Color _col;
    public Color t_col;
    [SerializeField] private float trigTime = 1;
    private float tgt;
    private Color act_color;
    void Start()
    {
        this.gameObject.GetComponent<Renderer>();
        tgt = trigTime;
    }
    public void SetColor(Color c)
    {
        _CharRender.material.SetColor("_Color", c);
        act_color = c;
        
    }
    public bool SetDelayColor(Color c)
    {
        float dTime = (trigTime-tgt)/trigTime;
        tgt -= Time.deltaTime;
        Color inter_c = new Color(Mathf.Lerp(act_color.r, c.r, dTime), Mathf.Lerp(act_color.g, c.g, dTime), Mathf.Lerp(act_color.b, c.b, dTime), 1f);
        _CharRender.material.SetColor("_Color", inter_c);
        if(tgt <= 0)
        {
            act_color = c;
            tgt = trigTime;
            return true;
           
        }
        else
        {
            return false;
        }
    }
    public bool SetDefault()
    {
        return SetDelayColor(_col);
    }
    public bool SetTriggered()
    {
        return SetDelayColor(t_col);
    }
}
