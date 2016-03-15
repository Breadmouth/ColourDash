using UnityEngine;
using System.Collections;

public class BGControllerScript : MonoBehaviour {

    BGScript[] sprites;

	void Start () {

        sprites = GetComponentsInChildren<BGScript>();

	}

    public void Reset()
    {
        foreach(BGScript sprite in sprites)
        {
            sprite.Reset();
        }
    }
}
