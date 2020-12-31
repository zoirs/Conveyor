using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrigController : MonoBehaviour
{
//    private void Start() {
//        Show();
//    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f) {
        GameObject line = new GameObject();
        line.transform.SetParent(gameObject.transform);
        line.transform.position = start;
        line.AddComponent<LineRenderer>();
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.material = Resources.Load("Materials/Grid", typeof(Material)) as Material;
        lr.startColor = color;
        lr.endColor = color;
        lr.SetWidth(0.02f, 0.02f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    public void Show() {
        float localScaleZ = gameObject.transform.localScale.z;
        float localScaleX = gameObject.transform.localScale.x;
        float delta = 0.0f;
        for (int i = (int) (localScaleZ * (5)); i <= (int) (localScaleZ * (15)); i++) {
            DrawLine(new Vector3(5 * localScaleX + delta, 0 + 1 * i+ delta, 4.9f), new Vector3(15 * localScaleX+ delta, 0 + 1 * i+ delta, 4.9f), Color.green);
        }

        for (int i = (int) (localScaleX * (5)); i <= (int) (localScaleX * (15)); i++) {
            DrawLine(new Vector3(0 + 1 * i+ delta, 5*localScaleZ+ delta, 4.9f), new Vector3(0 + 1 * i+ delta, 15*localScaleZ+ delta, 4.9f), Color.green);
        }
    }

    public void Hide() { }

}
