using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    public Image img_View;
    private GameObject img_choose;
    private List<GameObject> characters = new List<GameObject>();
    public string characterName;

    private void Start()
    {
        img_View = transform.parent.Find("img_Character").GetComponent<Image>();
        Transform ch = transform.Find("img_Character_List").transform.Find("img_bg");
        foreach (Transform character in ch)
        {
            characters.Add(character.gameObject);
        }
        img_choose = Instantiate(Resources.Load<GameObject>("UI_Main_Page/img_choose"), characters[0].transform);
    }
    public void Btn_PickCharacter(GameObject btn)
    {
        string spriteName = btn.transform.Find("img_character").GetComponent<Image>().sprite.name;
        UserInfo.characterName = spriteName;
        img_choose.transform.parent = btn.transform;
        img_choose.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
        img_View.sprite = Resources.Load<Sprite>("UI_Main_Page/Character/" + spriteName);
    }
}
