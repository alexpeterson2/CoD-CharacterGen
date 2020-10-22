using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationButton : MonoBehaviour
{
    public GameObject[] pages;

    private bool onAttributes;
    private bool onGeneralInfo;
    private bool onSkills;
    private bool onMerits;

    private void Start()
    {
        pages = GameObject.FindGameObjectsWithTag("Page");
    }

    public void PrevPage()
    {
        foreach(GameObject page in pages)
        {
            string pageName = page.name;

            switch (pageName)
            {
                case "Attributes":
                    if (page.activeSelf == true)
                    {
                        onGeneralInfo = true;
                        onAttributes = false;
                        break;
                    }
                    break;
                case "Skills":
                    if (page.activeSelf == true)
                    {
                        onSkills = false;
                        onAttributes = true;
                        break;
                    }
                    break;
                case "Merits":
                    if (page.activeSelf == true)
                    {
                        onSkills = true;
                        onMerits = false;
                        break;
                    }
                    break;
            }
            switch (pageName)
            {
                case "Attributes":
                    if (page.activeSelf == true && onAttributes == false)
                    {                        
                        page.SetActive(false);
                        break;
                    }
                    else if (page.activeSelf == false && onAttributes == true)
                    {
                        page.SetActive(true);
                        break;
                    }
                    break;
                case "Skills":
                    if (page.activeSelf == true && onSkills == false)
                    {
                        page.SetActive(false);
                        break;
                    }
                    else if (page.activeSelf == false && onSkills == true)
                    {
                        page.SetActive(true);
                        break;
                    }
                    break;
                case "Merits":
                    if (page.activeSelf == true && onMerits == false)
                    {
                        page.SetActive(false);
                        break;
                    }
                    else if (page.activeSelf == false && onMerits == true)
                    {
                        page.SetActive(true);
                        break;
                    }
                    break;
            }
        }
        
    }

    public void NextPage()
    {
        foreach (GameObject page in pages)
        {
            string pageName = page.name;

            switch (pageName)
            {
                case "GeneralInfo":
                    if (page.activeSelf == true)
                    {
                        onAttributes = true;
                        onGeneralInfo = false;
                        break;
                    }
                    break;
                case "Attributes":
                    if (page.activeSelf == true)
                    {
                        onSkills = true;
                        onAttributes = false;
                        break;
                    }
                    break;
                case "Skills":
                    if (page.activeSelf == true)
                    {
                        onSkills = false;
                        onMerits = true;
                        break;
                    }
                    break;
                
            }
            switch (pageName)
            {
                case "GeneralInfo":
                    if (page.activeSelf == true && onGeneralInfo == false)
                    {
                        page.SetActive(false);
                        break;
                    }
                    else if (page.activeSelf == false && onGeneralInfo == true)
                    {
                        page.SetActive(true);
                        break;
                    }
                    break;
                case "Attributes":
                    if (page.activeSelf == true && onAttributes == false)
                    {
                        page.SetActive(false);
                        break;
                    }
                    else if (page.activeSelf == false && onAttributes == true)
                    {
                        page.SetActive(true);
                        break;
                    }
                    break;
                case "Skills":
                    if (page.activeSelf == true && onSkills == false)
                    {
                        page.SetActive(false);
                        break;
                    }
                    else if (page.activeSelf == false && onSkills == true)
                    {
                        page.SetActive(true);
                        break;
                    }
                    break;
            }
        }
    }
}
