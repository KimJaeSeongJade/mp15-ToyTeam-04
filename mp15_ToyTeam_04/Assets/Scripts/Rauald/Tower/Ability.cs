using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    private string _name;
    private string _description;
    private float _value;
    private int _cost;

    public string Name => _name;
    public string Description => _description;
    public float Value => _value;
    public int Cost => _cost;

    public Ability(string name, string description, float value, int cost)
    {
        _name = name;
        _description = description;
        _value = value;
        _cost = cost;
    }
}