using System.Collections.Generic;
using Game.NovelVisualization.Runtime;
using UnityEditor;

namespace Game.NovelVisualization.Editor
{
    [CustomEditor(typeof(DSDialogue))]
    public class DSInspector : UnityEditor.Editor
    {
        /* Dialogue Scriptable Objects */
        private SerializedProperty dialogueContainerProperty;
        private SerializedProperty dialogueGroupProperty;
        private SerializedProperty dialogueProperty;

        /* Filters */
        private SerializedProperty groupedDialoguesProperty;
        private SerializedProperty startingDialoguesOnlyProperty;

        /* Indexes */
        private SerializedProperty selectedDialogueGroupIndexProperty;
        private SerializedProperty selectedDialogueIndexProperty;

        private void OnEnable()
        {
            dialogueContainerProperty = serializedObject.FindProperty("dialogueContainer");
            dialogueGroupProperty = serializedObject.FindProperty("dialogueGroup");
            dialogueProperty = serializedObject.FindProperty("dialogue");

            groupedDialoguesProperty = serializedObject.FindProperty("groupedDialogues");
            startingDialoguesOnlyProperty = serializedObject.FindProperty("startingDialoguesOnly");

            selectedDialogueGroupIndexProperty = serializedObject.FindProperty("selectedDialogueGroupIndex");
            selectedDialogueIndexProperty = serializedObject.FindProperty("selectedDialogueIndex");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDialogueContainerArea();

            DSDialogueContainerSO currentDialogueContainer = (DSDialogueContainerSO) dialogueContainerProperty.objectReferenceValue;

            if (currentDialogueContainer == null)
            {
                StopDrawing("Select a Dialogue Container to see the rest of the Inspector.");

                return;
            }

            DrawFiltersArea();

            bool currentGroupedDialoguesFilter = groupedDialoguesProperty.boolValue;
            bool currentStartingDialoguesOnlyFilter = startingDialoguesOnlyProperty.boolValue;
            
            List<string> dialogueNames;

            string dialogueFolderPath = $"Assets/DialogueSystem/Dialogues/{currentDialogueContainer.FileName}";

            string dialogueInfoMessage;

            if (currentGroupedDialoguesFilter)
            {
                List<string> dialogueGroupNames = currentDialogueContainer.GetDialogueGroupNames();

                if (dialogueGroupNames.Count == 0)
                {
                    StopDrawing("There are no Dialogue Groups in this Dialogue Container.");

                    return;
                }

                DrawDialogueGroupArea(currentDialogueContainer, dialogueGroupNames);

                DSDialogueGroupSO dialogueGroup = (DSDialogueGroupSO) dialogueGroupProperty.objectReferenceValue;

                dialogueNames = currentDialogueContainer.GetGroupedDialogueNames(dialogueGroup, currentStartingDialoguesOnlyFilter);

                dialogueFolderPath += $"/Groups/{dialogueGroup.GroupName}/Dialogues";

                dialogueInfoMessage = "There are no" + (currentStartingDialoguesOnlyFilter ? " Starting" : "") + " Dialogues in this Dialogue Group.";
            }
            else
            {
                dialogueNames = currentDialogueContainer.GetUngroupedDialogueNames(currentStartingDialoguesOnlyFilter);

                dialogueFolderPath += "/Global/Dialogues";

                dialogueInfoMessage = "There are no" + (currentStartingDialoguesOnlyFilter ? " Starting" : "") + " Ungrouped Dialogues in this Dialogue Container.";
            }

            if (dialogueNames.Count == 0)
            {
                StopDrawing(dialogueInfoMessage);

                return;
            }

            DrawDialogueArea(dialogueNames, dialogueFolderPath);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDialogueContainerArea()
        {
            GraphInspectorUtility.DrawHeader("Dialogue Container");

            dialogueContainerProperty.DrawPropertyField();

            GraphInspectorUtility.DrawSpace();
        }

        private void DrawFiltersArea()
        {
            GraphInspectorUtility.DrawHeader("Filters");

            groupedDialoguesProperty.DrawPropertyField();
            startingDialoguesOnlyProperty.DrawPropertyField();

            GraphInspectorUtility.DrawSpace();
        }

        private void DrawDialogueGroupArea(DSDialogueContainerSO dialogueContainer, List<string> dialogueGroupNames)
        {
            GraphInspectorUtility.DrawHeader("Dialogue Group");

            int oldSelectedDialogueGroupIndex = selectedDialogueGroupIndexProperty.intValue;

            DSDialogueGroupSO oldDialogueGroup = (DSDialogueGroupSO) dialogueGroupProperty.objectReferenceValue;

            bool isOldDialogueGroupNull = oldDialogueGroup == null;

            string oldDialogueGroupName = isOldDialogueGroupNull ? "" : oldDialogueGroup.GroupName;

            UpdateIndexOnNamesListUpdate(dialogueGroupNames, selectedDialogueGroupIndexProperty, oldSelectedDialogueGroupIndex, oldDialogueGroupName, isOldDialogueGroupNull);

            selectedDialogueGroupIndexProperty.intValue = GraphInspectorUtility.DrawPopup("Dialogue Group", selectedDialogueGroupIndexProperty, dialogueGroupNames.ToArray());

            string selectedDialogueGroupName = dialogueGroupNames[selectedDialogueGroupIndexProperty.intValue];

            DSDialogueGroupSO selectedDialogueGroup = GraphIOUtility.LoadAsset<DSDialogueGroupSO>($"Assets/DialogueSystem/Dialogues/{dialogueContainer.FileName}/Groups/{selectedDialogueGroupName}", selectedDialogueGroupName);

            dialogueGroupProperty.objectReferenceValue = selectedDialogueGroup;

            GraphInspectorUtility.DrawDisabledFields(() => dialogueGroupProperty.DrawPropertyField());

            GraphInspectorUtility.DrawSpace();
        }

        private void DrawDialogueArea(List<string> dialogueNames, string dialogueFolderPath)
        {
            GraphInspectorUtility.DrawHeader("Dialogue");

            int oldSelectedDialogueIndex = selectedDialogueIndexProperty.intValue;

            DSDialogueSO oldDialogue = (DSDialogueSO) dialogueProperty.objectReferenceValue;

            bool isOldDialogueNull = oldDialogue == null;

            string oldDialogueName = isOldDialogueNull ? "" : oldDialogue.DialogueName;

            UpdateIndexOnNamesListUpdate(dialogueNames, selectedDialogueIndexProperty, oldSelectedDialogueIndex, oldDialogueName, isOldDialogueNull);

            selectedDialogueIndexProperty.intValue = GraphInspectorUtility.DrawPopup("Dialogue", selectedDialogueIndexProperty, dialogueNames.ToArray());

            string selectedDialogueName = dialogueNames[selectedDialogueIndexProperty.intValue];

            DSDialogueSO selectedDialogue = GraphIOUtility.LoadAsset<DSDialogueSO>(dialogueFolderPath, selectedDialogueName);

            dialogueProperty.objectReferenceValue = selectedDialogue;

            GraphInspectorUtility.DrawDisabledFields(() => dialogueProperty.DrawPropertyField());
        }

        private void StopDrawing(string reason, MessageType messageType = MessageType.Info)
        {
            GraphInspectorUtility.DrawHelpBox(reason, messageType);

            GraphInspectorUtility.DrawSpace();

            GraphInspectorUtility.DrawHelpBox("You need to select a Dialogue for this component to work properly at Runtime!", MessageType.Warning);

            serializedObject.ApplyModifiedProperties();
        }

        private void UpdateIndexOnNamesListUpdate(List<string> optionNames, SerializedProperty indexProperty, int oldSelectedPropertyIndex, string oldPropertyName, bool isOldPropertyNull)
        {
            if (isOldPropertyNull)
            {
                indexProperty.intValue = 0;

                return;
            }

            bool oldIndexIsOutOfBoundsOfNamesListCount = oldSelectedPropertyIndex > optionNames.Count - 1;
            bool oldNameIsDifferentThanSelectedName = oldIndexIsOutOfBoundsOfNamesListCount || oldPropertyName != optionNames[oldSelectedPropertyIndex];

            if (oldNameIsDifferentThanSelectedName)
            {
                if (optionNames.Contains(oldPropertyName))
                {
                    indexProperty.intValue = optionNames.IndexOf(oldPropertyName);

                    return;
                }

                indexProperty.intValue = 0;
            }
        }
    }
}