using PeepBo.UI.Popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PeepBo.Managers
{
    public class DataManager
    {
        readonly string naniAddressableLabel = "Naninovel";
        readonly string shopAddressableLabel = "Shop";

        public IEnumerator TryDownloadData()
        {
            var loadNaniData = TryLoadNaniData();
            while (loadNaniData.MoveNext())
                yield return null;

            var loadStaticData = TryLoadStaticData();
            while (loadStaticData.MoveNext())
                yield return null;
        }

        private IEnumerator TryLoadNaniData()
        {
            /*
            var a = Addressables.ClearDependencyCacheAsync(addressableLabelName, false);
            yield return a;
            
            var result = Caching.ClearCache();
            Debug.Log($"result : {result}");

            var b = Addressables.LoadResourceLocationsAsync(addressableLabelName);
            yield return b;


            if (b.Result != null && b.Result.Count > 0)
            {
                foreach (var location in b.Result)
                {
                    Debug.Log(location);
                }
            }
            */
            AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(naniAddressableLabel);

            yield return getDownloadSize;

            while (!getDownloadSize.IsDone)
                yield return null;

            long downloadAddressableSize = getDownloadSize.Result;

            Addressables.Release(getDownloadSize);

            Debug.Log($"to download : {downloadAddressableSize}");

            if (downloadAddressableSize > 0) // 기기에 데이터가 없거나(처음 구동) 서버에서 Asset 수정(catalog update)이 일어났을 때
            {
                bool agree = false;

                GameManager.UI.ShowOneButtonPopup(
                    $"{string.Format("{0:0.00}", downloadAddressableSize / 1000000.0)}MB의 추가 콘텐츠를 다운 받아야 합니다."
                    ,null, 
                    () => { agree = true; });

                while (!agree)
                    yield return null;

                var loadingPopup = GameManager.UI.ShowPopupUI<UI_LoadingBarPopup>();
                loadingPopup.InitLoadingBar("추가 데이터 다운로드 중 ...");

                AsyncOperationHandle downloadDependencies = Addressables.DownloadDependenciesAsync(naniAddressableLabel);
                while (downloadDependencies.PercentComplete < 1f)
                {
                    float realPercent = (float)((float)downloadDependencies.GetDownloadStatus().DownloadedBytes / downloadAddressableSize * 100.0);
                    //Debug.Log(realPercent);
                    loadingPopup.UpdateLoadingBar(realPercent);
                    yield return null;
                }

                loadingPopup.ClosePopupUI();

                yield return downloadDependencies;
            }
        }

        private IEnumerator TryLoadStaticData()
        {
            var loadChoiceData = LoadChoiceData();
            while (loadChoiceData.MoveNext())
                yield return null;

            var loadShopData = LoadShopData();
            while (loadShopData.MoveNext())
                yield return null;
        }

        private IEnumerator LoadChoiceData()
        {
            yield break;
        }

        private IEnumerator LoadShopData()
        {
            yield break;
        }
    }
}