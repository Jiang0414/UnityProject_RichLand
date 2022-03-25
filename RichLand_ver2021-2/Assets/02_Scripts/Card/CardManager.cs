using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private static CardManager _instance;
    public static Texture[] cardSprites;
    public Texture cardTexture;
    public static CardManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CardManager();
            }
            if (cardSprites == null)
            {
                cardSprites = Resources.LoadAll<Texture>("Card/Cards_Textues");
            }
            return _instance;
        }
    }
    public Texture GetCardTextues(int ID)
    {        
        foreach (var texture in cardSprites)
        {
            if(texture.name == ID.ToString())
            {
                cardTexture = texture;
                break;
            }
        }
        return cardTexture;
    }
}
