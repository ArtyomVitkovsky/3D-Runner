using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Modules.MainModule.Scripts.UI.Button;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonInteraction : MonoBehaviour, IClickable, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField] protected Button button;
    [SerializeField] protected TextMeshProUGUI buttonText;
    [SerializeField] protected List<ButtonView> buttonViews;
    protected Transform buttonTransform;
    protected Vector3 startScale;
    protected List<UnityAction> listeners;
    protected List<UnityAction> persistentListeners;

    public UnityAction<ButtonInteraction> onButtonClick;


    public void Initialize()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        buttonTransform = button.transform;
        startScale = buttonTransform.localScale;
        listeners = new List<UnityAction>();
        persistentListeners = new List<UnityAction>();
        listeners = new List<UnityAction>();
    }
    
    public void Initialize(string buttonText)
    {
        this.buttonText.text = buttonText;
        Initialize();
    }

    protected virtual void OnButtonClick()
    {
        if (persistentListeners != null && persistentListeners.Count != 0)
        {
            foreach (var persistentListener in persistentListeners)
            {
                persistentListener?.Invoke();
            }
        }

        if (listeners != null && listeners.Count != 0)
        {
            foreach (var listener in listeners)
            {
                listener?.Invoke();
            }
        }
        
        onButtonClick?.Invoke(this);

        ClearListeners();
    }

    protected ButtonView GetButtonViewType(ButtonViewType buttonViewType)
    {
        var buttonView = buttonViews.Find(v => v.ViewType == buttonViewType) 
                         ?? buttonViews.Find(v => v.ViewType == ButtonViewType.Default);
        return buttonView;
    }

    public void AddListener(UnityAction action)
    {
        listeners ??= new List<UnityAction>();

        listeners.Add(action);
    }
    
    public virtual void AddPersistentListener(UnityAction action)
    {
        persistentListeners ??= new List<UnityAction>();

        persistentListeners.Add(action);
    }

    public void ClearListeners()
    {
        listeners.Clear();
    }

    public void ClearAllListeners()
    {
        ClearListeners();
        persistentListeners.Clear();
    }
    
    
    public void OnPressAnimation()
    {
        buttonTransform.DOScale(startScale * 0.8f, 0.1f);
    }

    public void OnUnPressAnimation()
    {
        buttonTransform.DOScale(startScale, 0.1f);
    }

    public void OnMouseOverAnimation()
    {
        buttonTransform.DOScale(startScale * 1.1f, 0.1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPressAnimation();
        
        var buttonView = GetButtonViewType(ButtonViewType.OnMouseDown);
        button.image.sprite = buttonView.Sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseOverAnimation();
        
        var buttonView = GetButtonViewType(ButtonViewType.OnMouseOver);
        button.image.sprite = buttonView.Sprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnUnPressAnimation();

        var buttonView = GetButtonViewType(ButtonViewType.Default);
        button.image.sprite = buttonView.Sprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnUnPressAnimation();
        
        var buttonView = GetButtonViewType(ButtonViewType.Default);
        button.image.sprite = buttonView.Sprite;
    }
}
