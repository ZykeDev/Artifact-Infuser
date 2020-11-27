using System.Collections.Generic;
using UnityEngine;

public static class BlueprintDatabase
{
    public static List<Blueprint> blueprintsList;


    public static void SetBlueprints(List<Blueprint> blueprints) 
    {
        blueprintsList = blueprints;
    }


    /// <summary>
    /// Returns the blueprint with the given id. Returns null if it doesn't exist in the current list.
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public static Blueprint GetBlueprint(int ID)
    {
        // Try first to get it using index = ID
        if (ID < blueprintsList.Count)
        {
            if (blueprintsList[ID].GetID() == ID)
            {
                return blueprintsList[ID];
            }
        }
        
        // Otherwise look up every bp in the list
        foreach(Blueprint blueprint in blueprintsList)
        {
            if (blueprint.GetID() == ID)
            {
                return blueprint;
            }
        }

        return null;
    }

    public static Blueprint GetBlueprint(Rarity rarity)
    {
        List<Blueprint> filteredBlueprints = FilterBlueprints(rarity);

        int randomIndex = Random.Range(0, filteredBlueprints.Count - 1);

        return filteredBlueprints[randomIndex];
    }




    private static List<Blueprint> FilterBlueprints(Rarity rarity)
    {
        List<Blueprint> filteredBlueprints = new List<Blueprint>();

        foreach (Blueprint bp in blueprintsList)
        {
            if (bp.GetRarity() == rarity)
            {
                filteredBlueprints.Add(bp);
            }
        }

        return filteredBlueprints;
    }
    
}
