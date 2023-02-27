using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserPlayData
{
    public List<string> unlockedEpisodeList = new List<string>();
    public Dictionary<string, PlayedEpisodeData> playedEpisodeDataDict = new Dictionary<string, PlayedEpisodeData>();
}

[Serializable]
public class PlayedEpisodeData
{
    public bool roomCompleted = false;
    public bool episodeCompleted = false;
    public Dictionary<string, string> choicedDict = new Dictionary<string, string>();
}

[Serializable]
public class UserPlayDataEntity
{
    public UserPlayData userPlayData;
    public void CreateUserPlayData()
	{

	}
}