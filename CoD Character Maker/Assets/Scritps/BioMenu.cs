using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioMenu : MonoBehaviour
{
    private Manager m_manager;

    void Start()
    {
        m_manager = FindObjectOfType<Manager>();
    }

    public void TemplateHandler(int val)
    {
        if (val == 1)
        {
            m_manager.character.template = Character.Template.Hunter;
        }
        else if (val == 2)
        {
            m_manager.character.template = Character.Template.Vampire;
        }
    }
}
