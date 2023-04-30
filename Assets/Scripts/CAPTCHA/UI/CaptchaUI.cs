using System.Collections.Generic;
using CAPTCHA.EventImplementations;
using DG.Tweening;
using Events;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Extensions;

namespace CAPTCHA.UI
{
    public class CaptchaUI : MonoBehaviour
    {
        public CanvasGroup CanvasGroup;

        public Image CaptchaImage;
        public TextMeshProUGUI ItemToBeChosenText;
        public Button DoneButton;

        public Transform GridColumnGroup;

        public Transform GridRowPrefab;
        public CaptchaGridItem GridItemPrefab;

        [ShowInInspector]
        private List<Transform> m_GridRowItems = new List<Transform>();

        [ShowInInspector]
        private List<CaptchaGridItem> m_CaptchaGridItems = new List<CaptchaGridItem>();

        private int m_CreatedColumnCount = 5;

        private void OnEnable()
        {
            CreateGrids(5,5);
            
            DoneButton.onClick.AddListener(OnSelectionComplete);
        }

        [Button]
        public void SetUI(CaptchaData data)
        {
            CanvasGroup.Toggle(true, 0.2f);

            CaptchaImage.sprite = data.CaptchaImage;
            ItemToBeChosenText.DOText(data.ItemToBeChosen, 0.2f);

            SetGrids(data.RowAndColumnCount[0], data.RowAndColumnCount[1], data.CorrectTileIndexes);
        }

        private void CreateGrids(int rows, int columns)
        {
            for (var i = 0; i < rows; i++)
            {
                var rowT = Instantiate(GridRowPrefab, GridColumnGroup);

                for (var k = 0; k < columns; k++)
                {
                    var gridItem = Instantiate(GridItemPrefab, rowT);
                    m_CaptchaGridItems.Add(gridItem);
                }
                
                m_GridRowItems.Add(rowT);
                rowT.gameObject.SetActive(false);
            }

            m_CreatedColumnCount = columns;
        }

        private void SetGrids(int rows, int columns, List<int> correctGridIndexes)
        {
            var totalGridCount = 0;
            var usedGridCount = 0;
            for (var i = 0; i < m_GridRowItems.Count; i++)
            {
                if (i >= rows)
                {
                    m_GridRowItems[i].gameObject.SetActive(false);
                    continue;
                }
                
                for (var k = 0; k < m_CreatedColumnCount; k++)
                {
                    if (k >= columns)
                    {
                        m_CaptchaGridItems[totalGridCount].gameObject.SetActive(false);
                        totalGridCount++;
                        continue;
                    }
                    
                    m_CaptchaGridItems[totalGridCount].SetUI(correctGridIndexes.Contains(usedGridCount));
                    m_CaptchaGridItems[totalGridCount].gameObject.SetActive(true);
                    totalGridCount++;
                    usedGridCount++;
                }
                
                m_GridRowItems[i].gameObject.SetActive(true);
            }
        }

        private void OnSelectionComplete()
        {
            if (CaptchaManager.WrongChosen)
            {
                using var restartEvt = CaptchaEvent.Get().SendGlobal((int)CaptchaEventType.Restart);
            }
            else
            {
                using var finishEvt = CaptchaEvent.Get().SendGlobal((int)CaptchaEventType.Finish);
                CloseUI();
            }
        }

        private void CloseUI()
        {
            CanvasGroup.Toggle(false, 0.2f);    
        }
        
        private void OnDisable()
        {
            DoneButton.onClick.RemoveListener(OnSelectionComplete);
        }
    }
}