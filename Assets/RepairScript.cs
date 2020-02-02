using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairScript : MonoBehaviour
{
    public List<string> lettersForRepair = new List<string>();
    public int length;
    private TextMesh text;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMesh>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = string.Join(", ", lettersForRepair.ToArray());
    }

    public void Repair()
    {
        for (int i = 0; i < length; i++) { 
            float random = Random.value;
            string letterForRepair;
            if (random > 0.33)
            {
                if (random > 0.66)
                {
                    letterForRepair = "A";
                }
                else
                {
                    letterForRepair = "Z";
                }
            }
            else
            {
                letterForRepair = "E";
            }
            lettersForRepair.Add(letterForRepair);
        }
        Debug.Log(lettersForRepair);
    }

    public void ExitRepair()
    {
        text.text = "";
        lettersForRepair.Clear();
    }
}
