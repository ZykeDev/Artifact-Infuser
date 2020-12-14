using System;
using System.Collections.Generic;
using UnityEngine;

public static class OfflineProgress
{
 
    /// <summary>
    /// Returns the number of seconds since the Epoch
    /// </summary>
    /// <returns></returns>
    public static int GetTime()
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (int)(DateTime.UtcNow - epochStart).TotalSeconds;
    }

    /// <summary>
    /// Returns the number of seconds since the given Epoch-formatted time
    /// </summary>
    /// <param name="since">Epoch-format time</param>
    /// <returns></returns>
    public static int GetTimeSince(int since)
    {
        int now = GetTime();
                
        return now - since;
    }



    public static Inventory GetGatheredResources(SaveData saveData, List<MapArea> areas, Upgrades upgradesHandler)
    {
        List<Assistant> assistants = saveData.assistants;
        Inventory booty = new Inventory();

        int passedTime = GetTimeSince(saveData.saveTime);

        for (int i = 0; i < assistants.Count; i++)
        {
            // Ignore non working ones
            if (!assistants[i].isWorking) break;

            float gatherTime = areas[assistants[i].area].GetTime();
            gatherTime = upgradesHandler.ApplyBonuses(gatherTime);

            if (passedTime >= (int)gatherTime)
            {
                if (assistants[i].repeat)
                {
                    // Get a gather for every gatherTime
                    int nofGathers = (int)(passedTime / (gatherTime + 0.01f));

                    for (int j = 0; j < nofGathers; j++)
                    {
                        booty.SetRandomResources(assistants[i].area);
                    }
                }
                // If it was not set to reapeat 
                else
                {
                    // only get 1 gather worth of resources
                    booty.SetRandomResources(assistants[i].area);
                }
            }
        }

        return booty;
    }


}
