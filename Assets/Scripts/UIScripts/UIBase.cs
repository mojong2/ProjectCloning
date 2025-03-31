using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project_J
{
    public class UIBase : MonoBehaviour
    {
        [SerializeField] GameManager gameManager;
        public GameManager GameManager 
        {
            get { return gameManager; }
            set { gameManager = value; }
        }
    }
}
