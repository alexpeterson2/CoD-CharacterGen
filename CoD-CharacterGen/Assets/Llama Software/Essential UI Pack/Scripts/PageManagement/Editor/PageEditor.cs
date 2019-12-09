using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace LlamaSoftware.UI.Utility.Editors
{
    [CustomEditor(typeof(Page))]
    [CanEditMultipleObjects]
    public class PageEditor : Editor
    {
        #region Serialized Properties
        private SerializedProperty ExitOnNewPagePush;
        private SerializedProperty EntryClip;
        private SerializedProperty ExitClip;
        private SerializedProperty EntryMode;
        private SerializedProperty EntryDirection;
        private SerializedProperty ExitMode;
        private SerializedProperty ExitDirection;
        private SerializedProperty PrePushAction;
        private SerializedProperty PostPushAction;
        private SerializedProperty PrePopAction;
        private SerializedProperty PostPopAction;
        #endregion

        #region GUIContents
        private GUIContent ExitOnNewPagePushGUIContent = new GUIContent("Exit On New Page Push", "If checked, the page will play the exit animation when a new page is pushed on top of this one. The Exit Audio Clip will not be played.");
        private GUIContent EntryClipGUIContent = new GUIContent("Entry Audio", "AudioClip to play when page is pushed.");
        private GUIContent ExitClipGUIContent = new GUIContent("Exit Audio", "AudioClip to play when page is popped.");
        private GUIContent EntryModeGUIContent = new GUIContent("Entry Mode", "The way the page should enter.");
        private GUIContent EntryDirectionGUIContent = new GUIContent("Entry Direction", "The side of the screen the page should enter from.");
        private GUIContent ExitModeGUIContent = new GUIContent("Exit Mode", "The way the page should exit.");
        private GUIContent ExitDirectionGUIContent = new GUIContent("Entry Direction", "The side of the screen the page should exit out.");
        private GUIContent PrePushActionGUIContent = new GUIContent("Pre-push Action", "Action to call before the page is pushed.");
        private GUIContent PostPushActionGUIContent = new GUIContent("Post-push Action", "Action to call after the page enter animation has completed.");
        private GUIContent PrePopActionGUIContent = new GUIContent("Pre-pop Action", "Action to call before the page is popped.");
        private GUIContent PostPopActionGUIContent = new GUIContent("Post-pop Action", "Action to call after the page exit animation has completed.");
        #endregion

        private GUILayoutOption[] DefaultGUILayoutOptions = new GUILayoutOption[] { };
        private AnimBool ShowEntryDirection;
        private AnimBool ShowExitDirection;
        private bool ShowActions;

        private void OnEnable()
        {
            ExitOnNewPagePush = serializedObject.FindProperty("ExitOnNewPagePush");
            EntryClip = serializedObject.FindProperty("EntryClip");
            ExitClip = serializedObject.FindProperty("ExitClip");
            EntryMode = serializedObject.FindProperty("EntryMode");
            EntryDirection = serializedObject.FindProperty("EntryDirection");
            ExitMode = serializedObject.FindProperty("ExitMode");
            ExitDirection = serializedObject.FindProperty("ExitDirection");
            PrePushAction = serializedObject.FindProperty("PrePushAction");
            PostPushAction = serializedObject.FindProperty("PostPushAction");
            PrePopAction = serializedObject.FindProperty("PrePopAction");
            PostPopAction = serializedObject.FindProperty("PostPopAction");

            ShowEntryDirection = new AnimBool(true);
            ShowEntryDirection.valueChanged.AddListener(Repaint);

            ShowExitDirection = new AnimBool(true);
            ShowExitDirection.valueChanged.AddListener(Repaint);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            ShowEntryDirection.target = EntryMode.enumValueIndex == 1;
            ShowExitDirection.target = ExitMode.enumValueIndex == 1;

            #region Animation Configuration
            EditorGUILayout.LabelField("Animation Configuration", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(ExitOnNewPagePush, ExitOnNewPagePushGUIContent, DefaultGUILayoutOptions);

            EditorGUILayout.PropertyField(EntryMode, EntryModeGUIContent, DefaultGUILayoutOptions);

            if (EditorGUILayout.BeginFadeGroup(ShowEntryDirection.faded))
            {
                EditorGUILayout.PropertyField(EntryDirection, EntryDirectionGUIContent, DefaultGUILayoutOptions);
            }
            EditorGUILayout.EndFadeGroup();

            EditorGUILayout.PropertyField(ExitMode, ExitModeGUIContent, DefaultGUILayoutOptions);

            if (EditorGUILayout.BeginFadeGroup(ShowExitDirection.faded))
            {
                EditorGUILayout.PropertyField(ExitDirection, ExitDirectionGUIContent, DefaultGUILayoutOptions);
            }
            EditorGUILayout.EndFadeGroup();
            EditorGUI.indentLevel--;
            #endregion

            EditorGUILayout.Space();

            #region Audio Configuration
            EditorGUILayout.LabelField("Audio Configuration", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(EntryClip, EntryClipGUIContent, DefaultGUILayoutOptions);
            EditorGUILayout.PropertyField(ExitClip, ExitClipGUIContent, DefaultGUILayoutOptions);

            EditorGUI.indentLevel--;
            #endregion

            EditorGUILayout.Space();

            if (ShowActions)
            {
                #region Action Configuration
                EditorGUILayout.LabelField("Custom Actions", EditorStyles.boldLabel);

                EditorGUILayout.LabelField("Entry Actions", EditorStyles.miniBoldLabel);
                EditorGUILayout.HelpBox("Pre-push Action should be used to set up any data you want the user to see before it shows up on the UI. Start any web service calls here, or set up state of your dynamic page.", MessageType.Info);
                EditorGUILayout.PropertyField(PrePushAction, PrePushActionGUIContent, DefaultGUILayoutOptions);

                EditorGUILayout.Space();

                EditorGUILayout.HelpBox("Post-push Action can be used to do any layout or sizing adjustments that you need to have happen only after the page is fully visible. Note that anything done here will be already visible to the user.", MessageType.Info);
                EditorGUILayout.PropertyField(PostPushAction, PostPushActionGUIContent, DefaultGUILayoutOptions);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Exit Actions", EditorStyles.miniBoldLabel);
                EditorGUILayout.HelpBox("Pre-pop Action can be used to save the state of a dynamic page, or disable elements just before the exit animation is played.", MessageType.Info);
                EditorGUILayout.PropertyField(PrePopAction, PrePopActionGUIContent, DefaultGUILayoutOptions);

                EditorGUILayout.Space();

                EditorGUILayout.HelpBox("Post-pop Action is called when the exit animation has completed. Do any final clean up or resetting you don't want visible to the user here.", MessageType.Info);
                EditorGUILayout.PropertyField(PostPopAction, PostPopActionGUIContent, DefaultGUILayoutOptions);

                #endregion

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Hide Actions", GUILayout.Width(125)))
                {
                    ShowActions = false;
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Show Actions", GUILayout.Width(125)))
                {
                    ShowActions = true;
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
