using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// use this class to add static utility functions for gameobjects and the like.
public static class StaticUtils
{
    public static void FindComponentAndSetActive<T>(GameObject obj, bool value, bool findInChildren = true) where T : Component
    {
        if (obj == null)
        {
            Debug.LogWarning("FindAndSetActive(): GameObject obj is null.");
            return;
        }

        if (obj.TryGetComponent(out T component)) // try in its components
            component.gameObject.SetActive(value);

        if (!findInChildren)
            return;

        T componentInChildren = obj.GetComponentInChildren<T>(true); // get highlight in its children
        if (componentInChildren != null)
            componentInChildren.gameObject.SetActive(value);

        if (component == null && componentInChildren == null)
            Debug.LogWarning("FindAndSetActive(): " + obj.name + "'s " + typeof(T).FullName + " couldn't be found.");
    }
}
