using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private Camera _camera;
    
    private void Update() {
        // https://forum.unity.com/threads/tile-map-tearing-problems.225777/#post-1507246
        // из за ошибки огругления мерцают линии, todo попробовать делать целые значения
        //_camera.orthographicSize = _camera.orthographicSize + Time.deltaTime * 0.01f;
    }
}