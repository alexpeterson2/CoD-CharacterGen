using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenu : MonoBehaviour
{
    private Manager m_manager;

    public enum MenuState { Bio, Stats, Merits, Equipment };

    public MenuState activeState;
    public GameObject hunterMenu;
    public GameObject vampireMenu;

    void Start()
    {
        m_manager = FindObjectOfType<Manager>();
    }

    void Update()
    {
        switch (activeState)
        {
            case MenuState.Bio:
                break;
            case MenuState.Stats:
                break;
            case MenuState.Merits:
                break;
            case MenuState.Equipment:
                break;
        }
    }

    // When Hunter button is pressed
    public void OnHunter()
    {
        m_manager.character.template = Character.Template.Hunter;
    }

    // When Vampire button is pressed
    public void OnVampire()
    {
        m_manager.character.template = Character.Template.Vampire;
    }
}
