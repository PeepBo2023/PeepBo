using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PeepBo.Managers
{
    public class InputManager : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.UI.ClosePopupUI();
            }

            if (Input.touchCount > 0)
                OnTouchEvent(Input.GetTouch(0));
            if (Input.GetMouseButtonDown(0))
                OnClickEvent(Input.mousePosition);
        }

        void OnTouchEvent(Touch touchFinger)
        {
            //Debug.Log("A");
        }

        void OnClickEvent(Vector3 pos)
        {
            //Debug.Log("A");
            //Engine.GetService<Camera>().scree
            Spawn(pos);
        }

        void Spawn(Vector3 screenPos)
        {
            //GameObject test = new GameObject();
            //Instantiate(test).AddComponent<Image>().sprite = sprite;

            //Instantiate(sprite);
        }
    }
}