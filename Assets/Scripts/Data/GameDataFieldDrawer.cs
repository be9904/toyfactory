using AttnKare.Data;
using UnityEditor;
using UnityEngine;

namespace AttnKare.Data
{
    [CustomPropertyDrawer(typeof(GameDataField))]
    public class GameDataFieldDrawer : PropertyDrawer
    {
        private readonly string[] popupOptions =
            { "int", "float", "bool", "string" };

        private GUIStyle popupStyle;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int lineCount = 2;
            return EditorGUIUtility.singleLineHeight * lineCount
                   + EditorGUIUtility.standardVerticalSpacing * (lineCount - 1);
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (popupStyle == null)
            {
                popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                popupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            // Get properties
            SerializedProperty dataType = property.FindPropertyRelative("dataType");
            SerializedProperty dataName = property.FindPropertyRelative("dataName");
            SerializedProperty intValue = property.FindPropertyRelative("intValue");
            SerializedProperty floatValue = property.FindPropertyRelative("floatValue");
            SerializedProperty boolValue = property.FindPropertyRelative("boolValue");
            SerializedProperty stringValue = property.FindPropertyRelative("stringValue");
            
            Rect dataNameRect = new Rect(
                position.x, 
                position.y, 
                position.width / 2, 
                EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(dataNameRect, dataName, GUIContent.none);

            // Calculate rect for configuration button
            Rect buttonRect = new Rect(
                position.x, 
                position.y + EditorGUIUtility.singleLineHeight + 1.5f * EditorGUIUtility.standardVerticalSpacing, 
                position.width, 
                EditorGUIUtility.singleLineHeight);
            buttonRect.yMin += popupStyle.margin.top;
            buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
            position.xMin = buttonRect.xMax;
            
            Rect fieldRect = new Rect(
                position.x, 
                position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, 
                position.width, 
                EditorGUIUtility.singleLineHeight);

            // Store old indent level and set it to 0, the PrefixLabel takes care of it
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            int result = EditorGUI.Popup(buttonRect, dataType.enumValueIndex, popupOptions, popupStyle);
            dataType.enumValueIndex = result;

            switch ((DataType)result)
            {
                case DataType.INT:
                    EditorGUI.PropertyField(fieldRect, intValue, GUIContent.none);
                    break;
                case DataType.FLOAT:
                    EditorGUI.PropertyField(fieldRect, floatValue, GUIContent.none);
                    break;
                case DataType.BOOL:
                    EditorGUI.PropertyField(fieldRect, boolValue, GUIContent.none);
                    break;
                case DataType.STRING:
                    EditorGUI.PropertyField(fieldRect, stringValue, GUIContent.none);
                    break;
            }

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
