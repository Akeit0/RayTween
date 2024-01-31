using System;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using RayTween;
using RayTween.Plugins;
using RayTween.Extensions;
using RayTween.Internal;
using TMPro;
using UnityEditor;
using Ease = RayTween.Ease;
using LoopType = RayTween.LoopType;
using Object = UnityEngine.Object;
using Random = Unity.Mathematics.Random;
using ScrambleMode = RayTween.ScrambleMode;
using StringOptions = RayTween.StringOptions;

namespace Scenes
{
    public class TestBehaviour : MonoBehaviour
    {
        public string Text;

        [SerializeField] TMP_Text text;

        [Button]
        void TextTween()
        {
            RTween.Create("", Text, 3f).Bind(text, (text, x) =>
            {
                var v = x.ToString();
                Debug.Log(v);
                text.text = v;
            }).SetOptions(
                new StringOptions()
                {
                    RandomState = new Random(5),
                    ScrambleMode = ScrambleMode.Custom,
                    RichTextEnabled = true,
                    CustomScrambleChars = "RayTween",
                }).SetScheduler(RayTween.Editor.EditorTweenScheduler.Update);
            
        }

        void Start()
        {
        }

        [Button]
        void TMPCharMotionExample()
        {
            
            
          
            // Get the number of characters from TMP_Text.textInfo.characterCount
            for (int i = 0; i < text.textInfo.characterCount; i++)
            {
              RTween.Create(Color.white, Color.red, 1f).BindToTMPCharColor(text, i)
                    .SetDelay(i * 0.1f).SetLink(this,true);

               RTween.Punch.Create(Vector3.zero, Vector3.up * 15f, 1f).BindToTMPCharPosition(text, i)
                    .SetDelay(i * 0.1f)
                    .SetEase(Ease.OutQuad) .SetEase(Ease.OutQuad).SetLink(this,true);
            }
        }
        [Button]
        void TMPCharMotionExampleL()
        {
            
          
            // Get the number of characters from TMP_Text.textInfo.characterCount
            for (int i = 0; i < text.textInfo.characterCount; i++)
            {
                LMotion.Create(Color.white, Color.red, 1f) .WithDelay(i * 0.1f).BindToTMPCharColor(text, i)
                   ;
                   

                LMotion.Punch.Create(Vector3.zero, Vector3.up * 15f, 1f).WithDelay(i * 0.1f)
                    .WithEase(LitMotion.Ease.OutQuad).BindToTMPCharPosition(text, i)
                    ;
            }
        }
        public Vector3[] Path;

        [Button]
        async UniTaskVoid Circle(int count)
        {
           
            // using PreservedTween<Vector3, Path3DTweenPlugin> p = RTween.CreatePath3D(3f).BindToPosition(transform)
            //     .WithPath(
            //         Path.AsSpan()).SetOptions(new PathTweenOptions(PathType.CatmullRom, true)).SetEase(Ease.OutSine)
            //     .SetLoops(3, LoopType.Flip).SetLink(this.gameObject).Preserve();

            for (int i = 0; i < count; i++)
            {
                RTween.Create(4f, 0.3f).BindToPositionY(transform).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo)
                    .SetLink(this.gameObject,true)  .Forget();

                await RTween.Create(4f, 0.3f).BindToPositionX(transform).SetEase(Ease.InSine).SetLink(this.gameObject,true);

                await RTween.Create(8f, 0.3f).BindToPositionX(transform).SetEase(Ease.OutSine).SetLink(this.gameObject,true);

                RTween.Create(-4f, 0.3f).BindToPositionY(transform).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo)
                    .SetLink(this.gameObject).Forget();

                await RTween.Create(4f, 0.3f).BindToPositionX(transform).SetEase(Ease.InSine).SetLink(this.gameObject,true);

                await RTween.Create(0f, 0.3f).BindToPositionX(transform).SetEase(Ease.OutSine).SetLink(this.gameObject,true);
            }
        }

        [Button]
        async UniTaskVoid LTest()
        {
           await LMotion.Create(0f, 1f, 3f).BindToUnityLogger();
            
            
        }

        // void Start()
        // {
        //     TweenDispatcher.OnUpdateAction += OnUpdate;
        //     unRegistered = false;
        // }
        //
        // bool unRegistered = false;
        // void OnUpdate(UpdateTiming timing)
        // {
        //     //if(timing==UpdateTiming.EditorUpdate)return;
        //     if (this == null)
        //     {
        //         if (!unRegistered&&timing==UpdateTiming.TimeUpdate)
        //         {
        //             TweenDispatcher.OnUpdateAction -= OnUpdate;
        //             unRegistered = true;
        //         }
        //         Debug.Log($"{EditorApplication.isPlaying} {timing.ToString()} {Time.frameCount}");
        //     }
        //     // else
        //     // {
        //     //     Debug.Log($" Update {timing.ToString()} {Time.frameCount}");
        //     // }
        // }
        // void OnDestroy()
        // {
        //    
        //     Debug.Log(Time.frameCount);
        // }
    }
}