using System;
using UnityEngine;

public class DNA : MonoBehaviour
{
    private float _r;

    public float R
    {
        get 
        { 
            return _r; 
        }
        set
        { 
            _r = value; 
        }
    }

    [NonSerialized]
    public float G;

    [NonSerialized]
    public float B;

    [NonSerialized]
    public float Scale;

    [NonSerialized]
    public float TimeToDie = 0;

    [NonSerialized]
    public bool Dead = false;

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;

    // Start is called before the first frame update
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();

        _spriteRenderer.color = new Color(_r, G, B);
        this.transform.localScale = new Vector3(Scale, Scale, Scale);
    }

    private void OnMouseDown()
    {
        Dead = true;
        TimeToDie = PopulationManager.Elapsed;
        Debug.Log("Dead At: " + TimeToDie);
        _spriteRenderer.enabled = false;
        _collider2D.enabled = false;
    }
}
