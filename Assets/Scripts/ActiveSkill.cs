using UnityEngine;
using UnityEngine.UI;
public class ActiveSkill : MonoBehaviour
{
    private Image[] activeButtons;
    int size;

    void Awake()
    {

        activeButtons = GetComponentsInChildren<Image>();
        size = activeButtons.Length;
    }


    public void SetActiveButton(int number)
    {


        activeButtons[number + 1].enabled = true;

    }

    public void SetDisactiveButton(int number)
    {

        activeButtons[number + 1].enabled = false;

    }
    public void ResetActiveButtons()
    {

        for (int i = 1; i < size; i++)
        {

            activeButtons[i].enabled = false;



        }
    }
    public void SetActiveButtons(int number)
    {

        for (int i = 1; i <= number; i++)
        {

            activeButtons[i].enabled = true;



        }
    }

}
