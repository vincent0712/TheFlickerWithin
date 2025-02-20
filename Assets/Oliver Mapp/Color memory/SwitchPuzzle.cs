using UnityEngine;

public class SwitchPuzzle : MonoBehaviour
{
    public Switch[] switches; // Array av switch-objekt
    public bool[] correctCombination = { false, false, false, false, false, true }; // Rätt kombination
    private bool victory;
    int W = 0;
    public bool  BigLamp;
    public GameObject[] SmallLamps;
    //rivate Fusebox fuse;
    private bool CanGetPoint = true;

    void Start()
    {
        //Fuse = GameObject.FindGameObjectsWithTag("Fuse").GetComponent<Fuse>;
        //switches = FindObjectsOfType<Switch>(); // Hitta alla switchar i scenen
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            for (int i = 0; i < switches.Length; i++)
            {
                // Kolla om switchens aktiva/inaktiva status matchar correctCombination
                if (switches[i].IsOn != correctCombination[i])
                {
                    Debug.Log($"Hej {i}");
                    //return; // Avbryt om en enda switch är fel
                    W = 0;
                }

                else if (switches[i].IsOn == correctCombination[i])
                {
                    //Debug.Log("Victory!"); // Alla switchar är rätt
                    victory = true;
                    W++;
                }
            }
            if (victory == true && W == 6)
            {
                Debug.Log("U win");
                BigLamp = true;

            }
        }
    }

    /*void OnMouseDown()
    {
        CheckWinCondition();
    }*/

    public void CheckWinCondition()
    {
        for (int i = 0; i < switches.Length; i++)
        {
            // Kolla om switchens aktiva/inaktiva status matchar correctCombination
            if (switches[i] == correctCombination[i])
            {
                Debug.Log("Hej");
                return; // Avbryt om en enda switch är fel

            }
        }
        Debug.Log("Victory!"); // Alla switchar är rätt
    }
}

