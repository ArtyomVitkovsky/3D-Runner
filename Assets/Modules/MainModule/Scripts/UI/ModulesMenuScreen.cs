using System;
using System.Collections.Generic;
using Modules.MainModule.Scripts.Enums;
using Modules.MainModule.Scripts.UI.Button;
using Modules.MainModule.Scripts.UI.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;


namespace Modules.MainModule.Scripts.UI
{
    public class ModulesMenuScreen : MonoBehaviour, IScreen
    {
        [SerializeField] private GridLayoutGroup buttonsGrid;
        [SerializeField] private RectTransform buttonsHolder;
        private ObjectsPool<ButtonInteraction> buttonsPool;

        private Dictionary<ButtonInteraction, Module> buttonsModulesMap;
        
        private IButtonInteractionFactory buttonInteractionFactory;

        public Dictionary<ButtonInteraction, Module> ButtonsModulesMap => buttonsModulesMap;

        [Inject]
        private void Construct(IButtonInteractionFactory buttonInteractionFactory)
        {
            this.buttonInteractionFactory = buttonInteractionFactory;
        }

        public void Initialize()
        {
            
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void Initialize(ModuleSo[] modules)
        {
            buttonsPool = new ObjectsPool<ButtonInteraction>(modules.Length, true);
            buttonsModulesMap ??= new Dictionary<ButtonInteraction, Module>();
            
            for (int i = 0; i < modules.Length; i++)
            {
                var buttonInteraction = buttonInteractionFactory.Create(buttonsHolder);
                buttonInteraction.Initialize(modules[i].ModuleName);
                if (!buttonsModulesMap.ContainsKey(buttonInteraction))
                {
                    buttonsModulesMap.Add(buttonInteraction, modules[i].Module);
                }
            }
            
            SetupGridLayout(buttonsGrid, buttonsHolder, modules.Length);
        }

        private void SetupGridLayout(GridLayoutGroup grid, RectTransform holder, int elementsCount)
        {
            Canvas.ForceUpdateCanvases();
            
            RecalculateGrid(grid);

            var spacingVertical = grid.spacing.y * elementsCount - 1
                                  + grid.padding.top
                                  + grid.padding.bottom;

            var spacingHorizontal = grid.spacing.x
                                    + grid.padding.left
                                    + grid.padding.right;

            var sizeCoefficient = grid.cellSize.y / grid.cellSize.x;

            var cellSizeX = holder.rect.width - spacingHorizontal;
            var cellSizeY = cellSizeX * sizeCoefficient;

            grid.cellSize = new Vector2(cellSizeX, cellSizeY);

            var holderHeight = cellSizeY * elementsCount + spacingVertical;

            holder.sizeDelta = new Vector2(holder.sizeDelta.x, holderHeight);
            
            RecalculateGrid(grid);
        }

        private void RecalculateGrid(GridLayoutGroup grid)
        {
            grid.CalculateLayoutInputHorizontal();
            grid.CalculateLayoutInputVertical();
            grid.SetLayoutHorizontal();
            grid.SetLayoutVertical();
        }
    }
}