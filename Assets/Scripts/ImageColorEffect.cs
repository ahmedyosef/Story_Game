using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Image))]
public class ImageColorEffect : MonoBehaviour
{
    public enum PlayMode
    {
        PlayOnce,
        LoopInfinite,
        LoopCustomCount
    }

    [Title("🎨 Image Settings")]
    [SerializeField, ReadOnly] private Image targetImage;

    [ColorPalette("Rainbow"), LabelText("Start Color")]
    public Color startColor = Color.white;

    [ColorPalette("Rainbow"), LabelText("End Color")]
    public Color endColor = Color.red;

    [Title("⚙️ Effect Settings")]
    [LabelText("Duration (sec)"), MinValue(0.1f)]
    public float duration = 1f;

    [LabelText("Ease Type")]
    public Ease easeType = Ease.InOutSine;

    [LabelText("Loop Type (for loops)")]
    public LoopType loopType = LoopType.Yoyo;

    [Space]
    [LabelText("Play Mode")]
    public PlayMode playMode = PlayMode.PlayOnce;

    [ShowIf("playMode", PlayMode.LoopCustomCount)]
    [LabelText("Loop Count")]
    [MinValue(1)]
    public int loopCount = 2;

    [LabelText("Auto Play On Start")]
    public bool playOnStart = false;

    private Tweener colorTween;

    // ────────────────────────────────────────────────
    private void Awake()
    {
        targetImage = GetComponent<Image>();
        targetImage.color = startColor;
    }

    private void Start()
    {
        if (playOnStart)
        {
            PlayColorEffect(endColor);
        }
    }

    // ────────────────────────────────────────────────
    [Button(ButtonSizes.Large), GUIColor(0.5f, 1f, 0.5f)]
    public void PlayColorEffect(Color your_Color)
    {
        endColor = your_Color;
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        // Kill any existing tween
        if (colorTween != null && colorTween.IsActive())
            colorTween.Kill();

        int loops = 0;
        switch (playMode)
        {
            case PlayMode.PlayOnce:
                loops = 0; // no looping
                break;
            case PlayMode.LoopInfinite:
                loops = -1; // infinite
                break;
            case PlayMode.LoopCustomCount:
                loops = loopCount;
                break;
        }

        // Create color tween
        colorTween = targetImage.DOColor(endColor, duration)
            .SetEase(easeType)
            .SetLoops(loops, loopType)
            .From(startColor)
            .OnComplete(() =>
            {
                if (playMode == PlayMode.PlayOnce)
                {
                    // 👇 Return to start color smoothly
                    targetImage.DOColor(startColor, duration * 0.5f)
                        .SetEase(Ease.InOutSine);
                }

                Debug.Log($"🎨 Color tween completed ({playMode})");
            });

        Debug.Log($"▶️ Started color tween ({playMode}), loops: {loops}");
    }

    [Button(ButtonSizes.Medium), GUIColor(1f, 0.4f, 0.4f)]
    public void StopEffect()
    {
        if (colorTween != null && colorTween.IsActive())
        {
            colorTween.Kill();
            Debug.Log("🛑 Color tween stopped.");
        }
    }

    [Button(ButtonSizes.Medium), GUIColor(0.6f, 0.8f, 1f)]
    public void ResetColor()
    {
        if (targetImage != null)
            targetImage.color = startColor;
    }
}
