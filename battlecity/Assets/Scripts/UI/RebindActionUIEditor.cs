#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

/// custom(自定义) inspector for <see cref="RebindActionUI"/>

[CustomEditor(typeof(RebindActionUI))]
public class RebindActionUIEditor : Editor
{
    private static class Styles
    {
        public static GUIStyle boldLabel = new GUIStyle("MiniBoldLabel");
    }

    private SerializedProperty m_ActionProperty;    ///  <see cref="RebindActionUI.m_Action"/>
    private SerializedProperty m_BindingIdProperty; ///  <see cref="RebindActionUI.m_BindingId"/>
    private SerializedProperty m_DisplayStringOptionsProperty; ///  <see cref="RebindActionUI.m_DisplayStringOptions"/>
    private SerializedProperty m_ActionLabelProperty;   ///  <see cref="RebindActionUI.m_ActionLabel"/>
    private SerializedProperty m_BindingTextProperty;   ///  <see cref="RebindActionUI.m_BindingText"/>
    private SerializedProperty m_RebindOverlayProperty;   ///  <see cref="RebindActionUI.m_RebindOverlay"/>
    private SerializedProperty m_RebindTextProperty;   ///  <see cref="RebindActionUI.m_RebindText"/>
    private SerializedProperty m_RebindButtonProperty;   ///  <see cref="RebindActionUI.m_RebindButton"/>

    private GUIContent m_UILabel = new GUIContent("UI");    // label
    private GUIContent m_BindingLabel = new GUIContent("Binding");  // label
    private GUIContent m_DisplayOptionsLabel = new GUIContent("Display Options");   // label

    private GUIContent[] m_BindingOptions;
    private string[] m_BindingOptionValues;
    private int m_SelectedBindingOption;

    protected void OnEnable()
    {
        // Find serialized(序列化) property(属性) by name.
        m_ActionProperty = serializedObject.FindProperty("m_Action");  
        m_BindingIdProperty = serializedObject.FindProperty("m_BindingId");
        m_DisplayStringOptionsProperty = serializedObject.FindProperty("m_DisplayStringOptions");
        m_ActionLabelProperty = serializedObject.FindProperty("m_ActionLabel");
        m_BindingTextProperty = serializedObject.FindProperty("m_BindingText");
        m_RebindOverlayProperty = serializedObject.FindProperty("m_RebindOverlay");
        m_RebindTextProperty = serializedObject.FindProperty("m_RebindText");
        m_RebindButtonProperty = serializedObject.FindProperty("m_RebindButton");

        RefreshBindingOptions();
    }

    // Implement(实现) this function to make a custom(自定义) inspector.
    public override void OnInspectorGUI()  
    {
        // with EditorGUI.EndChangeCheck() form a code block.
        EditorGUI.BeginChangeCheck();

        /// Binding Section ///
        // Make a label(标签) field.
        EditorGUILayout.LabelField(m_BindingLabel, Styles.boldLabel);

        // Scope(管理) for managing the indent(缩进) level of the field labels.
        using (new EditorGUI.IndentLevelScope())
        {
            EditorGUILayout.PropertyField(m_ActionProperty);

            // Takes the currently selected index as a parameter and returns the index selected by the user.
            int newSelectedBinding = EditorGUILayout.Popup(m_BindingLabel, m_SelectedBindingOption, m_BindingOptions);
            if (newSelectedBinding != m_SelectedBindingOption)
            {
                string bindingId = m_BindingOptionValues[newSelectedBinding];
                m_BindingIdProperty.stringValue = bindingId;
                m_SelectedBindingOption = newSelectedBinding;
            }

            // Displays a menu with an option for every value of the enum type when clicked.
            var optionsOld = (InputBinding.DisplayStringOptions)m_DisplayStringOptionsProperty.intValue;
            var optionsNew = (InputBinding.DisplayStringOptions)EditorGUILayout.EnumFlagsField(m_DisplayOptionsLabel, optionsOld);
            if (optionsOld != optionsNew)
                m_DisplayStringOptionsProperty.intValue = (int)optionsNew;
        }

        // Make a small space between the previous control and the following.
        EditorGUILayout.Space();

        /// UI Section  ///
        EditorGUILayout.LabelField(m_UILabel, Styles.boldLabel);
        using (new EditorGUI.IndentLevelScope())
        {
            EditorGUILayout.PropertyField(m_ActionLabelProperty);
            EditorGUILayout.PropertyField(m_RebindButtonProperty);
            EditorGUILayout.PropertyField(m_BindingTextProperty);
            EditorGUILayout.PropertyField(m_RebindOverlayProperty);
            EditorGUILayout.PropertyField(m_RebindTextProperty);
        }

        /// Event Section ///
        /// NONE ///

        if (EditorGUI.EndChangeCheck())
        {
            // Apply property modifications.
            serializedObject.ApplyModifiedProperties();
            RefreshBindingOptions();
        }

        // don't use base.OnInspectorGUI() to hide origin inspector.
        // base.OnInspectorGUI();
    }

