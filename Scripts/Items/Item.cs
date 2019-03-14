using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    public virtual void Use()
    {
        PlayerController Hero = FindObjectOfType<PlayerController>();
        
        if(name == "HP Potion")
        {
            Hero.HP.value += 3;
        }

        if (name == "MP Potion")
        {
            Hero.MP.value += 3;
        }
    }
}
