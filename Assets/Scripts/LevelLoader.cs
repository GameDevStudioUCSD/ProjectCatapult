﻿using UnityEngine;
using System.Collections;

/**
 * Filename: LevelLoader.cs \n
 * Author: Michael Gonzalez \n
 * Date Drafted: 12/17/2015 \n
 * Description: This class wraps the default Unity Application.LoadLevel() 
 *              functions. This class will help during development and 
 *              deployment when scene indexes change or are added by giving
 *              a centralized location where all indexes are declared and 
 *              refrenced. This class has integer constants to represent
 *              a scene's real index, enumerators to refer to the levels and 
 *              worlds, and methods to actually load the levels.
 */
public class LevelLoader : MonoBehaviour {

    private const int MAIN_MENU = 0;
    private const int SPACE_1 = 9;
    private const int SPACE_2 = 2;
    private const int SPACE_3 = 3;
    private const int SPACE_4 = 4;
    private const int SPACE_9 = 6;
    private const int SPACE_5 = 7;
    private const int SPACE_8 = 8;
    private const int SPACE_BOSS = 5;
    private const int SPACE_BONUS = 1;

    public World world;
    public Level level;
    public void LoadLevel()
    {
        LoadLevel(world, level);
    }
    /**
     * Function Name: LoadLevel() \n
     * This function will load Level level of World world
     */
    public static void LoadLevel( World world, Level level)
    {
        switch (world)
        {
            case World.Space:
                LoadSpaceLevel(level);
                break;
            case World.MainMenu:
                Application.LoadLevel(MAIN_MENU);
                break;
            default:
                Debug.LogError("Something horrible happened when trying to load world: " + world);
                break;
        }
    }
    public static Level SceneIndexToLevelNumber(World world, int scene)
    {
        if (world == World.Space)
        {
            switch (scene)
            {
                case SPACE_1:
                    return Level.One;
                case SPACE_2:
                    return Level.Two;
                case SPACE_3:
                    return Level.Three;
                case SPACE_4:
                    return Level.Four;
                case SPACE_5:
                    return Level.Five;
                case SPACE_8:
                    return Level.Eight;
                case SPACE_9:
                    return Level.Nine;
                case SPACE_BOSS:
                    return Level.Boss;
            }
        }
        return Level.Bonus;
    }
    public static void LoadSpaceLevel( Level level )
    {
        switch( level )
        {
            case Level.One:
                Application.LoadLevel(SPACE_1);
                break;
            case Level.Two:
                Application.LoadLevel(SPACE_2);
                break;
            case Level.Three:
                Application.LoadLevel(SPACE_3);
                break;
            case Level.Four:
                Application.LoadLevel(SPACE_4);
                break;
            case Level.Five:
                Application.LoadLevel(SPACE_5);
                break;
            case Level.Eight:
                Application.LoadLevel(SPACE_8);
                break;
            case Level.Nine:
                Application.LoadLevel(SPACE_9);
                break;
            case Level.Boss:
                Application.LoadLevel(SPACE_BOSS);
                break;
            case Level.Bonus:
                Application.LoadLevel(SPACE_BONUS);
                break;
            default:
                Debug.LogError("Tried to load a Space level that has not been implemented!");
                break;
        }
    }

	// Used for stage panel generator.
	public void setLevel( World world, Level level){
		this.world = world;
		this.level = level;
	}

	public static Level intToLevel(int levelInt){
		switch (levelInt) {
		case 1:
			return Level.One;
		case 2:
			return Level.Two;
		case 3:
			return Level.Three;
		case 4:
			return Level.Four;
		case 5:
			return Level.Five;
		case 6:
			return Level.Six;
		case 7:
			return Level.Seven;
		case 8:
			return Level.Eight;
		case 9:
			return Level.Nine;
		default:
			return Level.One;
		}
	}
}

public enum World { Space, MainMenu}
public enum Level { One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Boss, Bonus }
public enum Menu { Main, WorldSelection }