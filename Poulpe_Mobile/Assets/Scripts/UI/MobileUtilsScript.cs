using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

    public class MobileUtilsScript : MonoBehaviour
    {
        private TextMeshProUGUI textJumpCount;

    private void Start()
    {
        textJumpCount = GetComponent<TextMeshProUGUI>();
    }

    float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        textJumpCount.SetText(text);
    }
}

