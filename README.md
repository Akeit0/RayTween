## How to install

### Package Manager
1. Open the Package Manager by going to Window > Package Manager.
2. Click on the "+" button and select "Add package from git URL".
3. Enter the following URL:

```
https://github.com/Akeit0/RayTween.git?path=/RayTween/Packages
```
### manifest.json
Open `Packages/manifest.json` and add the following in the `dependencies` block:

```json
"com.akeit0.com.akeit0.ray-tween": "https://github.com/Akeit0/RayTween.git?path=/RayTween/Packages"
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

        RPunch.Create(Vector3.zero, Vector3.up * 15f, 1f).BindToTMPCharPosition(text, i)
            .SetDelay(i * 0.1f)
            .SetEase(Ease.OutQuad);
    }
}
```
```cs
async UniTaskVoid Circle(int count)
{
    using PreservedTween<Vector3,Path3DTweenPlugin> p = RPath.Create3D(3f).BindToPosition(transform).WithPath(
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