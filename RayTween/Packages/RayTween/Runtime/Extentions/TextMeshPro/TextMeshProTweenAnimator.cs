#if RAYTWEEN_SUPPORT_TMP
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RayTween.Extensions
{
    // TODO: optimization

    /// <summary>
    /// Wrapper class for animating individual characters in TextMeshPro.
    /// </summary>
    internal sealed class TextMeshProTweenAnimator
    {
      


        static TextMeshProTweenAnimator()
        {
            TweenDispatcher.OnUpdateAction += UpdateAnimators;
#if UNITY_EDITOR
            EditorApplication.update += UpdateAnimatorsEditor;
#endif
        }

        static bool initialized;
        static TextMeshProTweenAnimator rootNode;

        internal static TextMeshProTweenAnimator Get(TMP_Text text)
        {
            if (textToAnimator.TryGetValue(text, out var animator))
            {
              
                if (animator.refCount == 0) animator.Reset();
                animator.refCount++;
                return animator;
            }

            // get or create animator
            animator = rootNode ?? new();
            rootNode = animator.nextNode;
            animator.nextNode = null;

            // set target
            animator.target = text;
            animator.Reset();
            animator.refCount=1;
            // add to array
            if (tail == animators.Length)
            {
                Array.Resize(ref animators, tail * 2);
            }
            animators[tail] = animator;
            tail++;

            // add to dictionary
            textToAnimator.Add(text, animator);

            return animator;
        }
        internal void SetOnDispose<TValue,TPlugin>(TweenHandle<TValue,TPlugin> handle)where TValue : unmanaged where TPlugin : unmanaged,ITweenPlugin<TValue>
        {
            handle.OnDispose(
                this,static (target,_)=>target.refCount--);
        }
        internal static void Return(TextMeshProTweenAnimator animator)
        {
            animator.nextNode = rootNode;
            rootNode = animator;

            textToAnimator.Remove(animator.target);
            animator.target = null;
        }

        static readonly Dictionary<TMP_Text, TextMeshProTweenAnimator> textToAnimator = new();
        static TextMeshProTweenAnimator[] animators = new TextMeshProTweenAnimator[8];
        static int tail;

        static void UpdateAnimators(UpdateTiming timing)
        {
            if (timing != UpdateTiming.Update) return;
            var j = tail - 1;

            for (int i = 0; i < animators.Length; i++)
            {
                var animator = animators[i];
                if (animator != null)
                {
                    if (!animator.TryUpdate())
                    {
                        Return(animator);
                        animators[i] = null;
                    }
                    else
                    {
                        continue;
                    }
                }

                while (i < j)
                {
                    var fromTail = animators[j];
                    if (fromTail != null)
                    {
                        if (!fromTail.TryUpdate())
                        {
                            Return(fromTail);
                            animators[j] = null;
                            j--;
                        }
                        else
                        {
                            animators[i] = fromTail;
                            animators[j] = null;
                            j--;
                            goto NEXT_LOOP;
                        }
                    }
                    else
                    {
                        j--;
                    }
                }

                tail = i;
                break;

            NEXT_LOOP:;
            }
        }

#if UNITY_EDITOR
        static void UpdateAnimatorsEditor()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                return;
            }
            UpdateAnimators(UpdateTiming.Update);
        }
#endif

        internal struct CharInfo
        {
            public Vector3 position;
            public Vector3 scale;
            public Quaternion rotation;
            public Color color;
        }

        public TextMeshProTweenAnimator()
        {
            charInfoArray = new CharInfo[32];
            for (int i = 0; i < charInfoArray.Length; i++)
            {
                charInfoArray[i].color = Color.white;
                charInfoArray[i].rotation = Quaternion.identity;
                charInfoArray[i].scale = Vector3.one;
                charInfoArray[i].position = Vector3.zero;
            }
        }
        int refCount=0;
        TMP_Text target;
        internal CharInfo[] charInfoArray;

        TextMeshProTweenAnimator nextNode;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureCapacity(int length)
        {
            var prevLength = charInfoArray.Length;
            if (length > prevLength)
            {
                Array.Resize(ref charInfoArray, length);

                if (length > prevLength)
                {
                    var remainSpan = charInfoArray.AsSpan(prevLength, length - prevLength);
                    remainSpan.Fill(new CharInfo
                    {
                        color = target.color,
                        rotation = Quaternion.identity,
                        scale = Vector3.one,
                        position = Vector3.zero
                    });
                }
            }
        }

      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update()
        {
            TryUpdate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCharColor(int charIndex, Color color)
        {
            charInfoArray[charIndex].color = color;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCharPosition(int charIndex, Vector3 position)
        {
            charInfoArray[charIndex].position = position;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCharRotation(int charIndex, Quaternion rotation)
        {
            charInfoArray[charIndex].rotation = rotation;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCharScale(int charIndex, Vector3 scale)
        {
            charInfoArray[charIndex].scale = scale;
        }

        public void Reset()
        {
            for (int i = 0; i < charInfoArray.Length; i++)
            {
                charInfoArray[i].color = new(target.color.r, target.color.g, target.color.b, target.color.a);
                charInfoArray[i].rotation = Quaternion.identity;
                charInfoArray[i].scale = Vector3.one;
                charInfoArray[i].position = Vector3.zero;
            }
        }

       

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool TryUpdate()
        {
            if (target == null) return false;

          
            if (refCount <= 0) return false;

            target.ForceMeshUpdate();

            var textInfo = target.textInfo;
            EnsureCapacity(textInfo.characterCount);

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                ref var charInfo = ref textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                var materialIndex = charInfo.materialReferenceIndex;
                var vertexIndex = charInfo.vertexIndex;

                ref var colors = ref textInfo.meshInfo[materialIndex].colors32;
                ref var tweenCharInfo = ref charInfoArray[i];

                var charColor = tweenCharInfo.color;
                for (int n = 0; n < 4; n++)
                {
                    colors[vertexIndex + n] = charColor;
                }

                var verts = textInfo.meshInfo[materialIndex].vertices;
                var center = (verts[vertexIndex] + verts[vertexIndex + 2]) * 0.5f;

                var charRotation = tweenCharInfo.rotation;
                var charScale = tweenCharInfo.scale;
                var charOffset = tweenCharInfo.position;
                for (int n = 0; n < 4; n++)
                {
                    var vert = verts[vertexIndex + n];
                    var dir = vert - center;
                    verts[vertexIndex + n] = center +
                        charRotation * new Vector3(dir.x * charScale.x, dir.y * charScale.y, dir.z * charScale.z) +
                        charOffset;
                }
            }

            for (int i = 0; i < textInfo.materialCount; i++)
            {
                if (textInfo.meshInfo[i].mesh == null) continue;
                textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                target.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            return true;
        }
    }
}
#endif