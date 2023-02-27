using Naninovel;
using PeepBo.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace PeepBo.Nani
{
    [InitializeAtRuntime(@override: typeof(Naninovel.AudioManager))]
    public class PeepBoAudioManager : Naninovel.AudioManager
    {
        public PeepBoAudioManager(Naninovel.AudioConfiguration config, IResourceProviderManager providerManager, 
            ILocalizationManager localizationManager, ICharacterManager characterManager) 
            : base(config, providerManager, localizationManager, characterManager)
        {

        }

        public override UniTask ModifyBgmAsync(string path, float volume, bool loop, float time, AsyncToken asyncToken = default)
        {
            volume = GameManager.Setting.SettingData.bgmSound;
            return base.ModifyBgmAsync(path, volume, loop, time, asyncToken);
        }

        public override UniTask ModifySfxAsync(string path, float volume, bool loop, float time, AsyncToken asyncToken = default)
        {
            volume = GameManager.Setting.SettingData.sfxSound;
            return base.ModifySfxAsync(path, volume, loop, time, asyncToken);
        }

        public override UniTask PlayBgmAsync(string path, float volume = 1, float fadeTime = 0, bool loop = true, string introPath = null, string group = null, AsyncToken asyncToken = default)
        {
            volume = GameManager.Setting.SettingData.bgmSound;
            return base.PlayBgmAsync(path, volume, fadeTime, loop, introPath, group, asyncToken);
        }

        public override UniTask PlaySfxAsync(string path, float volume = 1, float fadeTime = 0, bool loop = false, string group = null, AsyncToken asyncToken = default)
        {
            volume = GameManager.Setting.SettingData.sfxSound;
            return base.PlaySfxAsync(path, volume, fadeTime, loop, group, asyncToken);
        }

        public override UniTask PlayVoiceAsync(string path, float volume = 1, string group = null, string authorId = null, AsyncToken asyncToken = default)
        {
            volume = GameManager.Setting.SettingData.voiceSound;
            return base.PlayVoiceAsync(path, volume, group, authorId, asyncToken);
        }

    }
}