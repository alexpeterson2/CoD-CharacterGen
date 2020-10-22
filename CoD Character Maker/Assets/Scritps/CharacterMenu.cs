using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenu : MonoBehaviour
{
    private Manager m_manager;

    void Start()
    {
        m_manager = FindObjectOfType<Manager>();
    }
    public void ChooseHunter()
    {
        m_manager.character.template = "Hunter";
    }

    public void ChooseVampire()
    {
        m_manager.character.template = "Vampire";
    }
}
