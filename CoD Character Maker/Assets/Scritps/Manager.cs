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
        
        if (manager != null && manager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            manager = this;
        }

        if (character == null)
        {
            character = new Character();
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
