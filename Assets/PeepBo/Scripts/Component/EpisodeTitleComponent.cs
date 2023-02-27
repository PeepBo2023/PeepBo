using PeepBo.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PeepBo.Component
{
    public class EpisodeTitleComponent : MonoBehaviour
    {
        // ������ �ѹ��� �������⶧���� OnShow���� Start�ص� ��
        void Start()
        {
            GameManager.EpisodeTitleDict.TryGetValue(GameManager.DummyEpisode, out var title);
            var text = GetComponent<TextMeshProUGUI>();
            text.text = title;
        }
    }
}