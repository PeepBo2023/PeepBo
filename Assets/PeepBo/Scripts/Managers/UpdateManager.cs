using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeepBo.Managers
{
    public class UpdateManager : MonoBehaviour
    {
        List<IEnumerator> list = new List<IEnumerator>();

        public void Add(IEnumerator iterator) => list.Add(iterator);

        void Update()
        {
            if (list.Count == 0) return;
            for (int i = 0; i < list.Count; ++i)
            {
                bool isNext = list[i].MoveNext();
                if (isNext)
                {
                    if (list[i].Current is IEnumerator enumerator)
                        list.Add(enumerator);
                }
                else
                    list.RemoveAt(i--);
            }
        }
    }
}