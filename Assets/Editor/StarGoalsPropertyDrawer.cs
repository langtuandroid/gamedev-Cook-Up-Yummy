using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StarGoals))]
public class StarGoalsPropertyDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect star1 = new Rect(position.x, position.y,position.width, 17);
        Rect star2 = new Rect(position.x , position.y+20, position.width,17);
        Rect star3 = new Rect(position.x , position.y+40, position.width, 17);

        EditorGUI.IntSlider(star1, property.FindPropertyRelative("goal1Star"), 0,100);
        EditorGUI.IntSlider(star2, property.FindPropertyRelative("goal2Star"), 0, 100);
        EditorGUI.IntSlider(star3, property.FindPropertyRelative("goal3Star"), 0, 100);

        if (property.FindPropertyRelative("goal1Star").intValue > property.FindPropertyRelative("goal2Star").intValue)
            property.FindPropertyRelative("goal2Star").intValue = property.FindPropertyRelative("goal1Star").intValue+1;

        if (property.FindPropertyRelative("goal2Star").intValue > property.FindPropertyRelative("goal3Star").intValue)
            property.FindPropertyRelative("goal3Star").intValue = property.FindPropertyRelative("goal2Star").intValue+1;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 60;
    }
    
}

