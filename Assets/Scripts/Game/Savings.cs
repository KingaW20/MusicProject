using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Savings
{
    [SerializeField] public List<string> Names;
    [SerializeField] public List<string> Dates;

    public Savings()
    {
        Names = new List<string>() { "", "", "", "", "", "", "" };
        Dates = new List<string>() { "", "", "", "", "", "", "" };
    }
}
