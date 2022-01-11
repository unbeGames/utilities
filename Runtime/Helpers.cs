using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.Text;
using UnityEngine;
using Unity.Burst;
using Newtonsoft.Json;
using Unity.Mathematics;

namespace Unbegames.Services {
  public static class Helpers {
    public static string CurrDateToFileName() {
      return DateTime.Now.ToString("yyyy_dd_M_HH_mm_ss");
    }

    public static string DeltaTimeStringFull(double delta) {
      var span = TimeSpan.FromSeconds(Math.Abs(delta));
      var symbol = delta >= 0 ? "-" : "+";
      return $"\nT {symbol} {span:dd\\:hh\\:mm\\:ss}";
    }

    public static string DeltaTimeStringSimple(double delta) {
      var symbol = delta >= 0 ? "-" : "+";
      return $"{symbol} {TimeSpan.FromSeconds(Math.Abs(delta)):dd\\:hh\\:mm\\:ss}";
    }

    public static string DurationString(double delta) {
      return $"{TimeSpan.FromSeconds(Math.Abs(delta)):mm\\:ss\\.f}";
    }

    public static string ConvertNumberToMoney(long value) {
      return (value / 100.0).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
    }

    public static void PutsList<T>(IEnumerable<T> list) {
      PutsList(string.Empty, list);
    }

    public static void PutsList<T>(string message, IEnumerable<T> list) {
      var str = new StringBuilder();
      foreach (var item in list) {
        str.Append(item);
        str.Append(" ");
      }
      GameDebug.Log($"{message}{str}");
    }

    [BurstDiscard]
    public static void Frame(object o, UnityEngine.Object context = null) {
      Debug.Log($"{Time.frameCount}: {o}", context);
    }

    public static void Puts(params object[] list) {
      if (list.Length > 1) {
        var sb = new StringBuilder();
        foreach (var obj in list) {
          sb.Append(obj);
          sb.Append(" ");
        }
        GameDebug.Log(sb.ToString());
      } else {
        PutsOne(list[0]);
      }
    }

    public static void PutsJson(params object[] list) {
      for (int i = 0; i < list.Length; i++) {
        list[i] = JsonConvert.SerializeObject(list[i], Formatting.Indented);
      }
      Puts(list);
    }

    public static void PutsOne(object o) {
      var result = o;
      if (o == null) {
        result = "null";
      }
      GameDebug.Log(result.ToString());
    }

    public static void NotImpl() {
      Error("This method is not implemented");
    }

    public static void Error(object obj, UnityEngine.Object context) {
      GameDebug.LogError(obj.ToString(), context);
    }

    public static void Warn(object obj) {
      GameDebug.LogWarning(obj.ToString());
    }

    public static void Exception(string message, Exception e) {
      GameDebug.LogException($"{message}: {e.Message}", e);
    }

    public static void Error(params object[] list) {
      foreach (var obj in list) {
        GameDebug.LogError(obj.ToString());
      }
    }

    public static Dictionary<k, v> ToDictionary<k, v>(List<k> keys, List<v> values) {
      return keys.ToDictionary(x => x, x => values[keys.IndexOf(x)]);
    }

    public static IEnumerator TimerAction(float rate, Action<float> action) {
      var time = 0.01f;
      while (time <= rate) {
        action.Invoke(time);
        time += Time.deltaTime;
        yield return null;
      }
    }

    public static T Instantiate<T>(GameObject prefab) {
      var gameObject = MonoBehaviour.Instantiate(prefab) as UnityEngine.GameObject;
      return gameObject.GetComponent<T>();
    }

    public static Dictionary<int, List<long>> Except(Dictionary<int, List<long>> first, Dictionary<int, List<long>> second) {
      var difference = new Dictionary<int, List<long>>();

      foreach (var key in first.Keys) {
        if (!second.ContainsKey(key)) {
          difference.Add(key, first[key]);
        } else {
          var list = first[key].Except(second[key]);
          if (list.Any()) {
            difference.Add(key, list.ToList<long>());
          }
        }
      }
      return difference;
    }

    public static IEnumerator DelayToEndOfFrame(Action action) {
      yield return null;
      action.Invoke();
    }

    public static IEnumerator Delay(float seconds, Action action) {
      yield return new WaitForSeconds(seconds);
      action.Invoke();
    }

    public static float VolumeToDb(float volume) {
      float result = -80f;
      if (volume > 0)
        result = 20f * Mathf.Log10(volume);
      return result;
    }

    public static float DbToVolume(float db) {
      return Mathf.Pow(10f, db / 20f);
    }

    public static void Select(GameObject obj) {
      EventSystem.current?.SetSelectedGameObject(obj);
    }

    public static GameObject Selected() {
      return EventSystem.current.currentSelectedGameObject;
    }

    public static IEnumerator LerpToColor(Renderer rend, float duration, Color from, Color to) {
      var time = 0f;
      while (time < duration) {
        var smooth = Mathf.SmoothStep(0f, 1f, time / duration);
        var currentColor = Color.Lerp(from, to, smooth);
        rend.material.SetColor("_EmissionColor", currentColor);
        yield return null;
        time += Time.deltaTime;
      }
    }

    public static IEnumerator LerpToColor(Light light, float duration, Color from, Color to) {
      var time = 0f;
      while (time < duration) {
        var smooth = Mathf.SmoothStep(0f, 1f, time / duration);
        var currentColor = Color.Lerp(from, to, smooth);
        light.color = currentColor;
        yield return null;
        time += Time.deltaTime;
      }
    }

    public static List<string> EnumToList(Type type) {
      return Enum.GetNames(type).ToList();
    }

    public static List<T> EnumToList<T>() {
      return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }

    public static T[] EnumToArray<T>() {
      return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
    }

    public static T ParseEnum<T>(string value, T defaultValue = default(T)) where T : struct {
      if (!Enum.TryParse<T>(value, out T result)) {
        result = defaultValue;
      }
      return result;
    }

    public static byte[] GetScreenShot(int width, int height) {
      var startX = Screen.width * 0.5f - width * 0.5f;
      var startY = Screen.height * 0.5f - height * 0.5f;

      var tex = new Texture2D(width, height, TextureFormat.RGB24, false, true);

      Rect rex = new Rect(startX, startY, width, height);

      tex.ReadPixels(rex, 0, 0);
      tex.Apply();

      var bytes = ImageConversion.EncodeToPNG(tex);
      GameObject.Destroy(tex);
      return bytes;
    }

    public static DateTime ConvertFromUnixTimestamp(uint timestamp) {
      DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
      return origin.AddSeconds(timestamp);
    }

    public static float NormalizedPosition(RectTransform rect, PointerEventData eventData) {
      RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out Vector2 pos);
      return pos.x / rect.rect.width;
    }

    public static void ResizeTexture(Texture2D source, int width, int height, bool linear) {
      RenderTexture rt = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32, linear ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB);
      rt.DiscardContents();
      Graphics.Blit(source, rt);
      RenderTexture.active = rt;
      source.Resize(width, height);
      source.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
      source.Apply(true);
      RenderTexture.active = null;
      rt.Release();
      GameObject.DestroyImmediate(rt);
    }

    public static float4 ColorTolinearFloat4(Color color) {
      color = color.linear;
      return new float4(color.r, color.g, color.b, color.a);
    }
  }
}

