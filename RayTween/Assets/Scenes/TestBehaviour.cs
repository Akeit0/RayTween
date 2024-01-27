using System;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;
using RayTween;
using RayTween.Plugins;
using RayTween.Extensions;
using TMPro;
using Ease = RayTween.Ease;
using LoopType = RayTween.LoopType;
using Random = Unity.Mathematics.Random;
using ScrambleMode = RayTween.ScrambleMode;
using StringOptions = RayTween.StringOptions;

namespace Scenes
{
    public class TestBehaviour : MonoBehaviour
    {
        public string Text;

        // private void Start()
        // {
        //     transform.position = default;
        //     Application.targetFrameRate = 60;
        //     Circle(3).Forget();
        //     // RTween.Create("", "Ray<Tween", 3f).BindToText(text).OnDispose((x) => Debug.Log(x.ResultType)).Forget();
        //     TextTween();
        //     RTween.DelayedCall(3.5f, TMPCharMotionExample);
        // }


        [SerializeField] TMP_Text text;

        [Button]
        void TextTween()
        {
            RTween.Create("", Text, 3f).Bind(text, (text, x) =>
            {
                var v = x.AsSpan().ToString();
                text.text = v;
            }).SetOptions(
                new StringOptions()
                {
                    RandomState = new Random(5),
                    ScrambleMode = ScrambleMode.Custom,
                    RichTextEnabled = true,
                    CustomScrambleChars = "RayTween",
                });
        }

        [Button]
        void TMPCharMotionExample()
        {
            // Get the number of characters from TMP_Text.textInfo.characterCount
            for (int i = 0; i < text.textInfo.characterCount; i++)
            {
                RTween.Create(Color.white, Color.red, 1f).BindToTMPCharColor(text, i)
                    .SetDelay(i * 0.1f)
                    .SetEase(Ease.OutQuad);

                RTween.Punch.Create(Vector3.zero, Vector3.up * 15f, 1f).BindToTMPCharPosition(text, i)
                    .SetDelay(i * 0.1f)
                    .SetEase(Ease.OutQuad);
            }
        }

        public Vector3[] Path;

        [Button]
        async UniTaskVoid Circle(int count)
        {
            using PreservedTween<Vector3, Path3DTweenPlugin> p = RTween.CreatePath3D(3f).BindToPosition(transform)
                .WithPath(
                    Path.AsSpan()).SetOptions(new PathTweenOptions(PathType.CatmullRom, true)).SetEase(Ease.OutSine)
                .SetLoops(3, LoopType.Flip).Preserve().SetLink(this.gameObject);

            for (int i = 0; i < count; i++)
            {
                RTween.Create(4f, 0.3f).BindToPositionY(transform).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo)
                    .Forget();

                await RTween.Create(4f, 0.3f).BindToPositionX(transform).SetEase(Ease.InSine);

                await RTween.Create(8f, 0.3f).BindToPositionX(transform).SetEase(Ease.OutSine);

                RTween.Create(-4f, 0.3f).BindToPositionY(transform).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo)
                    .Forget();

                await RTween.Create(4f, 0.3f).BindToPositionX(transform).SetEase(Ease.InSine);

                await RTween.Create(0f, 0.3f).BindToPositionX(transform).SetEase(Ease.OutSine);
            }
        }

        [Button]
        async UniTaskVoid LTest()
        {
           await LMotion.Create(0f, 1f, 3f).BindToUnityLogger();
            
            
        }
}
}