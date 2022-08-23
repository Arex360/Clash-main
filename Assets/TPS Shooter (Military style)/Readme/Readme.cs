using System;
using UnityEngine;

public class Readme : ScriptableObject
{
  public Header header;
  public Section[] sections;

  [Serializable]
  public class Header
  {
    public Texture2D icon;
    public string title;
  }

  [Serializable]
  public class Section
  {
    public string heading, text, linkText, url;
  }
}
