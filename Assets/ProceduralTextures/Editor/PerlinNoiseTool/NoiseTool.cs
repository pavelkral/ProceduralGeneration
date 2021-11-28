using System.IO;
using UnityEditor;
using UnityEngine;

public class NoiseTool : EditorWindow
{
     [MenuItem("ProceduralTexture/Noise Tools")]
     static void AddWindow()
     {
          Rect wr = new Rect(0, 0, 400, 600);
          NoiseTool window = (NoiseTool)EditorWindow.GetWindowWithRect(typeof(NoiseTool), wr, true, "Noise Tool");
          window.Show();
     }

     public enum NoiseType { Perlin, Value, Simplex, Worley };
     public enum GenerationMode { None, Abs, Sin };
     public enum TextureSize { x64 = 64, x128 = 128, x256 = 256, x512 = 512, x1024 = 1024, x2048 = 2048 };

     private ComputeShader computeShader;
     private NoiseType noiseType = NoiseType.Perlin;
     private GenerationMode generation = GenerationMode.None;
     private RenderTextureFormat format = RenderTextureFormat.ARGB32;
     private TextureSize size = TextureSize.x512;
     private float scale = 10f;

     RenderTexture renderTexture;
     int kernel;
     Texture2D texture;
     string path = "Assets/test.tga";

     private void OnGUI()
     {
          computeShader = EditorGUILayout.ObjectField("Compute Shader:", computeShader, typeof(ComputeShader), true) as ComputeShader;
          noiseType = (NoiseType)EditorGUILayout.EnumPopup("Noise Type:", noiseType);
          generation = (GenerationMode)EditorGUILayout.EnumPopup("Generation Type:", generation);
          format = (RenderTextureFormat)EditorGUILayout.EnumPopup("Texture Type:", format);
          size = (TextureSize)EditorGUILayout.EnumPopup("Texture size:", size);
          scale = EditorGUILayout.Slider("Noise level:", scale, 1f, 40f);

          if (GUILayout.Button("Generate a noise map"))
          {
               if (computeShader == null)
               {
                    ShowNotification(new GUIContent("Compute Shader cannot be empty!"));
               }
               else
               {
                    Init();
               }
          }
          if (renderTexture != null)
          {
               int x = 390;
               Rect rect = new Rect(5, 180, x, x);
               GUI.DrawTexture(rect, renderTexture);
          }
          if (GUILayout.Button("save"))
          {
               if (renderTexture == null)
               {
                    ShowNotification(new GUIContent("The sticker is empty!"));
               }
               else
               {
                    SaveTexture();
                    AssetDatabase.Refresh();
                    ShowNotification(new GUIContent("Saved successfully!"));
               }
          }
     }

     private RenderTexture CreateRT(int size)
     {
          RenderTexture renderTexture = new RenderTexture(size, size, 0, format);
          renderTexture.enableRandomWrite = true;
          renderTexture.wrapMode = TextureWrapMode.Repeat;
          renderTexture.Create();
          return renderTexture;
     }

     void Init()
     {
          renderTexture = CreateRT((int)size);
          kernel = computeShader.FindKernel("PerlinNoise");
          computeShader.SetTexture(kernel, "ResultTexture", renderTexture);
          computeShader.SetInt("size", (int)size);
          computeShader.SetFloat("scale", scale * 10f);
          computeShader.SetInt("Type", (int)noiseType);
          computeShader.SetInt("State", (int)generation);
          computeShader.Dispatch(kernel, (int)size / 8, (int)size / 8, 1);
     }

     private void OnDisable()
     {
          if (renderTexture == null) return;
          renderTexture.Release();
     }

     void SaveTexture()
     {
          RenderTexture previous = RenderTexture.active;
          RenderTexture.active = renderTexture;
          texture = new Texture2D(renderTexture.width, renderTexture.height);
          texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
          texture.Apply();
          RenderTexture.active = previous;

          byte[] bytes = texture.EncodeToTGA();
          File.WriteAllBytes(path, bytes);
     }
}

