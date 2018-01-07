using UnityEngine;
using System;
using System.IO;

public class DataManager : SpriteOffset {

	// started game data
    public int money;
    public Level[] levels;
    public Gadget[] gadgets;
    public Intelligence[] intelligences;

	// path to save data
	string dataPath;
    GameData data;

    void SetDataPath() {
        dataPath = Path.Combine(Application.persistentDataPath, "data.json");
    }

	void Awake () {
        // data are stored on this path
        SetDataPath();
		MakePersistent();
	}

	// make the gameobject persistent between scenes
	void MakePersistent() {
		// remove the copy of the scene controller if exist
		GameObject[] objs = GameObject.FindGameObjectsWithTag("DataManager");
		if (objs.Length > 1) {
			Destroy(gameObject);
		}
		// save the scene after the reload
		DontDestroyOnLoad(gameObject);
	}

	// called when open the game
	void OnEnable() {
		LoadData();
	}

	// called when close the game
	void OnDisable() {
		SaveData();
	}

	// load the scene from a path
	void LoadData() {
		// reload all the text of a json file if exist
		if(File.Exists(dataPath)) {
			// we return it as game data
			data = JsonUtility.FromJson<GameData>(File.ReadAllText(dataPath));
		}
        // before creating the json save all the scriptable object data inside dinamic structure
        else {
            data = new GameData(money, levels, gadgets, intelligences);
        }
	}

	// save the scene in a path
	void SaveData() {
        // create the file if not exist
        if (dataPath == null) SetDataPath();
		File.CreateText(dataPath).Close();
		// create the json from the scene and add to the file created
		File.WriteAllText(dataPath, JsonUtility.ToJson(data));
	}

	// get or set the levels of game
    public PersistentLevel[] Levels {
		get {
			return data.persistentLevels;
		}
		set { 
			data.persistentLevels = value;
			SaveData();
		}
	}

	// get or set the levels of game
    public PersistentGadget[] Gadgets {
		get {
			return data.persistentGadgets;
		}
		set { 
			data.persistentGadgets = value;
			SaveData();
		}
	}

	// get or set the intelligences of game
    public PersistentIntelligence[] Intelligence {
		get {
			return data.persistentIntelligences;
		}
		set { 
			data.persistentIntelligences = value;
			SaveData();
		}
	}

    public bool AlreadyStarted {
        get {
            return data.alreadyStarted;
        }
        set {
            data.alreadyStarted = value;
            SaveData();
        }
    }

	// get or set the money of game
	public int MoneyData {
		get { 
			return data.persistentMoney;
		}
		set {
			data.persistentMoney = value;
			SaveData();
		}
	}
}

// store the data of all the scenes
[Serializable]
public class GameData {
    public int persistentMoney;
    public PersistentLevel[] persistentLevels;
    public PersistentGadget[] persistentGadgets;
    public PersistentIntelligence[] persistentIntelligences;
    public bool alreadyStarted;

    public GameData(int money, Level[] levels, Gadget[] gadgets, Intelligence[] intelligences) {
        persistentMoney = money;
        persistentLevels = new PersistentLevel[levels.Length];
        persistentGadgets = new PersistentGadget[gadgets.Length];
        persistentIntelligences = new PersistentIntelligence[intelligences.Length];
        for (int i = 0; i < persistentLevels.Length; i++) {
            persistentLevels[i] = new PersistentLevel(levels[i]);
        }
        for (int i = 0; i < persistentGadgets.Length; i++) {
            persistentGadgets[i] = new PersistentGadget(gadgets[i]);
        }
        for (int i = 0; i < persistentIntelligences.Length; i++) {
            persistentIntelligences[i] = new PersistentIntelligence(intelligences[i]);
        }
    }
}

[Serializable]
public class PersistentLevel {
    public string levelName;
    public int positionCountInSceneArray;
    public int timeLimit;
    public int maxObject;
    public bool isLocked;
    public int firstStarPrize;
    public int secondStarPrize;
    public int thirdStarPrize;
    public string description;
    public int objectsScore;
    public int timeScore;
    public bool levelCompleted;

    // check the number of the stars reached in this level
    public int StarsScore {
        get {
            int i = 0;
            if (levelCompleted) {
                i++;
            }
            // collected all the object
            if (objectsScore == maxObject && i == 1) {
                i++;
            }
            // finished in limit time
            if (timeScore <= timeLimit && i == 2) {
                i++;
            }
            return i;
        }
    }

    public PersistentLevel(Level level) {
        levelName = level.levelName;
        positionCountInSceneArray = level.positionCountInSceneArray;
        timeLimit = level.timeLimit;
        maxObject = level.maxObject;
        isLocked = level.isLocked;
        firstStarPrize = level.firstStarPrize;
        secondStarPrize = level.secondStarPrize;
        thirdStarPrize = level.thirdStarPrize;
        description = level.description;
    }
}

[Serializable]
public class PersistentGadget {

    public string gadgetName;
    public Sprite image;
    public int positionCountInSceneArray;
    public float coolDown;
    public float boostDuration;
    public bool isLocked;
    public int price;
    public string description;

    public PersistentGadget(Gadget gadget) {
        gadgetName = gadget.gadgetName;
        image = gadget.image;
        positionCountInSceneArray = gadget.positionCountInSceneArray;
        coolDown = gadget.coolDown;
        boostDuration = gadget.boostDuration;
        isLocked = gadget.isLocked;
        price = gadget.price;
        description = gadget.description;
    }
}

[Serializable]
public class PersistentIntelligence {

    public PersistentLevel[] levels;
    public int[] unlockedLevels;
    public int positionCountInSceneArray;
    public int price;
    public bool isLocked;
    public Sprite image;
    public string description;

    public PersistentIntelligence(Intelligence intelligence) {
        positionCountInSceneArray = intelligence.positionCountInSceneArray;
        price = intelligence.price;
        isLocked = intelligence.isLocked;
        image = intelligence.image;
        description = intelligence.description;

        // set levels
        levels = new PersistentLevel[intelligence.levels.Length];
        for (int i = 0; i < levels.Length; i++) {
            levels[i] = new PersistentLevel(intelligence.levels[i]);
        }

        unlockedLevels = new int[intelligence.levels.Length];
        for (int i = 0; i < levels.Length; i++)
        {
            unlockedLevels[i] = intelligence.unlockedLevels[i];
        }

    }

    // buy the information and unlock the information
    public virtual void Buy() {
        for (int i = 0; i < levels.Length; i++) {
            levels[i].isLocked = false;
        }
    }
}