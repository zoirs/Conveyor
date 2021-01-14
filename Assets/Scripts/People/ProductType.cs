using System;
using UnityEngine;

public enum ProductType {
    Empty,
    Yellow,
    Brown,
}

static class PeopleStateExtensions {
    public static Material GetMaterial(this ProductType key, GameSettingsInstaller.ProductSettings materials) {
        switch (key) {
            case ProductType.Yellow:
                return materials.YellowMaterial;
            case ProductType.Brown:
                return materials.BrownMaterial;
            case ProductType.Empty:
                return materials.EmptyMaterial;
            default:
                throw new ArgumentOutOfRangeException(nameof(key), key, null);
        }
    }

    public static Color GetColor(this ProductType key, GameSettingsInstaller.ProductSettings materials) {
        switch (key) {
            case ProductType.Yellow:
                return materials.YellowColor;
            case ProductType.Brown:
                return materials.BrownColor;
            default:
                throw new ArgumentOutOfRangeException(nameof(key), key, null);
        }
    }
    public static Texture GetTexture(this ProductType key, GameSettingsInstaller.ProductSettings materials) {
        switch (key) {
            case ProductType.Yellow:
                return materials.Yellow;
            case ProductType.Brown:
                return materials.Brown;
            default:
                throw new ArgumentOutOfRangeException(nameof(key), key, null);
        }
    }
}