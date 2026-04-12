# TMP Style Effect

A custom TextMeshPro shader and component for Unity that provides **double outline** and **drop shadow** effects — all controlled from a single, artist-friendly Inspector.

## Features

| Feature | Description |
|---------|-------------|
| **Face Color / Gradient** | Solid face color or top-to-bottom vertex gradient |
| **Inner Outline** | Independent inward + outward width, separate color |
| **Outer Outline** | Second outline layer wrapping around the inner one |
| **Drop Shadow** | Offset, dilate, softness — with atlas-padding-aware UV clamping to prevent cross-glyph artifacts |
| **Auto Padding** | Leverages TMP's `GetPaddingForMaterial` via underlay properties so quads always fit the effect |
| **Editor Warnings** | Custom inspector shows max safe shadow offset and warns when it's exceeded |

## Preview

<!-- Add screenshots / GIFs here -->

## Requirements

- **Unity** 2021.3 LTS or newer (tested up to 6000.x)
- **TextMeshPro** (built-in package)

## Installation

### Option A — Unity Package Manager (Git URL)

1. Open **Window → Package Manager**.
2. Click **+ → Add package from git URL…**
3. Paste:

   ```
   https://github.com/zasuozz-oss/unity-tmp-style-effect.git?path=Assets/TMP_StyleEffect
   ```

4. Click **Add**.

### Option B — Manual

1. Clone or download this repository.
2. Copy the `Assets/TMP_StyleEffect` folder into your project's `Assets` directory.

## Quick Start

1. Add a **TextMeshPro — Text (UI)** object to your scene.
2. Attach the **TMP_StyleEffect** component to the same GameObject.
3. Adjust Face Color, Inner/Outer Outline, and Drop Shadow in the Inspector.
4. The component automatically creates a material instance using the `TextMeshPro/TMP_StyleEffect` shader — no manual material setup needed.

## How It Works

### Shader — `TextMeshPro/TMP_StyleEffect`

A single-pass SDF shader that composites four layers back-to-front:

```
Shadow → Outer Outline → Inner Outline → Face
```

Key techniques:
- **Atlas-padding UV clamping** — shadow UV offsets are clamped to the font atlas padding range, preventing neighbor-glyph bleeding.
- **SDF-limit masking** — outlines and shadows are masked by the raw SDF value to avoid artifacts at the edge of the SDF field.
- **Separate alpha clips** — text and shadow use independent clip thresholds so shadows can render beyond the text quad without being cut off.

### Component — `TMP_StyleEffect`

An `[ExecuteAlways]` MonoBehaviour that:
- Creates and manages a runtime material instance.
- Maps Inspector fields to shader properties.
- Writes TMP underlay properties so `GetPaddingForMaterial` calculates correct quad padding automatically.

### Editor — `TMP_StyleEffectEditor`

A custom inspector that:
- Displays the computed **Max Safe Offset** (readonly).
- Shows a warning when the shadow offset exceeds what the current atlas padding can support.

## File Structure

```
Assets/TMP_StyleEffect/
├── Scripts/
│   ├── TMP_StyleEffect.cs            # Runtime component
│   └── Editor/
│       └── TMP_StyleEffectEditor.cs   # Custom inspector
└── Shaders/
    └── TMP_SDF_DoubleOutline.shader   # SDF shader
```

## Shadow Offset Limits

Shadow UV offsets are **clamped** to the font's atlas padding to prevent cross-glyph sampling artifacts. If you need larger shadow offsets:

1. Increase the **Atlas Padding** when generating your font asset (Font Asset Creator).
2. Or set the **Atlas Padding Override** field on the component.

The Inspector will show a warning when your offset exceeds the safe range.

## License

[MIT](LICENSE)

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.
