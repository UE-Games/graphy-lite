using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;


namespace UniEnt.Graphy_Lite.Runtime.Util {


    static class ExtensionMethods {


        /// <summary>
        ///     Functions as the SetActive function in the GameObject class, but for a list of them.
        /// </summary>
        /// <param name="gameObjects">
        ///     List of GameObjects.
        /// </param>
        /// <param name="active">
        ///     Whether to turn them on or off.
        /// </param>
        [NotNull]
        public static List<GameObject> SetAllActive([NotNull] this List<GameObject> gameObjects, bool active) {
            foreach (GameObject gameObj in gameObjects)
                gameObj.SetActive(active);

            return gameObjects;
        }


        [NotNull]
        public static List<Image> SetOneActive([NotNull] this List<Image> images, int active) {
            for (var i = 0; i < images.Count; i++)
                images[i].gameObject.SetActive(i == active);

            return images;
        }


        [NotNull]
        public static List<Image> SetAllActive([NotNull] this List<Image> images, bool active) {
            foreach (Image image in images)
                image.gameObject.SetActive(active);

            return images;
        }


    }


}