    protected void RefreshBindingOptions()
    {
        InputActionReference actionReference = (InputActionReference)m_ActionProperty.objectReferenceValue;
        InputAction action = actionReference?.action;

        if (action == null)
        {
            m_BindingOptions = new GUIContent[0];
            m_BindingOptionValues = new string[0];
            m_SelectedBindingOption = -1;
            return;
        }

        var bindings = action.bindings;
        int bindingCount = bindings.Count;

        m_BindingOptions = new GUIContent[bindingCount];
        m_BindingOptionValues = new string[bindingCount];
        m_SelectedBindingOption = -1;

        string currentBindingId = m_BindingIdProperty.stringValue;

        for (int i = 0; i < bindingCount; i++)
        {
            var binding = bindings[i];
            var bindingId = binding.id.ToString();
            // "binding.groups" represent control schemes!
            var haveBindingGroups = !string.IsNullOrEmpty(binding.groups);

            // If we don't have a binding groups (control schemes), show the device that if there are, for example,
            // there are two bindings with the display string "A", the user can see that one is for the keyboard
            // and the other for the Gamepad.

            /// DontUseShortDisplayNames:
            // Do not use short names of controls as set up by shortDisplayName and the "shortDisplayName" property in JSON.
            // This will, for example, not use LMB instead of "left Button" on leftButton.
            /// IgnoreBindingOverrides:
            // By default, effectivePath is used for generating a display name.
            // Using this option, the display string can be forced to path instead.
            /// DontOmitDevice:
            // By default device names are omitted from display strings.
            // With this option, they are included instead. For example, "A" will be "A [Gamepad]" instead.

            var displayOptions =
                   InputBinding.DisplayStringOptions.DontUseShortDisplayNames |
                   InputBinding.DisplayStringOptions.IgnoreBindingOverrides;
            if (!haveBindingGroups)
                displayOptions |= InputBinding.DisplayStringOptions.DontOmitDevice;

            var displayString = action.GetBindingDisplayString(i, displayOptions);

            // If binding is part of a composite(合成的), include the part name.
            if (binding.isPartOfComposite)
                displayString = $"{ObjectNames.NicifyVariableName(binding.name)}: {displayString}";

            // Some composites use '/' as a separator(分隔符).
            // When used in popup, this will lead to(导致) to sub menus(子菜单).
            // Prevent(预防) by instead using a backlash.
            displayString = displayString.Replace('/', '\\');

            // If the binding is part of control schemes, mention them.
            /// string.Join():
            // Concatenates(连接) the elements of a specified(指定的) array or the members of a collection,
            // using the specified(指定的) separator(分隔符) between each element or member.
            if (haveBindingGroups)
            {
                var asset = action.actionMap?.asset;
                if (asset != null)
                {
                    var controlSchemes = string.Join(", ",
                        binding.groups.Split(InputBinding.Separator)
                            .Select(x => asset.controlSchemes.FirstOrDefault(c => c.bindingGroup == x).name));

                    displayString = $"{displayString} ({controlSchemes})";
                }
            }

            m_BindingOptions[i] = new GUIContent(displayString);
            m_BindingOptionValues[i] = bindingId;

            if (currentBindingId == bindingId)
                m_SelectedBindingOption = i;
        }
    }
}
#endif