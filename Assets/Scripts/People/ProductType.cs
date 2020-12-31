using System;
using UnityEngine;

public enum ProductType {
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
            default:
                throw new ArgumentOutOfRangeException(nameof(key), key, null);
        }
    }
}