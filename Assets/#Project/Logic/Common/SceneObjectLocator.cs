using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crossfire.Workspace
{
    internal static class SceneObjectLocator
    {
        public static T FindInScene<T>(Scene scene) where T : Object
        {
            var instances = Resources.FindObjectsOfTypeAll<T>();
            foreach (var instance in instances)
            {
                if (instance is Component component)
                {
                    if (component.gameObject.scene == scene)
                        return instance;

                    continue;
                }

                if (instance is GameObject gameObject && gameObject.scene == scene)
                    return instance;
            }

            return null;
        }
    }
}
