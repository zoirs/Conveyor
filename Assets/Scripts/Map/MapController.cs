using System.Collections;
using System.Collections.Generic;
using Map;
using UnityEngine;

using Zenject;

public class MapController : MonoBehaviour {

    private float mouseMoveTimer;
    private float mouseMoveTimerMax = .01f;

    [Inject] private MapService _mapService;
    [Inject] private SignalBus _signalBus;

    private int[,] map;

    private Mesh mesh;
    private MeshFilter meshFilter;
    private bool needChangeLevel;

    private void Start() {
        map = _mapService.Map;
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;

//        UpdateMapVisual();
//        StartCoroutine(ChangeLevel());
//        StartCoroutine(UpdateMapVisual());
    }

    IEnumerator ChangeLevel() {
        yield return new WaitForSeconds(30);
        needChangeLevel = true;
        StartCoroutine(ChangeLevel());
    }

    private void Update() {
        if (needChangeLevel) {
            _mapService.currentLevel++;
            if (_mapService.currentLevel > 1) {
                _signalBus.Fire<ChangeLevelSignal>();
            }

//            StartCoroutine(UpdateMapVisual());
            needChangeLevel = false;
        }
    }

//    IEnumerator UpdateMapVisual() {
//        Vector3[] vertices;
//        Vector2[] uv;
//        int[] triangles;
//
//        MeshUtils.CreateEmptyMeshArrays(map.GetLength(0) * map.GetLength(0), out vertices, out uv, out triangles);
//
//        for (int x = 0; x < map.GetLength(0); x++) {
//            for (int y = 0; y < map.GetLength(0); y++) {
//                Debug.Log(map.GetLength(0) + " x "+ x + " y "+ y);
//                int index = x * map.GetLength(0) + y;
//                Vector3 baseSize = new Vector3(1, 1) * 1;
//                int gridValue = map[x, y];
//                if (gridValue > _mapService.currentLevel) {
//                    continue;
//                }
//
//                int maxGridValue = 12;
//                float gridValueNormalized = Mathf.Clamp01((float)gridValue / maxGridValue);
//                Vector2 gridCellUV = new Vector2(gridValueNormalized, 0f);
//                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, _mapService.GetWorldPosition(x, y) + baseSize * .5f, 0f, baseSize, gridCellUV, gridCellUV);
//            }
//        }
//
//        mesh.vertices = vertices;
//        mesh.uv = uv;
//        mesh.triangles = triangles;
//        yield break;
//    }


}
