using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project_J
{
    public class InstantiateScripts : MonoBehaviour
    {
        public static void InstantiateObject<T>(T prefab, int number, GameObject parent, Queue<T> queue) where T : Component
        {
            for (int i = 0; i < number; i++)
            {
                T tmp = Instantiate(prefab);
                tmp.transform.SetParent(parent.transform);
                queue.Enqueue(tmp);
                tmp.gameObject.SetActive(false);
            }
        }
    }
}