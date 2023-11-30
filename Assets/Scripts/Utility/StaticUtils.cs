using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// use this class to add static utility functions for gameobjects and the like.
public static class StaticUtils
{
    /// <summary>
    /// Projector material is like renderer.shaderMaterial. if you need to change instance, use this extension.
    /// </summary>
    /// <param name="projector"></param>
    /// <param name="color"></param>
    /// <param name="keepAlpha"></param>
    public static void ChangeColor(this Projector projector, Color color, bool keepAlpha = true)
    {
        var mat = new Material(projector.material);
        if (!mat.name.Contains("(Instance)"))
            mat.name += " (Instance)";

        if (keepAlpha)
            color.a = mat.color.a;
        mat.color = color;

        projector.material = mat;
    }

    public static void FindComponentAndSetActive<T>(this GameObject obj, bool value, out T component, bool setGameObject = true, bool findInChildren = true) where T : Component
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

    public static void FindComponentsAndSetActive<T>(this GameObject obj, bool value, out T[] component, bool setGameObject = true, bool findInChildren = true) where T : Component
    {
        if (obj == null)
        {
            Debug.LogWarning("FindAndSetActive(): GameObject obj is null.");
            component = null;
            return;
        }

        T[] comps = obj.GetComponents<T>();
        if (comps.Length > 0)
        {
            foreach (T comp in comps)
            {
                comp.gameObject.SetActive(value);
            }
            component = comps;
            return;
        } // try in its components

        if (!findInChildren)
        {
            component = null;
            return;
        }

        T[] componentsInChildren = obj.GetComponentsInChildren<T>(true); // get highlight in its children
        if (componentsInChildren != null)
        {
            foreach (T comp in componentsInChildren)
            {
                comp.gameObject.SetActive(value);
            }
            component = componentsInChildren;
            return;
        }

        if ((comps == null && componentsInChildren == null) || (comps.Length < 1 && componentsInChildren.Length < 1))
            Debug.LogWarning("FindAndSetActive(): " + obj.name + "'s " + typeof(T).FullName + " couldn't be found.");

        component = null;
    }
}
