using RayTween.Internal;
using UnityEngine;

namespace RayTween.Extensions
{
    /// <summary>
    /// Provides binding extension methods for Material.
    /// </summary>
    public static class RayTweenMaterialExtensions
    {
        /// <summary>
        /// Create a tween data and bind it to a material property.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
       
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToMaterialFloat<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, Material material, string name)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(material);
            return fromTo.Bind(material, name, static ( m, n,x) =>
            {
                m.SetFloat(n, x);
            });
        }

        /// <summary>
        /// Create a tween data and bind it to a material property.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
       
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToMaterialFloat<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, Material material, int nameID)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(material);
            return fromTo.Bind(material, (m, x) =>
            {
                m.SetFloat(nameID, x);
            });
        }

        /// <summary>
        /// Create a tween data and bind it to a material property.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="material"></param>
        /// <param name="name"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<int, TPlugin> BindToMaterialInt<TPlugin>(this TweenFromTo<int, TPlugin> fromTo, Material material, string name)
            where TPlugin : unmanaged, ITweenPlugin<int>
        {
            Error.IsNull(material);
            return fromTo.Bind(material, name, static (m, n,x) =>
            {
                m.SetInteger(n, x);
            });
        }

        /// <summary>
        /// Create a tween data and bind it to a material property.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="material"></param>
        /// <param name="nameID"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<int, TPlugin> BindToMaterialInt<TPlugin>(this TweenFromTo<int, TPlugin> fromTo, Material material, int nameID)
            where TPlugin : unmanaged, ITweenPlugin<int>
        {
            Error.IsNull(material);
            return fromTo.Bind(material, ReadOnlyIntBox.Create(nameID), static (m, n,x) =>
            {
                m.SetInteger(n.Value, x);
            });
        }

        /// <summary>
        /// Create a tween data and bind it to a material property.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="material"></param>
        /// <param name="name"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Color, TPlugin> BindToMaterialColor<TPlugin>(this TweenFromTo<Color, TPlugin> fromTo, Material material, string name)
            where TPlugin : unmanaged, ITweenPlugin<Color>
        {
            Error.IsNull(material);
            return fromTo.Bind(material, name, static (m, n,x) =>
            {
                m.SetColor(n, x);
            });
        }

        /// <summary>
        /// Create a tween data and bind it to a material property.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="material"></param>
        /// <param name="nameID"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Color, TPlugin> BindToMaterialColor<TPlugin>(this TweenFromTo<Color, TPlugin> fromTo, Material material, int nameID)
            where TPlugin : unmanaged, ITweenPlugin<Color>
        {
            Error.IsNull(material);
            return fromTo.Bind(material, ReadOnlyIntBox.Create(nameID), static (m, n,x) =>
            {
                m.SetColor(n.Value, x);
            });
        }
    }
}
