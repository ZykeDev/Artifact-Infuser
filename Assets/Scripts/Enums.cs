﻿using System.Collections.Generic;

public enum ResourceType {
	WOOD,
	METAL,
	LEATHER,
	CRYSTALS


};

public enum RuneType
{
	ALPHA,
	NOVA,
	PRISMA
}


public enum Rarity
{
	COMMON,
	UNCOMMON,
	RARE,
	UNIQUE,
	LEGENDARY,
	ABYSSAL,
	IMMORTAL
}



public enum Tab {
	GATHERING,
	CRAFTER,
	INFUSER,
	ARMORY, 
	UPGRADES
};



public enum ArtifactType {
	ALL,
	WEAPON,
	ARMOR,
	ACCESSORY,
	ABYSS
}

public enum ArmoryTab {
	ALL,
	WEAPONS,
	ARMOR,
	ACCESSORIES,
	ABYSS
}



public enum DialogType
{
	EMPTY,
	SELL,
	DIALOGUE,
	REQUEST,
	OFFLINE

}


public enum AutosellAmount
{
	ALL,
	ALL_BUT_1
}

public enum AutosellType
{
	ANY,
	TYPE,
	RARITY
}


public enum EffectType
{
	NONE, 
	RESOURCES,
	GOLD,
	TIME,
	UNLOCK

}


public enum EffectBonus
{
	NONE, PLUS, TIMES
}

// List of all unlockable features
public enum UnlockFeature
{
	NONE,
	AUTOSELL,
	INFUSER,
	BLUEPRINTS_1
}