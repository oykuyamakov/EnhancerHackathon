using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CAPTCHA
{
    [CreateAssetMenu]
    public class CaptchaData : ScriptableObject
    {
        public Sprite CaptchaImage;
        public string ItemToBeChosen;
        
        public int[] RowAndColumnCount = new int[2];
        public List<int> CorrectTileIndexes = new List<int>();
    }
}
