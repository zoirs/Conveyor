using System;
using UnityEngine;

namespace Line {
    public enum LineType {
        RED, BLUE, NOT_USED
    }

    public static class LineTypeExtension {
        public static Material GetMaterial(this LineType lineType, GameSettingsInstaller.LineMaterialsSettings materials) {
            switch (lineType) {
                case LineType.RED:
                    return materials.Red;
                case LineType.BLUE:
                    return materials.Blue;
                case LineType.NOT_USED:
                    return materials.NotUsed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lineType), lineType, null);
            }
        }
    }
}