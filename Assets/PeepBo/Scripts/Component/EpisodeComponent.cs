using PeepBo.Managers;
using PeepBo.UI.Popup;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PeepBo.Component
{
    public class EpisodeComponent : MonoBehaviour
    {
        public Image episodeBackgroundImage;
        public Image episodeStateImage;
        public Image episodeNewImage;
        public TextMeshProUGUI episodeIndexText;
        public TextMeshProUGUI episodeTitleText;

        public string page;
        public string index;
        public string title;

        UI_EpisodePopup popup;
        public string episodeIndex;
        public void InitEpisodeComponent(string _episodeIndex, string _episodeTitle,  EpisodeState episodeState, UI_EpisodePopup popup)
        {
            this.popup = popup;
            episodeIndex = _episodeIndex;

            gameObject.name = episodeIndex.ToString();

            page = _episodeIndex.ToString().Substring(0,1);
            index = _episodeIndex.ToString().Substring(1,2);

            int.TryParse(index, out int n);

            if(n < 10)
            {
                index = _episodeIndex.ToString().Substring(2, 1);
            }

            title = _episodeTitle;

            episodeIndexText.text = page + "-" + index;
            episodeTitleText.text = GameManager.EpisodeTitleDict[_episodeIndex.ToString()];

            /*
            var images = GetComponentsInChildren<Image>(true);
            episodeBackgroundImage = images[0];
            episodeStateImage = images[1];
            episodeNewImage = images[2];
            */
            /*
            var texts = GetComponentsInChildren<TextMeshProUGUI>();
            episodeIndexText = texts[0];
            episodeTitleText = texts[1];
            */
            switch (episodeState)
            {
                case EpisodeState.Locked:
                    SetLocked();
                    break;
                case EpisodeState.New:
                    SetNew();
                    break;
                case EpisodeState.Playing:
                    SetPlaying();
                    break;
                case EpisodeState.NotPlayed:
                    SetNotPlayed();
                    break;
                case EpisodeState.Played:
                    SetPlayed();
                    break;
            }
        }

        private void SetLocked()
        {
            episodeBackgroundImage.color = new Color(episodeBackgroundImage.color.r, episodeBackgroundImage.color.g, episodeBackgroundImage.color.b, 70f/255f);
            episodeTitleText.color = new Color(episodeTitleText.color.r, episodeTitleText.color.g, episodeTitleText.color.b, 120f / 255f);
            episodeIndexText.color = new Color(episodeIndexText.color.r, episodeIndexText.color.g, episodeIndexText.color.b, 120f / 255f);
            episodeStateImage.sprite = popup.lockedImage.sprite;
        }

        private void SetNew()
        {
            popup.SetEpisodeInteractable(this);
            episodeNewImage.gameObject.SetActive(true);
        }

        private void SetPlayed()
        {
            popup.SetEpisodeInteractable(this);
            episodeTitleText.color = new Color(episodeTitleText.color.r, episodeTitleText.color.g, episodeTitleText.color.b, 120f / 255f);
        }

        private void SetNotPlayed()
        {
            popup.SetEpisodeInteractable(this);
            //episodeText.color = new Color(episodeText.color.r, episodeText.color.g, episodeText.color.b, 120f / 255f);
        }

        private void SetPlaying()
        {
            popup.SetEpisodeInteractable(this);
            episodeStateImage.sprite = popup.playingImage.sprite;
        }
    }
}