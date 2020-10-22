using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager manager;
    public Character character;

    // Start is called before the first frame update
    void Start()
    {
        if (manager == null)
        {
            manager = this;
        }
        else if (manager != null && manager != this)
        {
            Destroy(this);
        }

        if (character == null)
        {
            character = new Character();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
