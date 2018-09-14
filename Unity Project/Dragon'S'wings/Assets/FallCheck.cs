using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallCheck : MonoBehaviour
{
    public new Collider2D collider2D;

    public int numIslandCollisions = 0;

    private void Awake()
    {
        Collider2D original = GetComponentInParent<Entity>().GetComponent<Collider2D>();
        System.Type type = original.GetType();
        collider2D = (Collider2D) gameObject.AddComponent(type);
        System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.DeclaredOnly;
        System.Reflection.PropertyInfo[] propertyInfos = type.GetProperties(flags);
        foreach (var propertyInfo in propertyInfos)
        {
            if (propertyInfo.CanWrite)
            {
                try
                {
                    propertyInfo.SetValue(collider2D, propertyInfo.GetValue(original, null), null);
                }
                catch { }
            }
        }
        System.Reflection.FieldInfo[] fieldInfos = type.GetFields(flags);
        foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
        {
            fieldInfo.SetValue(collider2D, fieldInfo.GetValue(original));
        }

        collider2D.isTrigger = true;
    }

    private void Start()
    {
        transform.localPosition = GetComponentInParent<Entity>().circleCollider2D.offset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        numIslandCollisions++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        numIslandCollisions--;
    }
}