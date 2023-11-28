using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// use this class to add static utility functions for gameobjects and the like.
public static class StaticUtils
{
    public static void FindComponentAndSetActive<T>(GameObject obj, bool value, out T component, bool findInChildren = true) where T : Component
    {
        if (obj == null)
        {
            Debug.LogWarning("FindAndSetActive(): GameObject obj is null.");
            component = null;
            return;
        }

        if (obj.TryGetComponent(out T comp))
        {
            comp.gameObject.SetActive(value);
            component = comp;
            return;
        } // try in its components

        if (!findInChildren)
        {
            component = null;
            return;
        }

        T componentInChildren = obj.GetComponentInChildren<T>(true); // get highlight in its children
        if (componentInChildren != null)
        {
            componentInChildren.gameObject.SetActive(value);
            component = componentInChildren;
            return;
        }

        if (comp == null && componentInChildren == null)
            Debug.LogWarning("FindAndSetActive(): " + obj.name + "'s " + typeof(T).FullName + " couldn't be found.");
        
        component = null;
    }
}
