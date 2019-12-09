using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LlamaSoftware.UI.Editors
{
    [CustomEditor(typeof(MultiValueToggle))]
    [CanEditMultipleObjects]
    public class MultiValueToggleEditor : Editor
    {
        #region Serialized Properties
        private SerializedProperty Values;
        private SerializedProperty StartIndex;
        private SerializedProperty ChangeCallback;
        private SerializedProperty KeyboardDelay;
        private SerializedProperty AllowWrapping;
        #endregion

        #region GUIContents
        private GUIContent StartIndexGUIContent = new GUIContent("Start Index", "Index of the list to start on");
        private GUIContent ChangeCallbackGUIContent = new GUIContent("Change Callback", "Action to call when the value changes");
        private GUIContent KeyboardDelayGUIContent = new GUIContent("Keyboard Delay", "Delay (in seconds) to wait before allowing a second key press. Lower values increase the risk of accidental double-taps. Too high a value makes the control seem sluggish");
        private GUIContent AllowWrappingGUIContent = new GUIContent("Allow Wrapping", "Whether to allow incrementing again at the last value, to wrap to the first value, and decrementing at the first value to wrap the last value.");
        #endregion

        private MultiValueToggle MultiValueToggle;
        private ReorderableList ReorderableValues;
        private GUILayoutOption[] DefaultGUILayoutOptions = new GUILayoutOption[] { };

        private void OnEnable()
        {
            MultiValueToggle = (MultiValueToggle)target;

            Values = serializedObject.FindProperty("Values");
            StartIndex = serializedObject.FindProperty("StartIndex");
            ChangeCallback = serializedObject.FindProperty("ChangeCallback");
            KeyboardDelay = serializedObject.FindProperty("KeyboardDelay");
            AllowWrapping = serializedObject.FindProperty("AllowWrapping");

            ReorderableValues = new ReorderableList(serializedObject, Values, true, true, true, true);

            ReorderableValues.drawHeaderCallback = (rect) => {
                EditorGUI.LabelField(rect, "Values", EditorStyles.boldLabel);
            };

            ReorderableValues.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => { 
                SerializedProperty element = Values.GetArrayElementAtIndex(index);
                rect.y += 2;
                string value = EditorGUI.DelayedTextField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element.stringValue);

                if (!value.Equals(element.stringValue))
                {
                    Undo.RecordObject(MultiValueToggle, "Set Element Name");
                    MultiValueToggle.Values[index] = value;
                }
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.Space();
            ReorderableValues.DoLayoutList();
            EditorGUILayout.PropertyField(StartIndex, StartIndexGUIContent, DefaultGUILayoutOptions);
            EditorGUILayout.PropertyField(AllowWrapping, AllowWrappingGUIContent, DefaultGUILayoutOptions);
            EditorGUILayout.Slider(KeyboardDelay, 0, 1, KeyboardDelayGUIContent, DefaultGUILayoutOptions);
            EditorGUILayout.PropertyField(ChangeCallback, ChangeCallbackGUIContent, DefaultGUILayoutOptions);
            serializedObject.ApplyModifiedProperties();
        }
    }

}
