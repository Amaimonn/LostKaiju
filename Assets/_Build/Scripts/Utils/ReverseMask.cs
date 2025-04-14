using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

namespace LostKaiju.Utils
{    
    public class ReverseMask : MonoBehaviour, IMaterialModifier
    {
        private static readonly int _stencilComp = Shader.PropertyToID("_StencilComp");

        public Material GetModifiedMaterial(Material baseMaterial)
        {
            var modifiedMaterial = new Material(baseMaterial);
            modifiedMaterial.SetFloat(_stencilComp, Convert.ToSingle(CompareFunction.NotEqual));
            return modifiedMaterial;
        }
    }
}