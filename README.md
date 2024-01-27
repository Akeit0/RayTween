


# Special Thanks 
This library is based on the following libraries created by [AnnulusGames](https://github.com/AnnulusGames).

### TweenSystem
**[LitMotion](https://github.com/AnnulusGames/LitMotion)**
### Path tween
**[MagicTween](https://github.com/AnnulusGames/MagicTween)**

# Performance
Comparing with [LitMotion](https://github.com/AnnulusGames/LitMotion), start up performance is  1.1 times slower, and the update performance is same.
Exceptionally, string tween is faster than LitMotion.
## How to install

### Package Manager
1. Open the Package Manager by going to Window > Package Manager.
2. Click on the "+" button and select "Add package from git URL".
3. Enter the following URL:

```
https://github.com/Akeit0/RayTween.git?path=/RayTween/Packages/RayTween
```
### manifest.json
Open `Packages/manifest.json` and add the following in the `dependencies` block:

```json
"com.akeit0.com.akeit0.ray-tween": "https://github.com/Akeit0/RayTween.git?path=/RayTween/Packages/RayTween"
```
```cs
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
```
```cs
async UniTaskVoid Circle(int count)
{
   using PreservedTween<Vector3,Path3DTweenPlugin> p = RTween.CreatePath3D(3f).BindToPosition(transform).WithPath(
            Path.AsSpan()).SetOptions(new PathTweenOptions(PathType.CatmullRom, true)).SetEase(Ease.OutSine)
        .SetLoops(3, LoopType.Flip).Preserve();

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
```

# License
MIT License

https://github.com/AnnulusGames/LitMotion/blob/main/LICENSE

https://github.com/AnnulusGames/MagicTween/blob/main/LICENSE