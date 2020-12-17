using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenu : MonoBehaviour
{
    private Manager m_manager;

    public enum MenuState { Bio, Stats, Merits, Equipment };

    public MenuState currentState;
    public MenuState newState;
    public GameObject hunterMenu;
    public GameObject vampireMenu;
    public GameObject bioMenu;
    public GameObject statsMenu;
    public GameObject meritsMenu;
    public GameObject equipmentMenu;

    void Awake()
    {
        currentState = MenuState.Bio;
    }

    void Start()
    {
        m_manager = FindObjectOfType<Manager>();
    }

    void Update()
    {
        if(newState != currentState)
        {
            ChangeMenu();
        }
    }

    private void ChangeMenu()
    {
        GameObject[] menus = new GameObject[] { hunterMenu, vampireMenu, bioMenu, statsMenu, meritsMenu, equipmentMenu };

        currentState = newState;

        switch (currentState)
        {
            case MenuState.Bio:
                for (int i=0; i==menus.Length; i++)
                {
                    if (menus[i] != bioMenu)
                    {
                        menus[i].SetActive(false);
                    }
                    else
                    {
                        menus[i].SetActive(true);
                    }
                }
                break;
            case MenuState.Stats:
                for (int i = 0; i == menus.Length; i++)
                {
                    if (menus[i] != statsMenu)
                    {
                        menus[i].SetActive(false);
                    }
                    else
                    {
                        menus[i].SetActive(true);
                    }
                }
                break;
            case MenuState.Merits:
                for (int i = 0; i == menus.Length; i++)
                {
                    if (menus[i] != meritsMenu)
                    {
                        menus[i].SetActive(false);
                    }
                    else
                    {
                        menus[i].SetActive(true);
                    }
                }
                break;
            case MenuState.Equipment:
                for (int i = 0; i == menus.Length; i++)
                {
                    if (menus[i] != equipmentMenu)
                    {
                        menus[i].SetActive(false);
                    }
                    else
                    {
                        menus[i].SetActive(true);
                    }
                }
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

    public void OnBio()
    {
        newState = MenuState.Bio;
    }

    public void OnStats()
    {
        newState = MenuState.Stats;
    }

    public void OnMerits()
    {
        newState = MenuState.Merits;
    }

    public void OnEquipment()
    {
        newState = MenuState.Equipment;
    }
}
