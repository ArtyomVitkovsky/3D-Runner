using System.Collections;
using System.Collections.Generic;
using Modules.MainModule.Scripts.UI;
using Modules.MainModule.Scripts.UI.Screens;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private UIManager uiManager;
    [Inject]
    private void Construct(UIManager uiManager)
    {
        this.uiManager = uiManager;
        
        this.uiManager.SetScreenActive<LoadingScreen>(false, false);
    }
    
    
}
