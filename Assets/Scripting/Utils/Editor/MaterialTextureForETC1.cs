using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Reflection;

public class MaterialTextureForETC1 : MonoBehaviour
{
    public static float sizeScale = 1f;   
    private static string texPath = Application.dataPath + "/TestSplitTex";


   // [MenuItem("Assets/Create/创建通道分离图集", false)]
    public static void SelectObjectToPrefabs()
    {
        List<string> chooseObjPath = new List<string>();
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            string spritePath = AssetDatabase.GetAssetPath(Selection.objects[i]);
            chooseObjPath.Add(spritePath);
        }
        for (int i = 0; i < chooseObjPath.Count; i++)
        {
            string spritePath = chooseObjPath[i];
            Debug.Log(spritePath + " |" + Selection.objects[i].name);
            if (spritePath.EndsWith("png"))
            {
                SeperateRGBAandlphaChannel(spritePath);
            }
            else
            {
                TDebug.LogErrorFormat("不是png图片{0}" , spritePath);
            }
        }
    }

    #region process texture

    /// <summary>
    /// 返回材质路径
    /// </summary>
    /// <param name="_texPath"></param>
    /// <returns></returns>
    static string SeperateRGBAandlphaChannel(string _texPath)
    {
        string assetRelativePath = GetRelativeAssetPath(_texPath);
        SetTextureReadable(assetRelativePath);
        
        Texture2D sourcetex = AssetDatabase.LoadAssetAtPath(assetRelativePath, typeof(Texture2D)) as Texture2D;  //not just the textures under Resources file  
        if (!sourcetex)
        {
            Debug.LogError("Load Texture Failed : " + assetRelativePath);
            return "";
        }
        if (!HasAlphaChannel(sourcetex))
        {
            Debug.LogError("Texture does not have Alpha channel : " + assetRelativePath);
            return "";
        }
        
        string rgbTexPath = GetRGBTexPath(assetRelativePath);
        if (!Directory.Exists(FileBaseUtils.GetDirectoryPath(GetAlphaTexPath(_texPath))))
            Directory.CreateDirectory(FileBaseUtils.GetDirectoryPath(GetAlphaTexPath(_texPath)));

        Texture2D rgbTex = new Texture2D(sourcetex.width, sourcetex.height, TextureFormat.RGB24, false);
        Texture2D alphaTex = new Texture2D((int)(sourcetex.width * sizeScale), (int)(sourcetex.height * sizeScale), TextureFormat.RGB24, false);

        for (int i = 0; i < sourcetex.width; ++i)
            for (int j = 0; j < sourcetex.height; ++j)
            {
                Color color = sourcetex.GetPixel(i, j);
                Color rgbColor = color;
                Color alphaColor = color;
                alphaColor.r = color.a;
                alphaColor.g = color.a;
                alphaColor.b = color.a;
                if (color.a == 0) rgbTex.SetPixel(i, j, Color.white);  //透明度为0，设为白色
                else rgbTex.SetPixel(i, j, rgbColor);
                alphaTex.SetPixel((int)(i * sizeScale), (int)(j * sizeScale), alphaColor);
            }
        rgbTex.Apply();
        alphaTex.Apply();

        AssetDatabase.CopyAsset(_texPath, GetOriginTexPath(_texPath));

        byte[] bytes = rgbTex.EncodeToPNG();
        File.WriteAllBytes(GetRGBTexPath(_texPath), bytes);
        bytes = alphaTex.EncodeToPNG();
        File.WriteAllBytes(GetAlphaTexPath(_texPath), bytes);
       
        AssetDatabase.Refresh();
        FreshAlphaSet(GetAlphaTexPath(_texPath));
        FreshRbgSet(GetRGBTexPath(_texPath), GetOriginTexPath(_texPath));

        Material mat = AssetDatabase.LoadAssetAtPath<Material>(GetMatPath(_texPath));
        if (mat == null)
        {
            Shader uiShader = Shader.Find("UI/MyDefault");
            mat = new Material(uiShader);
            mat.SetTexture("_AlphaTex", (Texture) AssetDatabase.LoadAssetAtPath(GetAlphaTexPath(_texPath), typeof (Texture)));
            AssetDatabase.CreateAsset(mat, GetMatPath(_texPath));
        }
        else
        {
            mat.SetTexture("_AlphaTex", (Texture) AssetDatabase.LoadAssetAtPath(GetAlphaTexPath(_texPath), typeof (Texture)));
        }

        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        AssetDatabase.SaveAssets();

        Material mmmat = AssetDatabase.LoadAssetAtPath<Material>(MaterialTextureForETC1.GetMatPath(_texPath));
        if (mmmat == null)
            Debug.LogError("创建材质失败" + GetMatPath(_texPath));
        Debug.Log(assetRelativePath + "Succeed to seperate RGB and Alpha channel for texture : ");
        return GetMatPath(_texPath);
    }



    public static void FreshAlphaSet(string alphaPath)
    {
        TextureImporter texImp = (TextureImporter)TextureImporter.GetAtPath(alphaPath);
        texImp.filterMode = FilterMode.Bilinear;
        texImp.wrapMode = TextureWrapMode.Clamp;
        texImp.alphaSource = TextureImporterAlphaSource.None;
        texImp.alphaIsTransparency = false;
        texImp.mipmapEnabled = false;
        AssetDatabase.ImportAsset(alphaPath);
    }
    public static void FreshRbgSet(string rbgPath, string originTexPath)
    {
        TextureImporter texImp = (TextureImporter)TextureImporter.GetAtPath(rbgPath);
        TextureImporter originTexImp = (TextureImporter)TextureImporter.GetAtPath(originTexPath);
        //originTexImp.textureType = TextureImporterType.Default;
        texImp.filterMode = FilterMode.Bilinear;
        texImp.wrapMode = TextureWrapMode.Clamp;
        texImp.isReadable = false;
        texImp.textureType = TextureImporterType.Sprite;
        texImp.alphaSource = TextureImporterAlphaSource.None;
        texImp.alphaIsTransparency = false;
        texImp.spriteImportMode = originTexImp.spriteImportMode;
        texImp.spriteBorder = originTexImp.spriteBorder;
        texImp.spritePivot = originTexImp.spritePivot;
        texImp.spritesheet = originTexImp.spritesheet;
        texImp.mipmapEnabled = false;
        AssetDatabase.ImportAsset(rbgPath);
        originTexImp.spriteImportMode = SpriteImportMode.Single;
        AssetDatabase.ImportAsset(originTexPath);
    }


    static bool HasAlphaChannel(Texture2D _tex)
    {
        for (int i = 0; i < _tex.width; ++i)
            for (int j = 0; j < _tex.height; ++j)
            {
                Color color = _tex.GetPixel(i, j);
                float alpha = color.a;
                if (alpha < 1.0f - 0.001f)
                {
                    return true;
                }
            }
        return false;
    }

    static void SetTextureReadable(string _relativeAssetPath)
    {
        string postfix = GetFilePostfix(_relativeAssetPath);
        if (postfix == ".dds")    // no need to set .dds file.  Using TextureImporter to .dds file would get casting type error.  
        {
            TDebug.LogErrorFormat("只支持png格式:{0}",_relativeAssetPath);
            return;
        }

        TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(_relativeAssetPath);
        ti.isReadable = true;
        ti.alphaSource = TextureImporterAlphaSource.FromInput;
        AssetDatabase.ImportAsset(_relativeAssetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    #endregion


    #region string or path helper

    static bool IsTextureFile(string _path)
    {
        string path = _path.ToLower();
        return path.EndsWith(".psd") || path.EndsWith(".tga") || path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".dds") || path.EndsWith(".bmp") || path.EndsWith(".tif") || path.EndsWith(".gif");
    }

    public static string GetRGBTexPath(string _texPath)
    {
        return GetTexPath(_texPath, ".");
        //_texPath = _texPath.Insert(_texPath.LastIndexOf('/') + 1, "split/");
        //return GetTexPath(_texPath, ".");
    }

    static string GetOriginTexPath(string _texPath)
    {
        _texPath = _texPath.Insert(_texPath.LastIndexOf('/') + 1, "split/");
        return GetTexPath(_texPath, "_Origin.");
        //return GetTexPath(_texPath, ".");
    }

    static string GetAlphaTexPath(string _texPath)
    {
        _texPath = _texPath.Insert(_texPath.LastIndexOf('/') + 1, "split/");
        return GetTexPath(_texPath, "_Alpha.");
    }

    public static string GetMatPath(string _texPath)
    {
        _texPath = _texPath.Insert(_texPath.LastIndexOf('/') + 1, "split/");
        string tex = GetTexPath(_texPath, ".");
        return tex.Replace(".png", ".mat");
    }
    static string GetTexPath(string _texPath, string _texRole)
    {
        string result = _texPath.Replace(".", _texRole);
        string postfix = GetFilePostfix(_texPath);
        return result.Replace(postfix, ".png");
    }

    static string GetRelativeAssetPath(string _fullPath)
    {
        _fullPath = GetRightFormatPath(_fullPath);
        int idx = _fullPath.IndexOf("Assets");
        string assetRelativePath = _fullPath.Substring(idx);
        return assetRelativePath;
    }

    static string GetRightFormatPath(string _path)
    {
        return _path.Replace("\\", "/");
    }

    static string GetFilePostfix(string _filepath)   //including '.' eg ".tga", ".dds"  
    {
        string postfix = "";
        int idx = _filepath.LastIndexOf('.');
        if (idx > 0 && idx < _filepath.Length)
            postfix = _filepath.Substring(idx, _filepath.Length - idx);
        return postfix;
    }

    #endregion
}