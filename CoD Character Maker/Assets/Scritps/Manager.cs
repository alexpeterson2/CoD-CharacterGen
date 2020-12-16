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
        // Singleton setup
        if (manager != null && manager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            manager = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void NewCharacter()
    {
        // Character being made
        character = new Character();
    }
}
