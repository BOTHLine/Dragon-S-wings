using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PlayerSettings : MonoBehaviour
{
    public bool movementFlag;
    public float movementSpeed = 0.0f;
}

[CustomEditor(typeof(PlayerSettings))]
public class MyScriptEditor : Editor
{
    protected static bool showMovement = true;

    private SerializedObject m_object;
    private SerializedProperty m_property;

    private System.Type[] testTypes;
    private SerializedProperty[] m_properties;

    public void OnEnable()
    {
        m_object = new SerializedObject(target);
        m_property = m_object.FindProperty("movementSpeed");

        testTypes = GetAllDerivedTypes(typeof(EntityState));
        m_properties = new SerializedProperty[testTypes.Length];
        for (int i = 0; i < m_properties.Length; i++)
        {
            m_properties[i] = m_object.FindProperty(testTypes[i].FullName);
        }
    }

    public override void OnInspectorGUI()
    {
        showMovement = EditorGUILayout.Foldout(showMovement, "Movement");

        if (showMovement)
        {
            EditorGUILayout.PropertyField(m_property);
        }

        for (int i = 0; i < m_properties.Length; i++)
        {
         //   Debug.Log(i);
         //   EditorGUILayout.PropertyField(m_properties[i]);
            EditorGUILayout.ToggleLeft(testTypes[i].Name, false);
            var values = testTypes[i].GetFields();
            foreach (var value in values)
            {
                var valueType = value.GetType();
                Debug.Log(valueType);
                if (valueType == typeof(int))
                {
                    EditorGUILayout.IntField(value.Name, 0);
                }
                else if (valueType == typeof(float))
                {
                    EditorGUILayout.FloatField(value.Name, 0.0f);
                }
            }
        }
    }

    /*
    public static int DrawBitMaskField(Rect position, int mask, System.Type type, GUIContent label)
    {
        string[] itemNames = System.Enum.GetNames(type);
        int[] itemValues = System.Enum.GetValues(type) as int[];

        int val = mask;
        int maskVal = 0;
        for (int i = 0; i < itemValues.Length; i++)
        {
            if (itemValues[i] != 0)
            {
                if ((val & itemValues[i]) == itemValues[i])
                {
                    maskVal |= 1 << i;
                }
            }
            else if (val == 0)
            {
                maskVal |= 1 << i;
            }
        }
        int newMaskVal = EditorGUI.MaskField(position, label, maskVal, itemNames);
        int changes = maskVal ^ newMaskVal;

        for (int i = 0; i < itemValues.Length; i++)
        {
            if ((changes & (1 << i)) != 0)
            {
                if ((newMaskVal & (1 << i)) != 0)
                {
                    if (itemValues[i] == 0)
                    {
                        val = 0;
                        break;
                    }
                    else
                    {
                        val |= itemValues[i];
                    }
                }
                else
                {
                    val &= ~itemValues[i];
                }
            }
        }
        return val;
    }
    */

    public System.Type[] GetAllDerivedTypes(System.Type aType)
    {
        var result = new List<System.Type>();
        var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsSubclassOf(aType))
                {
                    result.Add(type);
                }
            }
        }
        return result.ToArray();
    }
}

/*
[CustomPropertyDrawer(typeof(BitMaskAttribute))]
public class EnumBitMaskPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var typeAttribute = attribute as BitMaskAttribute;
        label.text = label.text + "(" + property.intValue + ")";
        property.intValue = MyScriptEditor.DrawBitMaskField(position, property.intValue, typeAttribute.propertyType, label);
    }
}
*/