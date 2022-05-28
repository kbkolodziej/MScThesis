using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : Component
{
    private string unlocked = "Achievement unlocked!";
    public void AchievementInteraction(string type)
    {
        GameObject infoPanel = StartInfoPanelForAchievement();
        // play a sound
        if (type == "Murderer")
        {
            string[] murderer = new string[] { "Achievement unlocked: M U R D E R E R" };
            infoPanel.GetComponent<InfoPanel>().infoLines = murderer;
        }
    }

    private GameObject StartInfoPanelForAchievement()
    {
        GameObject infoPanel = Inventory.instance.infoPanel;
        if (!infoPanel.activeSelf)
            infoPanel.SetActive(true);
        infoPanel.GetComponent<InfoPanel>().isAchievementInfo = true;
        infoPanel.GetComponent<InfoPanel>().textComponent.fontStyle = TMPro.FontStyles.Italic | TMPro.FontStyles.SmallCaps;
        infoPanel.GetComponent<InfoPanel>().textComponent.alignment = TMPro.TextAlignmentOptions.Center;
        return infoPanel;
    }
}
