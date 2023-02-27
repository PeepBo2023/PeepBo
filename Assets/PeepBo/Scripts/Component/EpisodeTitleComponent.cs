using PeepBo.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PeepBo.Component
{
    public class EpisodeTitleComponent : MonoBehaviour
    {
        // 어차피 한번만 보여지기때문에 OnShow말고 Start해도 됨
        void Start()
        {
            GameManager.EpisodeTitleDict.TryGetValue(GameManager.DummyEpisode, out var title);
            var text = GetComponent<TextMeshProUGUI>();
            text.text = title;
        }
    }
}