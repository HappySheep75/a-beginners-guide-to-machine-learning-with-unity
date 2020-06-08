using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    private List<int> _genes = new List<int>();
    private int _dnaLength = 0;
    private int _maxValues = 0;

    public DNA(int length, int values)
    {
        _dnaLength = length;
        _maxValues = values;

        SetRandom();
    }

    public void SetRandom()
    {
        _genes.Clear();

        for (int i = 0; i < _dnaLength; i++)
        {
            _genes.Add(Random.Range(0, _maxValues));
        }
    }

    public void SetInt(int position, int value)
    {
        _genes[position] = value;
    }

    public void Combine(DNA dna1, DNA dna2)
    {
        for (int i = 0; i < _dnaLength; i++)
        {
            if (i < _dnaLength / 2.0)
            {
                this._genes[i] = dna1._genes[i];
            }
            else
            {
                this._genes[i] = dna2._genes[i];
            }
        }
    }

    public void Mutate()
    {
        _genes[Random.Range(0, _dnaLength)] = Random.Range(0, _maxValues);
    }

    public int GetGene(int position)
    {
        return _genes[position];
    }
}
