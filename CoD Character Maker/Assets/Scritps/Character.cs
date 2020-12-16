using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public Template template;
    public string charName;
    public int strength;
    public int resolve;
    public int wits;
    public int stamina;
    public int composure;
    public int manipulation;
    public int presence;
    public int dexterity;
    public int intelligence;

    public enum Template { Hunter, Vampire };
}
