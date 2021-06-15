using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindActionUI : MonoBehaviour
{
    public class UpdateBindingUIEvent : UnityEvent<RebindActionUI, string, string, string>
    {
        /// update ui display icons! <see cref="GamepadIcon"/>
    }

    [Tooltip("Reference to action that is to be rebound from the UI.")]
    [SerializeField] 
    private InputActionReference m_Action;

    [SerializeField] 
    private string m_BindingId;
    
    [SerializeField]
    private InputBinding.DisplayStringOptions m_DisplayStringOptions;

    [Tooltip("Text label that will receive the name of the action. Optional. Set to None to have the "
           + "rebind UI not show a label for the action.")]
    [SerializeField]
    private Text m_ActionLabel;

    [Tooltip("Text label that will receive the current, formatted binding string.")]
    [SerializeField]
    private Text m_BindingText;

    [Tooltip("Optional UI that will be shown while a rebind is in progress.")]
    [SerializeField]
    private GameObject m_RebindOverlay;

    [Tooltip("Optional text label that will be updated with prompt for user input.")]
    [SerializeField]
    private Text m_RebindText;

    [Tooltip("Event that is triggered when the way the binding is display should be updated. This allows displaying "
    + "bindings in custom ways, e.g. using images instead of text.")]
    [SerializeField]
    private UpdateBindingUIEvent m_UpdateBindingUIEvent;

    [Tooltip("Button OnClick triggers the rebind function")]
    [SerializeField]
    private Button m_RebindButton;

    private InputActionRebindingExtensions.RebindingOperation m_RebindOperation;
    private static List<RebindActionUI> s_RebindActionUIs;

    public InputActionReference ActionReference
    {
        get => m_Action;
        set
        {
            m_Action = value;
            UpdateActionLabel();
            UpdateBindingDisplay();
        }
    }

    public Text BindingText
    {
        get => m_BindingText;
        set
        {
            m_BindingText = value;
            UpdateBindingDisplay();
        }
    }

    public InputBinding.DisplayStringOptions displayStringOptions
    {
        get => m_DisplayStringOptions;
        set
        {
            m_DisplayStringOptions = value;
            UpdateBindingDisplay();
        }
    }

    public UpdateBindingUIEvent updateBindingUIEvent
    {
        get
        {
            if (m_UpdateBindingUIEvent == null)
                m_UpdateBindingUIEvent = new UpdateBindingUIEvent();
            return m_UpdateBindingUIEvent;
        }
    }

    private void Awake()
    {
        m_RebindButton.onClick.AddListener(delegate { RebindButtonOnClickEvent(); });
    }

    private void OnEnable()
    {
        if (s_RebindActionUIs == null)
            s_RebindActionUIs = new List<RebindActionUI>();
        s_RebindActionUIs.Add(this);
        if (s_RebindActionUIs.Count == 1)   // why = 1? : static function! belong to class and only need add once.
            InputSystem.onActionChange += OnActionChange;
    }

    private void OnDisable()
    {
        m_RebindOperation?.Dispose();
        m_RebindOperation = null;

        s_RebindActionUIs.Remove(this);
        if (s_RebindActionUIs.Count == 0)
        {
            s_RebindActionUIs = null;
            InputSystem.onActionChange -= OnActionChange;
        }
    }

    private static void OnActionChange(object obj, InputActionChange change)
    {
        // obj can be either an InputAction or an InputActionMap
        // depending on the specific change.

        if (change != InputActionChange.BoundControlsChanged)
            return;

        var action = obj as InputAction;
        var actionMap = action?.actionMap ?? obj as InputActionMap; // A ?? B(if A is null, use B)
        var actionAsset = actionMap?.asset ?? obj as InputActionAsset;

        for (var i = 0; i < s_RebindActionUIs.Count; ++i)
        {
            var component = s_RebindActionUIs[i];
            var referencedAction = component.ActionReference?.action;
            if (referencedAction == null)
                continue;

            if (referencedAction == action ||
                referencedAction.actionMap == actionMap ||
                referencedAction.actionMap?.asset == actionAsset)
                component.UpdateBindingDisplay();
        }
    }

    private void UpdateActionLabel()
    {
        if (m_ActionLabel != null)
        {
            var action = m_Action?.action;
            m_ActionLabel.text = action != null ? action.name : string.Empty;
        }
    }

    public void UpdateBindingDisplay()
    {
        var displayString = string.Empty;
        var deviceLayoutName = default(string);
        var controlPath = default(string);

        var action = m_Action?.action;
        if (action != null)
        {
            var bindingIndex = action.bindings.IndexOf(x => x.id.ToString() == m_BindingId);
            if (bindingIndex != -1)
                displayString = action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, displayStringOptions);
        }

        if (m_BindingText != null)
            m_BindingText.text = displayString;

        /// update Icon display <see cref="GamepadIcon.OnUpdateBindingDisplay"/>
        m_UpdateBindingUIEvent?.Invoke(this, displayString, deviceLayoutName, controlPath);
    }

    private void RebindButtonOnClickEvent()
    {
        if (!ResolveActionAndBinding(out var action, out var bindingIndex))
            return;

        // If the binding is a composite(合成的), we need to rebind each part in turn.
        if (action.bindings[bindingIndex].isComposite)
        {
            var firstPartIndex = bindingIndex + 1;
            if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
                PerformInteractiveRebind(action, firstPartIndex, allCompositeParts: true);
        }
        else
        {
            PerformInteractiveRebind(action, bindingIndex);
        }
    }

    /// <summary>
    /// Return the action and binding index for the binding that is targeted by the component according to
    /// </summary>
    public bool ResolveActionAndBinding(out InputAction action, out int bindingIndex)
    {
        bindingIndex = -1;

        action = m_Action?.action;
        if (action == null)
            return false;

        if (string.IsNullOrEmpty(m_BindingId))
            return false;

        var bindingId = new Guid(m_BindingId);
        bindingIndex = action.bindings.IndexOf(x => x.id == bindingId);
        if (bindingIndex == -1)
        {
            Debug.LogError($"Cannot find binding with ID '{bindingId}' on '{action}'", this);
            return false;
        }

        return true;
    }

    private void PerformInteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts = false)
    {
        m_RebindOperation?.Cancel();

        // Debug.LogFormat("binding group name is {0}", action.bindings[bindingIndex].groups);

        if (transform.parent.name.Equals("KeyBoardBind"))
        {
            m_RebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                .WithControlsHavingToMatchPath("<Keyboard>")
                .WithCancelingThrough("<Keyboard>/escape")
                .OnMatchWaitForAnother(0.1f)
                .OnApplyBinding((operation, path) => OnRebindApply(action, path, action.bindings[bindingIndex].groups))
                .OnCancel(operation => OnRebindCancel())
                .OnComplete(operation => OnRebindComplete(action, bindingIndex, allCompositeParts));
        }
        else if (transform.parent.name.Equals("ControllerBind"))
        {
            m_RebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                .WithControlsHavingToMatchPath("<Gamepad>")
                .WithCancelingThrough("<Gamepad>/buttonEast")
                .OnMatchWaitForAnother(0.1f)
                .OnApplyBinding((operation, path) => OnRebindApply(action, path, action.bindings[bindingIndex].groups))
                .OnCancel(operation => OnRebindCancel())
                .OnComplete(operation => OnRebindComplete(action, bindingIndex, allCompositeParts));
        }

        // If it's a part binding, show the name of the part in the UI.
        var partName = default(string);
        if (action.bindings[bindingIndex].isPartOfComposite)
            partName = $"Binding '{action.bindings[bindingIndex].name}'. ";

        // Bring up rebind overlay, if we have one.
        m_RebindOverlay?.SetActive(true);
        if (m_RebindText != null)
        {
            var text = !string.IsNullOrEmpty(m_RebindOperation.expectedControlType)
                ? $"{partName}Waiting for {m_RebindOperation.expectedControlType} input..."
                : $"{partName}Waiting for input...";
            m_RebindText.text = text;
        }

        // If we have no rebind overlay and no callback but we have a binding text label,
        // temporarily set the binding text label to "<Waiting>".
        if (m_RebindOverlay == null && m_RebindText == null && /*m_RebindStartEvent == null &&*/ m_BindingText != null)
            m_BindingText.text = "<Waiting...>";

        // Give listeners a chance to act on the rebind starting.
        // m_RebindStartEvent?.Invoke(this, m_RebindOperation);

        m_RebindOperation.Start();
    }

    private void OnRebindApply(InputAction action, string path, string group)
    {
        InputActionMap actionMap = action?.actionMap;
        var bindings = actionMap?.bindings;

        foreach (var binding in bindings)
        {
            if (binding.effectivePath.Equals(path))
            {
                Debug.LogFormat("path has been existed, path is {0}", binding.path);
                // 检测到按键存在冲突，如：up已绑定到Y，此时down也准备绑定到Y
            }
        }

        // group 用于区分 control scheme,如：fire 动作可包含 keyboard 和 Gamepad 两种主题。
        // 若不指定主题，则新的 control path 会覆盖所有主题！
        action.ApplyBindingOverride(path, group);
    }

    private void OnRebindCancel()
    {
        // m_RebindStopEvent?.Invoke(this, operation);
        m_RebindOverlay?.SetActive(false);
        UpdateBindingDisplay();
        CleanUp();
    }

    private void OnRebindComplete(InputAction action, int bindingIndex, bool allCompositeParts = false)
    {
        m_RebindOverlay?.SetActive(false);
        // m_RebindStopEvent?.Invoke(this, operation);
        UpdateBindingDisplay();
        CleanUp();

        // If there's more composite parts we should bind, initiate a rebind
        // for the next part.
        if (allCompositeParts)
        {
            var nextBindingIndex = bindingIndex + 1;
            if (nextBindingIndex < action.bindings.Count && action.bindings[nextBindingIndex].isPartOfComposite)
                PerformInteractiveRebind(action, nextBindingIndex, true);
        }
    }

#if UNITY_EDITOR
    // We want the label for the action name to update in edit mode, too,
    // so we kick that off from here.
    protected void OnValidate()
    {
        UpdateActionLabel();
        UpdateBindingDisplay();
    }
#endif

    private void CleanUp()
    {
        m_RebindOperation?.Dispose();
        m_RebindOperation = null;
    }
}
