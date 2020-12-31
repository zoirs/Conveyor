using Line;
using UnityEngine;

namespace DefaultNamespace {
    public interface Company {
        GameObject gameObject { get ; }
        
        LineController GetNextLine(LineController currentLine);
        void AddLine(LineController lineController);
    }
}